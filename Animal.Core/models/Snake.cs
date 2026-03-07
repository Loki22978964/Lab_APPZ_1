using AnimalSM.Core.interfaces;
using System;

namespace AnimalSM.Core.models
{
    public class Snake : Animal, IMovable
    {
        private const double HUNGER_THRESHOLD_HOURS = 8.0;

        public Snake() : base()
        {
        }

        public Snake(string name) : base(name)
        {
        }

        public void Move()
        {
            double hoursAfterEating = (DateTime.Now - LastFeedingTime).TotalHours;
            if (hoursAfterEating >= HUNGER_THRESHOLD_HOURS)
            {
                IsHappy = false;
                Console.WriteLine($"{Name} is too hungry to move, but can crawl slowly");
            }
            else
            {
                Console.WriteLine($"{Name} crawling in a terrarium");
            }
        }

        protected override void OnEat()
        {
            Console.WriteLine($"{Name} eats and hissing");
        }

        public override void Sleep()
        {
            Console.WriteLine($"{Name} sleeps and hissing.");
        }
    }
}
