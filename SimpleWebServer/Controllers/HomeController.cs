using System;
using System.Net;

namespace SimpleWebServer.Controllers
{
    public static class HomeController
    {
        public static void HandleRequest(HttpListenerContext context)
        {
            string response = "Hello Browser!";
            byte[] encoded = System.Text.Encoding.UTF8.GetBytes(response);
            context.Response.ContentLength64 = encoded.Length;
            context.Response.OutputStream.Write(encoded, 0, encoded.Length);
            context.Response.OutputStream.Close();
        }
    }
}
