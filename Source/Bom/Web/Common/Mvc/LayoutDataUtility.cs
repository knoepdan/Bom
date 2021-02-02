using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Bom.Web.Common.Mvc
{
    public static class LayoutDataUtility
    {

        /// <summary>
        /// Model for a data that has to be available in the view
        /// </summary>
        /// <param name="viewData">ViewData (in view accessible via: this.ViewData in RazorView)</param>
        /// <exception cref="Exception">throws if not available</exception>
        public static LayoutData LayoutData(this ViewDataDictionary<dynamic> viewData)
        {
            LayoutData? layoutData = GetLayoutDataOrNull(viewData);
            if (layoutData == null)
            {
                throw new Exception("layout data is not set");
            }
            return layoutData;
        }

        public static LayoutData LayoutData(this ViewDataDictionary viewData)
        {
            LayoutData? layoutData = GetLayoutDataOrNull(viewData);
            if (layoutData == null)
            {
                throw new Exception("layout data is not set");
            }
            return layoutData;
        }


        public static LayoutData? GetLayoutDataOrNull(ViewDataDictionary viewData)
        {
            var layoutData = viewData["LayoutData"] as LayoutData;
            return layoutData;
        }

        internal static void SetLayoutData(this ViewDataDictionary viewData, LayoutData layoutData)
        {
            viewData["LayoutData"] = layoutData;
        }

    }
}
