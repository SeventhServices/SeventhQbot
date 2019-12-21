using SeventhServices.Resource.Common.Classes;

namespace SeventhServices.QQRobot.Resource.Entities
{
    public class BoundAccount : Account
    {
        public BoundAccount()
        {
            
        }

        public BoundAccount(string qq, string pid, string uuid, bool isSaveEncrypt = true) :
            base(pid, uuid, isSaveEncrypt) 
        {
            Qq = qq;
        }

        public BoundAccount(string qq, string tpid, string encUuid, string ivs) 
            : base(tpid, encUuid, ivs )
        {
            Qq = qq;
        }

        public string Qq { get; set; }
        public AccountBinding AccountBinding { get; set; }
    }
}