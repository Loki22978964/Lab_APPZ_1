using System;

namespace AnimalSM.Core.interfaces
{
    public interface IAnimal
    {
        string Name { get; }
        bool IsAlive { get; }
        bool IsHappy { get; }
        DateTime LastFeedingTime { get; }

        event Action? OnFed;
        event Action? OnDied;

        void Eat();
        void Sleep();
        void Died();
    }
}