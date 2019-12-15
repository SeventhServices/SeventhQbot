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
            = OpenEventType.None;

        public int SubRev { get; set; } = 3;

        public Downloadconfig DownloadConfig { get; set; } 
            = new Downloadconfig();


   
    }
}