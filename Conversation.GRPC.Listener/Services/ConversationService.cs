using Conversation.shared;
using Dapr.AppCallback.Autogen.Grpc.v1;
using Dapr.Client;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
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
                Topic = "send-mail"
            });
            result.Subscriptions.Add(new TopicSubscription
            {
                PubsubName = "conversation-pubsub",
                Topic = "send-sms"
            });
            return Task.FromResult(result);
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
                var input = JsonConvert.DeserializeObject<Message>(request.Data.ToStringUtf8());
                                
                if (request.Topic == "email")
                {
                    Email(input);
                }
                else if(request.Topic == "sms")
                {
                    Sms(input);
                }
            }

            return new TopicEventResponse();
        }

        private void Email(Message input)
        {
            this._logger.LogInformation("Email message received: ", JsonConvert.SerializeObject(input));
        }

        private void Sms(Message input)
        {
            this._logger.LogInformation("SMS message received: ", JsonConvert.SerializeObject(input));
        }
    }
}
