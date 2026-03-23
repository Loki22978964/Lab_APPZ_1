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

            if (_owner.Pet != null)
            {
                SubscribeToPetEvents();
            }
        }

        private void SubscribeToPetEvents()
        {
            if (_owner.Pet != null)
            {
                _owner.Pet.OnFed += HandlePetFed;
                _owner.Pet.OnDied += HandlePetDied;
            }
        }

        private void HandlePetFed()
        {
            CheckOfTheDay();
            CurrentFoodIntake++;

            Console.WriteLine($"[Validator] The animal ate. Portions for today: {CurrentFoodIntake}");

            if (CurrentFoodIntake > MAX_FOOD_INTAKE)
            {
                Console.WriteLine("[Validator] RE-FEEDING!");
                _owner.Pet.Died();
            }
        }

        private void HandlePetDied()
        {
            if (_owner.Pet != null)
            {
                _owner.Pet.OnFed -= HandlePetFed;
                _owner.Pet.OnDied -= HandlePetDied;
            }
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
            if (_owner.Pet == null || !_owner.Pet.IsAlive) return;

            var hours = (DateTime.Now - _owner.Pet.LastFeedingTime).TotalHours;
            if (hours >= 24)
            {
                Console.WriteLine("[Validator] Занадто пізно... Тварина померла від голоду.");
                _owner.Pet.Died();
                return;
            }

            _owner.Feed();
        }
    }

    public class TestProperties
    {
        public string FirstName;
        internal string LastName;
        protected int Age;
        private string PhoneNumber;
    }
}