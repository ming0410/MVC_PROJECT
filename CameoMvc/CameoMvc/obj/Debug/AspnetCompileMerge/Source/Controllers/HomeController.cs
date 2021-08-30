using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.WebSockets;

namespace CameoMvc.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public HttpResponseMessage Get()
        {
            if (HttpContext.IsWebSocketRequest)
            {
                HttpContext.AcceptWebSocketRequest(ProcessWSChat);
            }
            return new HttpResponseMessage(HttpStatusCode.SwitchingProtocols);
        }
        private async Task ProcessWSChat(AspNetWebSocketContext arg)
        {
            WebSocket socket = arg.WebSocket;
            while (true)
            {
                ArraySegment<byte> buffer = new ArraySegment<byte>(new byte[1024]);
                //接收數據到緩衝區。此操作將懸停等待，直到接收到數據或連接超時。
                WebSocketReceiveResult result = await socket.ReceiveAsync(buffer, CancellationToken.None);
                if (socket.State == WebSocketState.Open)
                {
                    string message = Encoding.UTF8.GetString(buffer.Array, 0, result.Count);
                    string returnMessage = "You send :" + message + ". at" + DateTime.Now.ToLongTimeString();
                    buffer = new ArraySegment<byte>(Encoding.UTF8.GetBytes(returnMessage));
                    //發送數據到客戶端
                    await socket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
                }
                else
                {
                    break;
                }
            }
        }
    }
}