using System;
using SeventhServices.Asset.Common.Classes;
using SeventhServices.QQRobot.Resource.Interfaces;

namespace SeventhServices.QQRobot.Resource.Entities
{

    public class AccountBinding
    {
        public string Qq { get; set; }
        public DateTime BindTime { get; set; }
        public bool IsPidOnly { get; set; }
        public string BoundAccountPid { get; set; }
        public BoundAccount BoundAccount { get; set; }
    }
}