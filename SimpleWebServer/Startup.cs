using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using SimpleWebServer.Controllers;

namespace SimpleWebServer
{
    public static class Startup
    {
        private static HttpListener listener;
        private static Semaphore sem = new Semaphore(30, 30);

        public static void Start()
        {
            listener = new HttpListener();
            listener.Prefixes.Add("http://localhost/");
            listener.Start();
            Task.Run(() => RunServer());
        }

        private static void RunServer()
        {
            while (true)
            {
                sem.WaitOne();
                StartConnectionListener();
            }
        }

        private static async void StartConnectionListener()
        {
            HttpListenerContext context = await listener.GetContextAsync();
            sem.Release();
            await RouteRequest(context);
        }

        private static async Task RouteRequest(HttpListenerContext context)
        {
            if (context.Request.Url.AbsolutePath.StartsWith("/demo"))
            {
                await DemoController.HandleRequest(context);
            }
            else
            {
                // Default response for unhandled routes
                string response = "Hello Browser!";
                byte[] encoded = System.Text.Encoding.UTF8.GetBytes(response);
                context.Response.ContentLength64 = encoded.Length;
                context.Response.OutputStream.Write(encoded, 0, encoded.Length);
                context.Response.OutputStream.Close();
            }
        }
    }
}
