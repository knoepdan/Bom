using Microsoft.AspNetCore.Html;

using Xunit;

namespace Ch.Knomes.Localization
{
    public class DummyTextserviceTests
    {
        [Fact]
        public void DummyTextservice_returns_expected_text_results()
        {
            ITextService service = new DummyTextservice(true);

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
        public void DummyTextservice_returns_expected_html_results()
        {
            IHtmlService service = new DummyTextservice(true);

            Assert.True(service.FixedHtml("<div>aa</div>").ToString() == "<div>aa</div>");
            Assert.True(service.FixedHtml("aa").ToString() == "aa");
            var fixedParamString = service.FixedHtml("aa {0} {1}  {2}", 1, "<div>htmlParam</div>", new HtmlString("<div>htmlParam</div>")).ToString();
            Assert.True(fixedParamString.StartsWith("aa 1") && fixedParamString.EndsWith("<div>htmlParam</div>") && fixedParamString != "aa 1 <div>htmlParam</div> <div>htmlParam</div>");

            Assert.True(service.TodoHtml("code", "aa").ToString() == "aa");
            Assert.True(service.TodoHtml("code", "aa {0}", 1, 44).ToString() == "aa 1"); // too many params just first is taken
            var result = service.TodoHtml("code", "aa {0} {1}", 1).ToString(); // not enough params
            Assert.True(result == "aa {0} {1}");// text is returned

            var fixedLocString = service.LocalizeHtml("code", "aa {0} {1}  {2}", 1, "<div>htmlParam</div>", new HtmlString("<div>htmlParam</div>")).ToString();
            Assert.True(fixedLocString.StartsWith("aa 1") && fixedLocString.EndsWith("<div>htmlParam</div>") && fixedLocString != "aa 1 <div>htmlParam</div> <div>htmlParam</div>");

        }
    }
}