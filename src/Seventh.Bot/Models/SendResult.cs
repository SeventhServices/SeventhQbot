using Seventh.Bot.Client.Enums;

namespace Seventh.Bot.Models
{
    public class SendResult
    {
        public bool IsSuccess { get; set; }
        public string Qq { get; set; }
        public string Group { get; set; }
        private MsgType _msgType;
        public MsgType MsgType
        {
            get => _msgType;
            set { 
                _msgType = value;
                MsgTypeName = _msgType.ToString();
            }
        }

        public string MsgTypeName { get; set; }
        public string Message { get; set; }
    }
}