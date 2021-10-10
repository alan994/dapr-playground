using Conversation.shared;
using Dapr.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Conversation.Api2
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDaprClient();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async (HttpContext context) =>
                {
                    var dapr = context.RequestServices.GetRequiredService<DaprClient>();
                    await dapr.PublishEventAsync("conversation-pubsub", "conversations", new User{ FirstName = "Alan", LastName = "Jagar" });
                    await context.Response.WriteAsync("Poruka poslana");
                });

                endpoints.MapGet("/email", async (HttpContext context) =>
                {
                    var message = new Message()
                    {
                        Body = "Ovo je sadržaj poruke. " + DateTime.Now.ToString(),
                        Receiver = "Dominik Borović",
                        Sender = "Alan Jagar",
                        Type = MessageType.Email
                    };

                    var dapr = context.RequestServices.GetRequiredService<DaprClient>();
                    await dapr.PublishEventAsync("conversation-pubsub", "mail", message);
                    await context.Response.WriteAsync("Email sent");
                });

                endpoints.MapGet("/sms", async (HttpContext context) =>
                {
                    var message = new Message()
                    {
                        Body = "Ovo je sadržaj poruke. " + DateTime.Now.ToString(),
                        Receiver = "Leopold Mandić",
                        Sender = "Alan Jagar",
                        Type = MessageType.SMS
                    };

                    var dapr = context.RequestServices.GetRequiredService<DaprClient>();
                    await dapr.PublishEventAsync("conversation-pubsub", "sms", message);
                    await context.Response.WriteAsync("SMS Sent");
                });
            });
        }
    }
}


public class User
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
}
