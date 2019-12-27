using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SSE.web.Services
{
    public static class ServerSendEventHub
    {
        static ConcurrentDictionary<string, List<HttpContext>> _hubs = new ConcurrentDictionary<string, List<HttpContext>>();

        public static Dictionary<string, long> ListChannel()
        {
            Dictionary<string, long> channels = new Dictionary<string, long>();
            lock (_hubs)
            {
                foreach (var h in _hubs)
                {
                    channels.Add(h.Key, h.Value.Count);
                }
            }
            //{0cacef28-4e03-4ef2-a728-1b225eb20b27}
            return channels;
        }

        public static async Task RegisterListenerClientContext(string channel, HttpContext context)
        {
            var response = context.Response;

            response.Headers["Cache-Control"] = "no-cache";
            response.Headers["X-Accel-Buffering"] = "no";

            response.ContentType = "text/event-stream";

            var members = _hubs.GetOrAdd(channel, _ => new List<HttpContext>());
            lock (members)
            {
                members.Add(context);
            }

            try
            {
                await Task.Delay(Timeout.Infinite, context.RequestAborted);
            }
            catch (TaskCanceledException)
            {
            }

            lock (members)
            {
                members.Remove(context);
            }
        }

        static async Task Publish(string channel, string message)
        {
            if (!_hubs.TryGetValue(channel, out var members))
            { return; }

            members = SafeCopy(members);

            await Task.WhenAll(members.Select(Send));

            async Task Send(HttpContext context)
            {
                var member = new StreamWriter(context.Response.Body);

                await member.WriteAsync($"data: {message}\n\n");
                await member.FlushAsync();
                member.Close();
            }

            List<HttpContext> SafeCopy(List<HttpContext> context)
            {
                lock (context)
                {
                    return context.ToList();
                }
            }
        }

        public static async Task Publish<T>(string channel, T data)
        {
            var message = System.Text.Json.JsonSerializer.Serialize(data);

            await Publish(channel, message);
        }
    }


}

