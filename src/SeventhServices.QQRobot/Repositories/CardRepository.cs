using System;
using System.Collections.Generic;
using System.Linq;
using SeventhServices.QQRobot.Abstractions;
using SeventhServices.Resource.Asset.SqlLoader;
using SeventhServices.Resource.Asset.SqlLoader.Classes;

namespace SeventhServices.QQRobot.Repositories
{
    public class CardRepository : IRepository<Card>
    {
        
        public CardRepository(LocalDbLoader localDbLoader)
        {
            _cards = localDbLoader?.TryLoad<Card>().ToList();
        }

        private readonly List<Card> _cards;

        public Card GetById(int id)
        {
            return _cards.Find(c => c.CardId == id);
        }

        public List<Card> GetByCharacterId(int id)
        {
            return _cards.Where(c => c.CharacterId == id).Select(c => c).ToList();
        }

        public Card FuzzyGetByName(string name)
        {
            return _cards.Find(c =>
                c.CardName.Contains(name, StringComparison.Ordinal));
        }
    }
}