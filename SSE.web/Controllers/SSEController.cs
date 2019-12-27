using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SSE.web.Services;

namespace SSE.web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SSEController : ControllerBase
    {
       
        public string GetRootWeb(Microsoft.AspNetCore.Http.HttpRequest request)
        {
            return $"{request.Scheme}://" + request.Host.ToString().Trim('/');
        }

        [HttpGet("Listener")]
        public async Task Listener(string c, string token)
        {
            await ServerSendEventHub.RegisterListenerClientContext(c, HttpContext);
        }

        [HttpPost("SendMessage")]
        public async Task SendMessage([FromBody] ServerSendEventMessage postData)
        {
            var c = postData.c;
            var token = postData.token;
            var msg = postData.msg;

            var uiChannel = $"{c}";
            await ServerSendEventHub.Publish(uiChannel, msg);
        }
    }
}