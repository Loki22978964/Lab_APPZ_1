using System;

namespace AnimalSM.Core.interfaces
{
    public interface IAnimal
    {
        string Name { get; }
        bool IsAlive { get; }
        bool IsHappy { get; }
        DateTime LastFeedingTime { get; }

        void Eat();
        void Sleep();
        void Died();
    }
}