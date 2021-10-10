using System;

namespace Conversation.shared
{
    public class Message
    {
        public MessageType Type { get; set; }
        public string Sender { get; set; }
        public string Receiver { get; set; }
        public string Body { get; set; }
    }

    public enum MessageType
    {
        Email = 1, SMS = 2
    }
}
