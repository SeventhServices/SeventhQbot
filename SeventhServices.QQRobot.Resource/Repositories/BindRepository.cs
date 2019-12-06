using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SeventhServices.QQRobot.Resource.Entities;

namespace SeventhServices.QQRobot.Resource.Repositories
{
    public class BindRepository
    {
        private readonly QBotDbContext _qBotDbContext;

        public BindRepository(QBotDbContext qBotDbContext)
        {
            _qBotDbContext = qBotDbContext;
        }

        public AccountBinding GetOneByQq(string qq)
        {
            return _qBotDbContext.AccountBindings
                .Include(b => b.BoundAccount)
                .FirstOrDefault(b => b.Qq == qq);
        }

        public IEnumerable<AccountBinding> GetByQq(string qq)
        {
            return _qBotDbContext.AccountBindings
                .Include(b => b.BoundAccount)
                .Where(b => b.Qq == qq).ToList();
        }

        public AccountBinding Add(AccountBinding bind)
        {
            _qBotDbContext.AccountBindings.Add(bind);
            _qBotDbContext.SaveChanges();

            return bind;
        }

        public void RemoveAll(string qq)
        {
            var accountBindings = GetByQq(qq);
            
            _qBotDbContext.AccountBindings.RemoveRange(accountBindings);
            _qBotDbContext.SaveChanges();
        }
    }
}