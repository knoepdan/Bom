
namespace Ch.Knomes.Localization.Store
{
    public class LanguageCodeInfo
    {
        internal LanguageCodeInfo(string langCode, int count)
        {
            this.LanguageCode = langCode;
            this.Count = count;
        }

        public string LanguageCode { get; } = default!;

        public int Count { get; } = default!;
    }
}