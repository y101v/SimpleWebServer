using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

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
            // Handle the request
            string response = "Hello Browser!";
            byte[] encoded = System.Text.Encoding.UTF8.GetBytes(response);
            context.Response.ContentLength64 = encoded.Length;
            context.Response.OutputStream.Write(encoded, 0, encoded.Length);
            context.Response.OutputStream.Close();
        }
    }
}
