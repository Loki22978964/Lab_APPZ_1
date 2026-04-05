using AnimalSM.Core.interfaces;
using System;

namespace AnimalSM.Core.models
{
    public class Snake : Animal, IMovable, ITalkable
    {
        public Snake() : base()
        {
        }

        public Snake(string name) : base(name)
        {
        }

        public void Move()
        {
            if (!CanMove())
            {
                IsHappy = false;
                NotifyStatusChanged($"{Name} is too hungry to move, but can crawl slowly.");
            }
            else
            {
                NotifyStatusChanged($"{Name} crawling in a terrarium.");
            }
        }

        protected override void OnEat()
        {
            NotifyStatusChanged($"{Name} eats and hissing.");
        }

        public override void Sleep()
        {
            NotifyStatusChanged($"{Name} sleeps and hissing.");
        }

        public void Talk()
        {
            NotifyStatusChanged($"{Name} hisses: Ssssss...");
        }
    }
}