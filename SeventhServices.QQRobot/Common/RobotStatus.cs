using System;
using SeventhServices.Asset.Common.Abstractions;
using SeventhServices.Client.Common.Enums;
using SeventhServices.Client.Network.Models.Response.Shared;

namespace SeventhServices.QQRobot.Common
{
    public class RobotStatus : ConfigureFile
    {
        public DateTime LastEventBorderDateTime { get; set; }
        public OpenEventType OpenEventType { get; set; } 
            = OpenEventType.Nnlive;

        public Downloadconfig Downloadconfig { get; set; } 
            = new Downloadconfig();
    }
}