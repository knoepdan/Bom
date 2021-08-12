using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Knomes.Localize.Store
{
    public interface ITextItem
    {
        /// <summary>
        /// Language code suach as "en" or "en-us"
        /// </summary>
        public string LangCode { get; }

        public string Text { get;  } 

        public TextType Type { get; } 
    }

    public interface ITextItemWithCode : ITextItem
    {
        public string Code { get; }
    }


    public class BasicTextItem : ITextItem
    {
        public string LangCode { get; set; } = default!;

        public string Text { get; set; } = default!;

        public TextType Type => TextType.Undefined;

        public override string ToString()
        {
            return $"'{Text}' ({LangCode} / {Type})";
        }
    }

    public class TextItem : ITextItem
    {
        public string LangCode { get; set; } = default!;

        public string Text { get; set; } = default!;

        public TextType Type { get; set; } = TextType.Undefined;

        public override string ToString()
        {
            return $"'{Text}' ({LangCode} / {Type})";
        }
    }

    public class CodeTextItem : ITextItemWithCode
    {
        public CodeTextItem() { }

        public CodeTextItem(string code, string langCode, string text, TextType textType = TextType.Undefined) {
            this.Code = code;
            this.LangCode = langCode;
            this.Text = text;
            this.Type = textType;
        }

        public string Code { get; set; } = default!;

        public string LangCode { get; set; } = default!;

        public string Text { get; set; } = default!;

        public TextType Type { get; set; } = TextType.Undefined;

        public override string ToString()
        {
            return $"{Code}: '{Text}' ({LangCode} / {Type})";
        }
    }
}
