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
            if (listA == null)
            {
                throw new ArgumentNullException(nameof(listA));
            }
            if (listB == null)
            {
                throw new ArgumentNullException(nameof(listB));
            }
            bool hasSameContent = listA.All(listB.Contains) && listA.Count == listB.Count;
            return hasSameContent;
        }

        public static bool HasSameContentInSameOrder<T>(IEnumerable<T> colA, IEnumerable<T> colB)
        {
            if (colA == null)
            {
                throw new ArgumentNullException(nameof(colA));
            }
            if (colB == null)
            {
                throw new ArgumentNullException(nameof(colB));
            }
            var listA = colA.ToList();
            var listB = colB.ToList();
            if (listA.Count != listB.Count)
            {
                return false;
            }
            for (int i = 0; i < listA.Count; i++)
            {
                var a = listA[i];
                var b = listB[i];
                if (a == null)
                {
                    if (b != null)
                    {
                        return false;
                    }
                    continue;
                }
                if (!a.Equals(b))
                {
                    return false;
                }
            }
            return true;
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