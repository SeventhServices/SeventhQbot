namespace Sagilio.Bot.Parser.Abstractions
{
    public interface IParsing
    {
        MessageCommand TryParse(ref string message ,string qq);
    }
}