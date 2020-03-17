using System;

namespace Seventh.Bot.Resource.Entities
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