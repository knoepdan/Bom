using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace Bom.Core.TestUtils
{
    public static class ComparisonUtils
    {
        public static bool HasSameContent<T>(IList<T> listA, IList<T> listB)
        {
            bool hasSameContent = listA.All(listB.Contains) && listA.Count == listB.Count;
            return hasSameContent;
        }

        public static bool HasDuplicates<T>(IEnumerable<T> list)
        {
            var count = list.Count();
            if (list.Distinct().Count() == count)
            {
                return false;
            }
            return true;
        }
        public static void ThrowIfDuplicates<T>(IEnumerable<T> list)
        {
            bool hasDuplicates = HasDuplicates(list);
            if (hasDuplicates)
            {
                throw new Exception($"List contains duplictes!! Count: {list.Count()}");
            }
        }
    }
}