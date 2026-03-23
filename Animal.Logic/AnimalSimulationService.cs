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
        private readonly IAnimalFactory _animalFactory;
        private readonly IOwnerFactory _ownerFactory;
        private readonly Dictionary<Guid, AnimalHealthValidator> _validators = new();

        public AnimalSimulationService(
            IAnimalFactory animalFactory,
            IOwnerFactory ownerFactory)
        {
            _animalFactory = animalFactory ?? throw new ArgumentNullException(nameof(animalFactory));
            _ownerFactory = ownerFactory ?? throw new ArgumentNullException(nameof(ownerFactory));
        }

        public IReadOnlyList<IOwner> Owners => _owners.AsReadOnly();

        public IReadOnlyList<int> AvailableAnimalTypeIds => _animalFactory.GetAvailableAnimalTypeIds().ToList();

        public void AddOwnerWithAnimal(string ownerName, int animalTypeId, string petName)
        {
            if (string.IsNullOrWhiteSpace(ownerName))
            {
                Console.WriteLine("Owner name cannot be empty!");
                return;
            }

            var animal = _animalFactory.CreateAnimal(animalTypeId, petName);
            if (animal == null)
            {
                Console.WriteLine("Failed to create animal!");
                return;
            }

            var owner = _ownerFactory.Create(ownerName, animal);
            _owners.Add(owner);

            _validators[owner.Id] = new AnimalHealthValidator(owner);

            Console.WriteLine($"\n✓ Owner '{ownerName}' with animal '{animal.Name}' successfully added!");
        }

        public void AddAnimalToExistingOwner(int ownerIndex, int animalTypeId, string petName)
        {
            if (ownerIndex < 1 || ownerIndex > _owners.Count)
            {
                Console.WriteLine("Invalid owner number!");
                return;
            }

            var selectedOwner = _owners[ownerIndex - 1];
            var animal = _animalFactory.CreateAnimal(animalTypeId, petName);
            if (animal == null)
            {
                Console.WriteLine("Failed to create animal!");
                return;
            }

            selectedOwner.AdoptPet(animal);

            if (!_validators.ContainsKey(selectedOwner.Id))
            {
                _validators[selectedOwner.Id] = new AnimalHealthValidator(selectedOwner);
            }

            Console.WriteLine($"\n✓ Animal '{animal.Name}' successfully added to owner '{selectedOwner.Name}'!");
        }

        public void AddAnimalOnly(int animalTypeId, string petName)
        {
            var animal = _animalFactory.CreateAnimal(animalTypeId, petName);
            if (animal == null)
            {
                Console.WriteLine("Failed to create animal!");
                return;
            }

            var owner = _ownerFactory.Create();
            owner.AdoptPet(animal);
            _owners.Add(owner);

            _validators[owner.Id] = new AnimalHealthValidator(owner);

            Console.WriteLine($"\n✓ Animal '{animal.Name}' added! Owner: '{owner.Name}' (automatically created)");
        }

        public IOwner? SelectOwnerWithPet(int index)
        {
            var ownersWithPets = _owners.Where(o => o.Pet != null).ToList();
            if (index < 1 || index > ownersWithPets.Count)
                return null;
            return ownersWithPets[index - 1];
        }

        public void FeedPet(IOwner owner)
        {
            if (owner == null) return;

            if (!_validators.TryGetValue(owner.Id, out var validator))
            {
                validator = new AnimalHealthValidator(owner);
                _validators[owner.Id] = validator;
            }

            validator.PetFeeding();
        }

        public void CleanPet(IOwner owner)
        {
            if (owner.Pet == null)
            {
                Console.WriteLine("Owner has no pet!");
                return;
            }
            Console.WriteLine($"Cleaning after {owner.Pet.Name}...");
            Console.WriteLine($"Cleaning after {owner.Pet.Name} completed!");
        }

        public void ExecuteAction(IOwner owner, int actionId)
        {
            if (owner.Pet == null)
            {
                Console.WriteLine("Owner has no pet!");
                return;
            }

            switch (actionId)
            {
                case 1:
                    ExecuteMove(owner.Pet);
                    break;
                case 2:
                    ExecuteFly(owner.Pet);
                    break;
                case 3:
                    ExecuteSleep(owner.Pet);
                    break;
                default:
                    Console.WriteLine("Invalid action choice!");
                    break;
            }
        }

        private void ExecuteMove(Animal pet)
        {
            if (pet is IMovable movable)
            {
                movable.Move();
            }
            else
            {
                Console.WriteLine($"{pet.Name} cannot move in this way.");
            }
        }

        private void ExecuteFly(Animal pet)
        {
            if (pet is IFlyable flyable)
            {
                flyable.Fly();
            }
            else
            {
                Console.WriteLine($"{pet.Name} cannot fly.");
            }
        }

        private void ExecuteSleep(Animal pet)
        {
            pet.Sleep();
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