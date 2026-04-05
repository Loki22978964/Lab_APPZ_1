using AnimalSM.Core.interfaces;
using AnimalSM.Core.models;

namespace AnimalSM.Core.models
{
    public class Parrot : Animal, IMovable, IFlyable, ITalkable
    {
        public Parrot() : base() { } 
        public Parrot(string name) : base(name) { }

        public void Move()
        {
            if (!CanMove())
            {
                IsHappy = false;
                NotifyStatusChanged($"{Name} is too hungry to walk fast, but can walk slowly.");
            }
            else
            {
                NotifyStatusChanged($"{Name} the parrot walks on his own two legs.");
            }
        }

        public void Fly()
        {
            if (!CanMove())
            {
                IsHappy = false;
                NotifyStatusChanged($"{Name} is too hungry to fly, but can walk slowly.");
            }
            else
            {
                NotifyStatusChanged($"{Name} is flying in the house.");
            }
        }

        protected override void OnEat()
        {
            NotifyStatusChanged($"{Name} eats and chirps.");
        }

        public override void Sleep()
        {
            NotifyStatusChanged($"{Name} sleeps and is NOT chirping.");
        }

        public void Talk()
        {
            NotifyStatusChanged($"{Name} says: Hello! I'm a parrot!");
        }
    }
}