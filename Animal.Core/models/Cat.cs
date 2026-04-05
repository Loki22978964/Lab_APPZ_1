using AnimalSM.Core.interfaces;

namespace AnimalSM.Core.models
{
    public class Cat : Animal, IMovable, ITalkable
    {
        public Cat() : base()
        {
        }

        public Cat(string name) : base(name)
        {
        }

        public void Move()
        {
            if (!CanMove())
            {
                IsHappy = false;
                NotifyStatusChanged($"{Name} is too hungry to run, but can walk slowly.");
            }
            else
            {
                IsHappy = true;
                NotifyStatusChanged($"{Name} runs on his 4 paws in the house.");
            }
        }

        protected override void OnEat()
        {
            IsHappy = true;
            NotifyStatusChanged($"{Name} eats and purrs.");
        }

        public override void Sleep()
        {
            IsTooTired = false;
            IsHappy = true;
            NotifyStatusChanged($"{Name} sleeps and purrs.");
        }

        public void Talk()
        {
            NotifyStatusChanged($"{Name} says: Meow!");
        }
    }
}