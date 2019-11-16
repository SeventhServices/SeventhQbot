using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using SeventhServices.QQRobot.Parser.Abstractions;
using System.Threading.Tasks;

namespace SeventhServices.QQRobot.Parser
{
    public class CommandParser : ICommandParser
    {
        private readonly ICollection<IParsing> _parsings = new List<IParsing>();

        public void Add<T>(T parsing) where T : IParsing
        {
            _parsings.Add(parsing);
        }

        public void Clear()
        {
            _parsings.Clear();
        }

        public bool Any()
        {
            return _parsings.Any();
        }

        public MessageCommand Parse(string message, string qq)
        {
            foreach (var parsing in _parsings)
            {
                var command = parsing.TryParse(ref message, qq);
                if ( command != null && command.CanReturn)
                {
                    return command;
                }
            }

            return null;
        }

    }
}