using System.Collections.Generic;

namespace Seventh.Bot.Abstractions
{
    public interface IRepository<T>
    {
        T GetById(int id);

        T FuzzyGetByName(string name);

        List<T> GetByCharacterId(int id);
    }
}