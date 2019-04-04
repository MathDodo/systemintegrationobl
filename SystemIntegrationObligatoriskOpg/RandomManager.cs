using Data;
using System;

namespace SystemIntegrationObligatoriskOpg
{
    public sealed class RandomManager : SingletonBase<RandomManager>
    {
        public Random _Randy;

        private RandomManager()
        {
            _Randy = new Random();
        }
    }
}