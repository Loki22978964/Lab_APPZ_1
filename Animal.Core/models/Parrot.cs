using AnimalSM.Core.interfaces;

namespace AnimalSM.Core.models
{
    public class Parrot : Animal, IMovable, IFlyable
    {
        public Parrot() : base()
        {
        }

        public Parrot(string name) : base(name)
        {
        }

        public void Move()
        {
            if (!CanMove())
            {
                IsHappy = false;
                Console.WriteLine($"{Name} is too hungry to walk, but can walk slowly");
            }
            else
            {
                Console.WriteLine($"{Name} the parrot walks on his own two legs");
            }
        }

        public void Fly()
        {
            if (!CanMove())
            {
                IsHappy = false;
                Console.WriteLine($"{Name} is too hungry to fly, but can walk slowly");
            }
            else
            {
                Console.WriteLine($"{Name} is flying in the house");
            }
        }

        protected override void OnEat()
        {
            Console.WriteLine($"{Name} eats and chirps");
        }

        public override void Sleep()
        {
            Console.WriteLine($"{Name} sleeps and NOT chirping");
        }
    }
}
