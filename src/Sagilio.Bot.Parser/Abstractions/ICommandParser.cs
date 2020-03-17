namespace Sagilio.Bot.Parser.Abstractions
{
    public interface ICommandParser
    {
        void Add<T>(T parsing) where T : IParsing;

        void Clear();

        bool Any();

        MessageCommand Parse(string message, string qq);
    }
}