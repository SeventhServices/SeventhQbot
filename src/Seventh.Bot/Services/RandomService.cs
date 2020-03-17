using System;

namespace Seventh.Bot.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class RandomService
    {

        private readonly Random _random = new Random();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="probability"></param>
        /// <returns></returns>
        public bool RandomBool(float probability)
        {
            return _random.Next(0, 10000) < (probability * 10000);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="probability"></param>
        /// <returns></returns>
        public (bool,string) RandomBoolTest(float probability)
        {
            var seed = _random.Next(0, 10000);
            var range = probability * 10000;
            return (seed < range, $"DEBUG : seed:{seed} < range:{range} => {seed < range}");
        }
    }
}