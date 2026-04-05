using System;

namespace AnimalSM.Core.models
{
    public abstract class Animal
    {
        public string Name { get; protected set; }
        public bool IsAlive { get; protected set; }
        public bool IsHappy { get; protected set; }
        public DateTime LastFeedingTime { get; protected set; }
        protected bool IsTooTired { get; set; }

        public event Action? OnFed;
        public event Action? OnDied;
        public event Action<string>? OnStatusChanged;

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

        protected void NotifyStatusChanged(string message)
        {
            OnStatusChanged?.Invoke(message);
        }

        public virtual void Eat()
        {
            if (!IsAlive)
            {
                NotifyStatusChanged($"{Name} is dead and cannot eat.");
                return;
            }

            this.LastFeedingTime = DateTime.Now;
            this.IsHappy = true;
            this.IsTooTired = false;

            OnFed?.Invoke();
            NotifyStatusChanged($"{Name} has been fed.");
            OnEat();
        }

        protected abstract void OnEat();

        public virtual void Sleep()
        {
            IsTooTired = false;
            IsHappy = true;
            NotifyStatusChanged($"{Name} is sleeping.");
        }

        public virtual void Died()
        {
            IsAlive = false;
            NotifyStatusChanged($"{this.Name} the {GetType().Name.ToLower()} died.");
            OnDied?.Invoke();
        }

        public virtual void ReceiveCleaning()
        {
            IsHappy = true;
            NotifyStatusChanged($"{Name}'s living area is now clean.");
        }

        protected virtual bool CanMove()
        {
            if ((DateTime.Now - this.LastFeedingTime).TotalHours >= 8)
            {
                SetTooHungry();
                return false;
            }
            return !IsTooTired;
        }

        protected void SetTooHungry()
        {
            IsTooTired = true;
            IsHappy = false;
            NotifyStatusChanged($"{Name} is too hungry.");
        }
    }
}