using System;
using Sagilio.Bot.Parser.Abstractions;
using Sagilio.Bot.Parser.Commands;

namespace Sagilio.Bot.Parser
{
    public class StringParsing : IParsing
    {
        private readonly Action<MessageCommand, string, string> _actionWithMessageAndQq;
        private readonly Action<MessageCommand, string> _actionWithMessage;
        private readonly Action<MessageCommand> _action;
        private readonly string _defaultReturnString = "www";
        private string _startString = string.Empty;
        private bool _breakOnFailed;

        public StringParsing(Action<MessageCommand> action)
        {
            _action = action;
        }

        public StringParsing(Action<MessageCommand> action, string defaultReturnString)
            : this(action)
        {
            _defaultReturnString = defaultReturnString;
        }

        public StringParsing(Action<MessageCommand,string> action)
        {
            _actionWithMessage = action;
        }

        public StringParsing(Action<MessageCommand,string> action, string defaultReturnString) 
            : this(action)
        {
            _defaultReturnString = defaultReturnString;
        }

        public StringParsing(Action<MessageCommand, string,string> action)
        {
            _actionWithMessageAndQq = action;
        }

        public StringParsing(Action<MessageCommand, string,string> action, string defaultReturnString)
            : this(action)
        {
            _defaultReturnString = defaultReturnString;
        }

        public StringParsing WhenStartWith(string startString)
        {
            _startString = startString;
            return this;
        }

        public StringParsing BreakOnFailed(bool breakOnFailed)
        {
            _breakOnFailed = breakOnFailed;
            return this;
        }

        public MessageCommand TryParse(ref string message, string qq)
        {
            var command = new TextReturnCommand();

            if (string.IsNullOrEmpty(_startString))
            {
                SelectAction(command,message, qq);
                return command;
            }
            if (message.StartsWith(_startString))
            {
                message = message.TrimStart(_startString.ToCharArray());
                SelectAction(command, message, qq);
                return command;
            }

            if (_breakOnFailed)
            {
                command.Break = true;
                return command;
            }

            return null;
        }

        private void SelectAction(MessageCommand command, string message, string qq)
        {
            _action?.Invoke(command);
            _actionWithMessage?.Invoke(command, message);
            _actionWithMessageAndQq?.Invoke(command,message,qq);
        }
    }
}