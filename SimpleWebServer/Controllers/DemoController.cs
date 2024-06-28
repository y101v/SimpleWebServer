using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SimpleWebServer.Controllers
{
    public class DemoController
    {
        public static async Task HandleRequest(HttpListenerContext context)
        {
            if (context.Request.HttpMethod == "PUT" && context.Request.Url.AbsolutePath == "/demo/ajax")
            {
                using (var reader = new StreamReader(context.Request.InputStream, context.Request.ContentEncoding))
                {
                    var data = await reader.ReadToEndAsync();
                    // Process the data (e.g., parse JSON, validate CSRF token, etc.)
                    var response = "Received data: " + data;
                    byte[] buffer = Encoding.UTF8.GetBytes(response);
                    context.Response.ContentLength64 = buffer.Length;
                    await context.Response.OutputStream.WriteAsync(buffer, 0, buffer.Length);
                }
            }
            else
            {
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                byte[] buffer = Encoding.UTF8.GetBytes("Not Found");
                context.Response.ContentLength64 = buffer.Length;
                await context.Response.OutputStream.WriteAsync(buffer, 0, buffer.Length);
            }
            context.Response.OutputStream.Close();
        }
    }
}

