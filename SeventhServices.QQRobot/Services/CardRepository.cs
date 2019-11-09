using System.Collections.Generic;
using System.Linq;
using SeventhServices.Asset.LocalDB;
using SeventhServices.QQRobot.Abstractions;
using SqlParse.Classes;

namespace SeventhServices.QQRobot.Services
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
    }
}