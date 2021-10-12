using Conversation.shared;
using Dapr.AppCallback.Autogen.Grpc.v1;
using Dapr.Client;
using Dapr.Client.Autogen.Grpc.v1;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using static Dapr.AppCallback.Autogen.Grpc.v1.AppCallback;

namespace Conversation.GRPC.Listener.Services
{
    public class ConversationService : AppCallbackBase
    {        
        private readonly ILogger<ConversationService> _logger;
        private readonly DaprClient _daprClient;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="daprClient"></param>
        /// <param name="logger"></param>
        public ConversationService(DaprClient daprClient, ILogger<ConversationService> logger)
        {
            _daprClient = daprClient;
            _logger = logger;
        }

        public override Task<ListTopicSubscriptionsResponse> ListTopicSubscriptions(Empty request, ServerCallContext context)
        {
            var result = new ListTopicSubscriptionsResponse();
            result.Subscriptions.Add(new TopicSubscription
            {
                PubsubName = "conversation-pubsub",
                Topic = "mail"
            });
            result.Subscriptions.Add(new TopicSubscription
            {
                PubsubName = "conversation-pubsub",
                Topic = "sms"
            });
            return Task.FromResult(result);
        }

        public override async Task<InvokeResponse> OnInvoke(InvokeRequest request, ServerCallContext context)
        {
            var response = new InvokeResponse();
            switch (request.Method)
            {
                case "mail":     
                    var message = request.Data.Unpack<Conversation.GRPC.Generated.Message>();
                    await Email(message);
                    response.Data = Any.Pack(new Conversation.GRPC.Generated.Empty());
                    break;
                case "sms":
                    var input = request.Data.Unpack<Conversation.GRPC.Generated.Message>();
                    await Email(input);
                    response.Data = Any.Pack(new Conversation.GRPC.Generated.Empty());
                    break;
            }
            return response;
        }

        /// <summary>
        /// implement OnTopicEvent to handle deposit and withdraw event
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        /// 
        public override async Task<TopicEventResponse> OnTopicEvent(TopicEventRequest request, ServerCallContext context)
        {
            if (request.PubsubName == "conversation-pubsub")
            {
                var input = JsonConvert.DeserializeObject<Generated.Message>(request.Data.ToStringUtf8());
                                
                if (request.Topic == "mail")
                {
                    await Email(input);
                }
                else if(request.Topic == "sms")
                {
                    await Sms(input);
                }
            }

            return new TopicEventResponse();
        }

        private async Task Email(Generated.Message input)
        {
            this._logger.LogInformation("Email message received: {0}", JsonConvert.SerializeObject(input));

            var httpClient = DaprClient.CreateInvokeHttpClient();
            var response = await httpClient.GetAsync("http://conversation-api1/home");
            this._logger.LogInformation("Result from service invocation {0}", JsonConvert.SerializeObject(await response.Content.ReadAsStringAsync()));
        }

        private async Task Sms(Generated.Message input)
        {
            this._logger.LogInformation("SMS message received: ", JsonConvert.SerializeObject(input));
            //var response = await this._daprClient.InvokeMethodGrpcAsync<Service.HelloRequest, Service.HelloReply>("conversation-grpc-service", "SayHello", new Service.HelloRequest() { Name = "Alan SMS" });
            //this._logger.LogInformation("SMS message received by GRPC service invocation: ", JsonConvert.SerializeObject(response));
        }
    }
}
