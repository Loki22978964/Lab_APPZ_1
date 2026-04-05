using AnimalSM.Core.interfaces;
using System;

namespace AnimalSM.Logic.validators
{
    public class AnimalHealthValidator
    {
        public const int MAX_FOOD_INTAKE = 5;
        private int CurrentFoodIntake = 0;

        private readonly IOwner _owner;

        public event Action<string>? NotificationRaised;

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
                _owner.Pet.OnStatusChanged += HandlePetStatusChanged;
            }
        }

        private void HandlePetStatusChanged(string message)
        {
            NotificationRaised?.Invoke($"[Pet] {message}");
        }

        private void HandlePetFed()
        {
            CurrentFoodIntake++;

            NotificationRaised?.Invoke($"[Validator] The animal ate. Portions today: {CurrentFoodIntake}/{MAX_FOOD_INTAKE}");
        }

        private void HandlePetDied()
        {
            if (_owner.Pet != null)
            {
                _owner.Pet.OnFed -= HandlePetFed;
                _owner.Pet.OnDied -= HandlePetDied;
                _owner.Pet.OnStatusChanged -= HandlePetStatusChanged;
            }

            NotificationRaised?.Invoke($"[Validator] Pet of owner '{_owner.Name}' died.");
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
                NotificationRaised?.Invoke($"The {_owner.Pet.GetType().Name} named {_owner.Pet.Name} is alive.");
            }
        }

        public void PetFeeding()
        {
            if (_owner.Pet == null || !_owner.Pet.IsAlive)
            {
                NotificationRaised?.Invoke("[Validator] Feeding aborted: pet missing or dead.");
                return;
            }

            CheckOfTheDay();

            if (CurrentFoodIntake >= MAX_FOOD_INTAKE)
            {
                NotificationRaised?.Invoke($"[Validator] {_owner.Pet.Name} is completely full and refuses to eat anymore today!");
                return;
            }

            var hours = (DateTime.Now - _owner.Pet.LastFeedingTime).TotalHours;
            if (hours >= 24)
            {
                NotificationRaised?.Invoke("[Validator] Too late... The animal died of hunger.");
                _owner.Pet.Died();
                return;
            }

            _owner.Feed();
        }
    }
}