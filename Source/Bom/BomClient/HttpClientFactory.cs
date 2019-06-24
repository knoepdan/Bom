using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;

namespace BomClient
{
    public static class HttpClientFactory
    {

        public static HttpClient CreateHttpClient()
        {
            var client = new HttpClient();
            return client;
        }
    }
}