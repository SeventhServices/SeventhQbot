using System.Threading.Tasks;

namespace SeventhServices.QQRobot.Parser.Abstractions
{
    public interface IParsing
    {
        MessageCommand TryParse(ref string message ,string qq);
    }
}