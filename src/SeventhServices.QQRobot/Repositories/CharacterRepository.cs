using System;
using System.Collections.Generic;
using System.Linq;
using SeventhServices.QQRobot.Abstractions;
using SeventhServices.Resource.Asset.SqlLoader;
using SeventhServices.Resource.Asset.SqlLoader.Classes;

namespace SeventhServices.QQRobot.Repositories
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

        public Character FuzzyGetByName(string name)
        {
            return _characters.FirstOrDefault(c => 
                c.CharacterName.Contains(name,StringComparison.OrdinalIgnoreCase) 
                || c.EnglishName.Contains(name,StringComparison.OrdinalIgnoreCase));
        }

        public List<Character> GetByCharacterId(int id)
        {
            throw new NotImplementedException();
        }
    }
}