using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ch.Knomes.Localization.Store;

namespace Ch.Knomes.Localization.Resolver
{
    public interface ITextResolver
    {
        ITextItem? GetTextItem(IEnumerable<ITextItem> translations);
    }
}
