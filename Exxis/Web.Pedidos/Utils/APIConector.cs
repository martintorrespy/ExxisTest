using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;

namespace Web.Pedidos.Utils
{
     
    public class APIConector
    {
        public static HttpClient httpClient = new HttpClient();

        static APIConector()
        {
            httpClient.BaseAddress = new Uri("http://localhost:1754/api/");
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        }
    }
}