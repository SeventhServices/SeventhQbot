using System.Collections.Generic;

namespace SeventhServices.QQRobot.Parser.Abstractions
{
    public abstract class MessageCommand
    {
        public List<string> ReturnMessage { get; set; } = new List<string>();

        public bool Continue { get; set; }

        public bool Break { get; set; }
    }
}