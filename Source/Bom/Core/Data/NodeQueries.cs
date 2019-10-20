using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Bom.Core.Model;
using System.Globalization;

namespace Bom.Core.Data
{
    public static class NodeQueries
    {

        public static IQueryable<Node> Orphans(this IQueryable<Node>? list)
        {
            if (list == null)
            {
                throw new ArgumentNullException(nameof(list));
            }

            list = list.Where(x => !x.Paths.Any());
            return list;
        }
    }
}
