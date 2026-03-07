using AnimalSM.Core.interfaces;

namespace AnimalSM.Core.models
{
    public class Cat : Animal, IMovable
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
                Console.WriteLine($"{Name} is too hungry to run, but can walk slowly.");
            }
            else
            {
                Console.WriteLine($"{Name} runs on his 4 paws in the house.");
            }
        }

        protected override void OnEat()
        {
            Console.WriteLine($"{Name} eats and purrs.");
        }

        public override void Sleep()
        {
            Console.WriteLine($"{Name} sleeps and purrs.");
        }
    }
}
