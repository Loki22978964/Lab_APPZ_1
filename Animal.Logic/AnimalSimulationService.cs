using AnimalSM.Core.interfaces;
using AnimalSM.Logic.validators;
using AnimalSM.Core.models;
using System.Linq;
using System.Collections.Generic;
using System;

namespace AnimalSM.Logic
{
    public class AnimalSimulationService
    {
        private readonly List<IOwner> _owners = new();
        private readonly List<Animal> _wildAnimals = new();

        private readonly IAnimalFactory _animalFactory;
        private readonly IOwnerFactory _ownerFactory;
        private readonly Dictionary<Guid, AnimalHealthValidator> _validators = new();

        public event Action<string>? OnServiceMessage;

        public AnimalSimulationService(
            IAnimalFactory animalFactory,
            IOwnerFactory ownerFactory)
        {
            _animalFactory = animalFactory ?? throw new ArgumentNullException(nameof(animalFactory));
            _ownerFactory = ownerFactory ?? throw new ArgumentNullException(nameof(ownerFactory));
        }

        public IReadOnlyList<IOwner> Owners => _owners.AsReadOnly();
        public IReadOnlyList<Animal> WildAnimal => _wildAnimals.AsReadOnly();
        public IReadOnlyList<int> AvailableAnimalTypeIds => _animalFactory.GetAvailableAnimalTypeIds().ToList();

        private void SetupValidatorForOwner(IOwner owner)
        {
            if (!_validators.ContainsKey(owner.Id))
            {
                var validator = new AnimalHealthValidator(owner);
                
                validator.NotificationRaised += message => OnServiceMessage?.Invoke(message);
                _validators[owner.Id] = validator;
            }
        }

        public void AddOwnerWithAnimal(string ownerName, int animalTypeId, string petName)
        {
            if (string.IsNullOrWhiteSpace(ownerName))
            {
                OnServiceMessage?.Invoke("Error: Owner name cannot be empty!");
                return;
            }

            var animal = _animalFactory.CreateAnimal(animalTypeId, petName);
            if (animal == null)
            {
                OnServiceMessage?.Invoke("Error: Failed to create animal!");
                return;
            }

            var owner = _ownerFactory.Create(ownerName, animal);
            _owners.Add(owner);
            SetupValidatorForOwner(owner);

            OnServiceMessage?.Invoke($"\n✓ Owner '{ownerName}' with animal '{animal.Name}' successfully added!");
        }

        public void AddAnimalToExistingOwner(int ownerIndex, int animalTypeId, string petName)
        {
            if (ownerIndex < 1 || ownerIndex > _owners.Count)
            {
                OnServiceMessage?.Invoke("Error: Invalid owner number!");
                return;
            }

            var selectedOwner = _owners[ownerIndex - 1];
            var animal = _animalFactory.CreateAnimal(animalTypeId, petName);
            if (animal == null) return;

            selectedOwner.AdoptPet(animal);
            SetupValidatorForOwner(selectedOwner);

            OnServiceMessage?.Invoke($"\n✓ Animal '{animal.Name}' successfully added to owner '{selectedOwner.Name}'!");
        }

        public void AddAnimalOnly(int animalTypeId, string petName)
        {
            var animal = _animalFactory.CreateAnimal(animalTypeId, petName);
            if (animal == null) return;

            _wildAnimals.Add(animal);

            OnServiceMessage?.Invoke($"\n✓ Animal '{animal.Name}' added!");
        }

        public IOwner? SelectOwnerWithPet(int index)
        {
            var ownersWithPets = _owners.Where(o => o.Pet != null).ToList();
            if (index < 1 || index > ownersWithPets.Count) return null;
            return ownersWithPets[index - 1];
        }

        public void FeedPet(IOwner owner)
        {
            if (owner == null) return;
            SetupValidatorForOwner(owner);
            _validators[owner.Id].PetFeeding();
        }

        public void CleanPet(IOwner owner)
        {
            if (owner.Pet == null) return;
            
            owner.Pet.ReceiveCleaning();
            OnServiceMessage?.Invoke($"[Service] You cleaned up after {owner.Pet.Name}. It is now happy!");
        }

        public void ExecuteAction(IOwner owner, int actionId)
        {
            if (owner.Pet == null) return;

            switch (actionId)
            {
                case 1:
                    if (owner.Pet is IMovable movable) movable.Move();
                    else OnServiceMessage?.Invoke($"{owner.Pet.Name} cannot move in this way.");
                    break;
                case 2:
                    if (owner.Pet is IFlyable flyable) flyable.Fly();
                    else OnServiceMessage?.Invoke($"{owner.Pet.Name} cannot fly.");
                    break;
                case 3:
                    owner.Pet.Sleep();
                    break;
                case 4:
                    if (owner.Pet is ITalkable talkable) talkable.Talk();
                    else OnServiceMessage?.Invoke($"{owner.Pet.Name} cannot talk.");
                    break;
                default:
                    OnServiceMessage?.Invoke("Invalid action choice!");
                    break;
            }
        }

        public void UpdateStatus()
        {
            foreach (var validator in _validators.Values)
            {
                validator.CheckOfTheDay();
                validator.IsTheAnimalAlive();
            }
        }

        public IReadOnlyList<IOwner> GetOwnersWithPets() => _owners.Where(o => o.Pet != null).ToList();
    }
}