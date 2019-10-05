using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;


namespace Bom.Web.Lib.Infrastructure.ErrorHandling
{
    public static class ErrorUtility
    {

        public static async Task SetErrorResponse(HttpContext context)
        {
            if(context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            // set defaults
            context.Response.ContentType = "application/json"; // "text/html";
            var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
            context.Response.StatusCode = GetHttpStatusCodeFromException(exceptionHandlerPathFeature?.Error); // default
            var errorInfo = GetErrorInfo(exceptionHandlerPathFeature?.Error, exceptionHandlerPathFeature?.Path);

            // get error info
            var json = JsonConvert.SerializeObject(errorInfo);
            await context.Response.WriteAsync(json, System.Text.Encoding.UTF8);
        }

        private static int GetHttpStatusCodeFromException(Exception? ex)
        {
            if (ex == null)
            {
                return 500;
            }

            var appEx = GetAppException(ex);
            if (appEx != null)
            {
                switch (appEx.ErrorCode)
                {
                    case Core.Error.ErrorCode.NotFound:
                        return 404;
                    case Core.Error.ErrorCode.Forbidden:
                        return 403;
                    default:
                        return 400;
                }
            }
            return 500;
        }

        public static string GetRequestInfo(HttpContext context, bool includeExInfo)
        {
            if (context == null)
            {
                return "Unknown, context null";
            }

            var sb = new System.Text.StringBuilder();
            if (includeExInfo)
            {
                var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                if (exceptionHandlerPathFeature != null && exceptionHandlerPathFeature.Error != null)
                {
                    sb.AppendLine(exceptionHandlerPathFeature.Error.ToString());
                }
                if (exceptionHandlerPathFeature != null && !string.IsNullOrEmpty(exceptionHandlerPathFeature.Path))
                {
                    sb.AppendLine("Path: " + exceptionHandlerPathFeature.Path);
                }
            }
            if (context.Request != null)
            {
                sb.AppendLine($"Path: {context.Request.Path}  Querystring: {context.Request.QueryString}  [Method: {context.Request.Method}]");
            }
            // possible improvment: add more info
            return sb.ToString();
        }

        private static ErrorInfo GetErrorInfo(Exception? ex, string? path)
        {
            var info = new ErrorInfo();
            info.Message = $"Error in path {path}, ex: {ex?.GetType()?.Name}";
            var appEx = GetAppException(ex);
            if (appEx != null)
            {
                info.ErrorCode = (int)appEx.ErrorCode;
                info.UserMessage = appEx.UserMessage;
                // info.Message.. maybe also give a better technical error message (but not for now)
            }

            return info;
        }

        private static Core.Error.BomException? GetAppException(Exception? ex)
        {
            while (ex != null)
            {
                var appEx = ex as Core.Error.BomException;
                if (appEx != null)
                {
                    return appEx;
                }
                ex = ex.InnerException;
            }
            return null;
        }

    }
}
