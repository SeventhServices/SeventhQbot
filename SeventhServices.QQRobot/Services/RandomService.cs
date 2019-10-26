using System;

namespace SeventhServices.QQRobot.Services
{
    public class RandomService
    {
        private readonly Random _random = new Random();

        public bool RandomBool(float probability)
        {
            return _random.Next(0, 10000) < (probability * 10000);
        }
    }
}