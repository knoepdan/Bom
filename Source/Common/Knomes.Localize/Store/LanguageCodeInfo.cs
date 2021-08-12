
namespace Knomes.Localize.Store
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

        public override string ToString()
        {
            return $"[{nameof(LanguageCodeInfo)}] {this.LanguageCode}: {this.Count}";
        }
    }
}