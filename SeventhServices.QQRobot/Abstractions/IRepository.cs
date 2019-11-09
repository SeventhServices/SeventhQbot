namespace SeventhServices.QQRobot.Abstractions
{
    public interface IRepository<T>
    {
        T GetById(int id);
    }
}