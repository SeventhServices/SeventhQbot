using System.Collections.Generic;
using System.IO;
using SeventhServices.Asset.Common.Classes;

namespace SeventhServices.QQRobot.Services
{
    public class BindDictionary
    {
        private readonly Dictionary<string,Account> _bindAccounts 
            = new Dictionary<string, Account>();

        public bool TryAddAccount(string qq, string pid, string id)
        {
            return _bindAccounts.TryAdd(qq, new Account(pid,id));
        }

        public bool TryGetAccount(string qq, out Account account)
        {
            return _bindAccounts.TryGetValue(qq, out account);
        }

        public bool RemoveAccount(string qq)
        {
            return _bindAccounts.Remove(qq);
        }
    }
}