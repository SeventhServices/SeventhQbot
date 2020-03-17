using System.Collections.Generic;
using System.Linq;
using Sagilio.Bot.Parser.Abstractions;

namespace Sagilio.Bot.Parser
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

                if (command == null)
                {
                    continue;
                }

                if (command.Break)
                {
                    break;
                }

                if (command.Continue)
                {
                    continue;
                }

                return command;
            }

            return null;
        }

    }
}