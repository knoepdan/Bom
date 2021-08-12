using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Html;
using Ch.Knomes.TestUtils.Resources;
using Knomes.Localize.TestUtils;
using System.Web;
using Knomes.Localize.Resolver;
using Knomes.Localize.Store;

using Xunit;

namespace Knomes.Localize
{
    public class TextserviceTests
    {
        [Fact]
        public void Textservice_returns_expected_text_results_for_fixed_and_todo()
        {
            ITextService service = CreateTextservice();

            Assert.True(service.Fixed("aa") == "aa");
            Assert.True(service.Fixed("aa {0}", 1, 2) == "aa 1");

            Assert.True(service.Todo("code", "aa") == "aa");
            Assert.True(service.Todo("code", "aa {0}", 1, 44) == "aa 1"); // too many params just first is taken
            var result = service.Todo("code", "aa {0} {1}", 1); // not enough params
            Assert.True(result == "aa {0} {1}");// text is returned

            Assert.True(service.Localize("code", "aa") == "aa");
            Assert.True(service.Localize("code", "aa {0}", 1, 2) == "aa 1");
            Assert.True(service.Localize("code", "aa {0} {1}", 1, 2) == "aa 1 2");
        }
        [Fact]
        public void Textservice_returns_expected_text_results()
        {
            ITextService service = CreateTextservice();

            // Test fallbacks (codes don't exist)
            Assert.True(service.Localize("code", "aa") == "aa");
            Assert.True(service.Localize("code", "aa {0}", 1, 2) == "aa 1");
            Assert.True(service.Localize("code", "aa {0} {1}", 1, 2) == "aa 1 2");

            // Test code that exists
            using (var langSwitch = service.Resolver.GetTemporayLanguageSwitch("en"))
            {
                Assert.True(service.Localize("Common.Save", "aa") == "Save");
                Assert.True(service.Localize("Test.Smile", "aa {0}", 1, 2) == "<div>Smile 1</div>"); // <div>Smile {0}</div>   (TEXT)
            }
            using (var langSwitch = service.Resolver.GetTemporayLanguageSwitch("de"))
            {
                Assert.True(service.Localize("Common.Save", "aa") == "Speichern");
                Assert.True(service.Localize("Test.Smile", "aa {0}", 1, 2) == "<div>Lachen 1</div>"); // <div>Lachen {0}</div>   (HTML)
            }
        }

        [Fact]
        public void Textservice_returns_expected_html_results_for_fixed_and_todo()
        {
            IHtmlService service = CreateTextservice();

            Assert.True(service.FixedHtml("<div>aa</div>").ToString() == "<div>aa</div>");
            Assert.True(service.FixedHtml("aa").ToString() == "aa");
            var fixedParamString = service.FixedHtml("aa {0} {1}  {2}", 1, "<div>htmlParam</div>", new HtmlString("<div>htmlParam</div>")).ToString();
            Assert.True(fixedParamString.StartsWith("aa 1") && fixedParamString.EndsWith("<div>htmlParam</div>") && fixedParamString != "aa 1 <div>htmlParam</div> <div>htmlParam</div>");

            Assert.True(service.TodoHtml("code", "aa").ToString() == "aa");
            Assert.True(service.TodoHtml("code", "aa {0}", 1, 44).ToString() == "aa 1"); // too many params just first is taken
            var result = service.TodoHtml("code", "aa {0} {1}", 1).ToString(); // not enough params
            Assert.True(result == "aa {0} {1}");// text is returned
    
        }

        [Fact]
        public void Textservice_returns_expected_html_results_fallbacks()
        {
            IHtmlService service = CreateTextservice();

            Assert.True(service.LocalizeHtml("code", "aa").ToString() == "aa");
            var fixedLocString = service.LocalizeHtml("code", "aa {0} {1}  {2}", 1, "<div>htmlParam</div>", new HtmlString("<div>htmlParam</div>")).ToString();
            Assert.True(fixedLocString.StartsWith("aa 1") && fixedLocString.EndsWith("<div>htmlParam</div>") && fixedLocString != "aa 1 <div>htmlParam</div> <div>htmlParam</div>");
        }

        [Fact]
        public void Textservice_returns_expected_html_results()
        {
            IHtmlService service = CreateTextservice();

            // Test code that exists
            using (var langSwitch = service.Resolver.GetTemporayLanguageSwitch("en"))
            {
                Assert.True(service.LocalizeHtml("Common.Save", "aa").ToString() == "Save"); // basic

                // Does html encode content text.Type is "Text" -> see json
                Assert.True(service.LocalizeHtml("Test.Smile", "aa {0}", 1, 2).ToString() != "<div>Smile 1</div>"); // <div>Smile {0}</div>   (TEXT)

                // Test with Parameters: Does html encode content text.Type is "Text" -> see json   
                var localizedString = service.LocalizeHtml("Test.Smile", "aa {0}", new HtmlString("<div>PARAM</div>")).ToString();  // <div>Smile {0}</div>   (TEXT)
                Assert.True(localizedString != "<div>Smile <div>PARAM</div></div>" && localizedString.Contains("<div>PARAM</div>"));
                var localizedString2 = service.LocalizeHtml("Test.Smile", "aa {0}", "<div>PARAM</div>").ToString();  // <div>Smile {0}</div>   (TEXT)
                Assert.True(localizedString2 != "<div>Smile <div>PARAM</div></div>" && !localizedString2.Contains("<div>PARAM</div>")); // param is also encoded
                var decoded = HttpUtility.HtmlDecode(localizedString2);
                Assert.True(decoded == "<div>Smile <div>PARAM</div></div>");
            }
            using (var langSwitch = service.Resolver.GetTemporayLanguageSwitch("de"))
            {
                Assert.True(service.LocalizeHtml("Common.Save", "aa").ToString() == "Speichern");

                // Does html NOT encode content text.Type is "Html" -> see json
                Assert.True(service.LocalizeHtml("Test.Smile", "aa {0}", 2).ToString() == "<div>Lachen 2</div>"); // <div>Lachen {0}</div>   (HTML)

                // Test with Parameters that are to be encoded: Does NOT html encode content text.Type is "Text" -> see json   
                var localizedString = service.LocalizeHtml("Test.Smile", "aa {0}", new HtmlString("<div>PARAM</div>")).ToString();  // <div>Lachen {0}</div>   (HTML)
                Assert.True(localizedString == "<div>Lachen <div>PARAM</div></div>"); // html encoding occurs because content and param are to be treated as intended html
                var localizedString2 = service.LocalizeHtml("Test.Smile", "aa {0}", "<div>PARAM</div>").ToString();  // <div>Lachen {0}</div>   (HTML)
                Assert.True(localizedString2 != "<div>Lachen <div>PARAM</div></div>" && !localizedString2.Contains("<div>PARAM</div>")); // param is also encoded
                var decoded = HttpUtility.HtmlDecode(localizedString2);
                Assert.True(decoded == "<div>Lachen <div>PARAM</div></div>");

            }
        }

        private Textservice<LocalizationStore, CurrentThreadTextResolver>  CreateTextservice()
        {
            var store = LocalizationTestUtils.GetLocacationStore()!;
            return new Textservice<LocalizationStore, CurrentThreadTextResolver>(store, new CurrentThreadTextResolver());
        }

    }
}