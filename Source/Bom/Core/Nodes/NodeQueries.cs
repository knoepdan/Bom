using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Bom.Core.Nodes.DbModels;
using System.Globalization;

namespace Bom.Core.Nodes
{
    public static class NodeQueries
    {

        public static IQueryable<Node> Orphans(this IQueryable<Node>? list)
        {
            if (list == null)
            {
                throw new ArgumentNullException(nameof(list));
            }

            list = list.Where(x => x.Paths != null && !x.Paths.Any());
            return list;
        }
    }
}
