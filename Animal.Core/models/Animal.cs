using AnimalSM.Core.interfaces;
using System;

namespace AnimalSM.Core.models
{
    public abstract class Animal : IAnimal
    {
        public string Name { get; protected set; }
        public bool IsAlive { get; protected set; }
        public bool IsHappy { get; protected set; }
        public DateTime LastFeedingTime { get; protected set; }
        protected bool IsTooTired { get; set; }

        protected Animal()
        {
            this.Name = $"nameless{GetType().Name}_" + Guid.NewGuid();
        }

        protected Animal(string name)
        {
            this.Name = name;
            this.IsAlive = true;
            this.IsHappy = true;
            this.LastFeedingTime = DateTime.Now;
            this.IsTooTired = false;
        }

        public virtual void Eat()
        {
            if (!IsAlive)
            {
                Console.WriteLine("the dead don't eat");
                return;
            }

            this.LastFeedingTime = DateTime.Now;
            this.IsHappy = true;
            this.IsTooTired = false;
            OnEat();
        }

        protected abstract void OnEat();

        public virtual void Sleep()
        {
            Console.WriteLine($"{Name} is sleeping");
        }

        public virtual void Died()
        {
            IsAlive = false;
            Console.WriteLine($"{this.Name} the {GetType().Name.ToLower()} died(((");
        }

        protected virtual bool CanMove()
        {
            if ((int)(DateTime.Now - this.LastFeedingTime).TotalHours >= 5 )
            {
                SetTooHungry();
            }
            return !IsTooTired;
        }

        protected void SetTooHungry()
        {
            IsTooTired = true;
            IsHappy = false;
        }
    }
}
