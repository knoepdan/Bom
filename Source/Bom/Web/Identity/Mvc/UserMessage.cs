namespace Bom.Web.Identity.Mvc
{
    public class UserMessage
    {

        public enum MessageType
        {
            Info = 0,
            Error = 1
        }

        public UserMessage(string message, MessageType type)
        {
            this.Message = message;
            this.Type = type;

        }

        public MessageType Type { get; set; }

        public string Message { get; set; }
    }
}
