using AnimalSM.Core.interfaces;
using System;

namespace AnimalSM.Logic.validators
{
    public class AnimalHealthValidator
    {
        public const int MAX_FOOD_INTAKE = 5;
        private int CurrentFoodIntake = 0;

        private readonly IOwner _owner;

        public AnimalHealthValidator(IOwner owner)
        {
            _owner = owner ?? throw new ArgumentNullException(nameof(owner));
        }

        public void CheckOfTheDay()
        {
            if (_owner.Pet == null) return;

            if (DateTime.Now.Date > _owner.Pet.LastFeedingTime.Date)
            {
                CurrentFoodIntake = 0;
            }
        }

        public void IsTheAnimalAlive()
        {
            if (_owner.Pet == null) return;

            if (_owner.Pet.IsAlive)
            {
                Console.WriteLine($"The {_owner.Pet.GetType().Name} named {_owner.Pet.Name} is alive.");
            }
        }

        public void PetFeeding()
        {
            if (_owner.Pet == null) return;

            CheckOfTheDay();

            var hours = (int)Math.Floor((DateTime.Now - _owner.Pet.LastFeedingTime).TotalHours);

            if (hours >= 24)
            {
                _owner.Pet.Died();
                return;
            }

            if (CurrentFoodIntake >= MAX_FOOD_INTAKE)
            {
                _owner.Pet.Died();
                return;
            }

            _owner.Feed();
            CurrentFoodIntake++;
        }
    }
}