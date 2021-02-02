using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Text;
using Microsoft.AspNetCore.Routing;


namespace Bom.Web.Common.Mvc.Utils
{
    public static class MvcLinkUtility
    {

        /*
        public static RouteValueDictionary ReplaceValueInRoute(RouteValueDictionary existingDic, string routeName, string value)
        {
            var newRouteDic = new RouteValueDictionary(existingDic);
            newRouteDic[routeName] = value;
            return newRouteDic;
        }

        public static string CreateUrl(string pathBase, RouteValueDictionary routeDic, Microsoft.AspNetCore.Http.IQueryCollection? query)
        {
            const char sep = '/';
            var sb = new StringBuilder();
            if (!string.IsNullOrEmpty(pathBase))
            {
                sb.Append(sep);
                sb.Append(Uri.EscapeDataString(pathBase.TrimStart(sep)));
            }


            throw new Exception("Approach doesnt work because order in RouteValueDictionary is not defined.. maybe only when there is no route name)
            // Initially I suspected the values in routeDic are in reverse order, so reversing order would have been the fix but this is not the case
            Bom.Utils.Dev.Todo("Is the order in the RouteValueDictionary really defined?", Bom.Utils.Dev.Urgency.Middle);
            var ar = routeDic.Values.ToArray();
            for(int i = ar.Length-1; i >= 0; i--)
            {
                var route = ar[i];
                if (route != null)
                {
                    sb.Append(sep);
                    var val = route.ToString()!;
                    sb.Append(Uri.EscapeDataString(val));
                }
            }
 
            // Add query string
            var queryString = CreateUrlQueryString(query);
            if (!string.IsNullOrEmpty(queryString))
            {
                sb.Append("?");
                sb.Append(queryString);
            }
            return sb.ToString();
        }

        private static string CreateUrlQueryString(Microsoft.AspNetCore.Http.IQueryCollection? query)
        {
            if(query != null)
            {
                var sb = new StringBuilder("");
                foreach(var keyVal in query)
                {
                    foreach(var val in keyVal.Value)
                    {
                        sb.Append(Uri.EscapeDataString(keyVal.Key));
                        sb.Append("=");
                        sb.Append(Uri.EscapeDataString(val));
                        sb.Append("&");
                    }
                }
                return sb.ToString().TrimEnd('&');
            }
            return "";
        }
        */
    }
}
