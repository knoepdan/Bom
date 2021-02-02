using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bom.Web.Common
{

    public static class Const
    {
        public const string DefaultLang = "en";


        public static class RouteArgumentNames
        {
            public const string Lang = "lang";

            /// <summary>
            /// action: built in
            /// </summary>
            public const string Action = "action";

            /// <summary>
            /// controller  -> built in
            /// </summary>
            public const string Controller = "controller";
        }
    }
}


