using System.Collections.Generic;
using System.Linq;
using SeventhServices.Asset.LocalDB;
using SeventhServices.Asset.LocalDB.Classes;
using SeventhServices.QQRobot.Abstractions;

namespace SeventhServices.QQRobot.Services
{
    public class CharacterRepository : IRepository<Character>
    {
        public CharacterRepository(LocalDbLoader localDbLoader)
        {
            _characters = localDbLoader?.TryLoad<Character>().ToList();
        }

        private readonly List<Character> _characters;

        public Character GetById(int id)
        {
            return _characters.Find(c => c.CharacterId == id);
        }
    }
}