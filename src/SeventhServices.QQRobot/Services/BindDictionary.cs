using System.Collections.Generic;
using SeventhServices.Resource.Common.Classes;

namespace SeventhServices.QQRobot.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class BindDictionary
    {
        private readonly Dictionary<string,Account> _bindAccounts 
            = new Dictionary<string, Account>();

        public bool TryAddAccount(string qq, string pid, string id)
        {
            return _bindAccounts.TryAdd(qq, new Account(pid,id));
        }

        public bool TryAddAccount(string qq, string pid, string id,string ivs)
        {
            return _bindAccounts.TryAdd(qq, new Account(pid, id, ivs));
        }

        /// <summary>
        /// Get account
        /// </summary>
        /// <param name="qq"></param>
        /// <param name="account"></param>
        /// <returns></returns>
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