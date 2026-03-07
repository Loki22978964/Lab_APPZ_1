using AnimalSM.Core.interfaces;
using System.Collections.Generic;
using System.Linq;

namespace AnimalSM.Logic
{
    public class SimulationMenuController
    {
        private readonly AnimalSimulationService _service;

        private static readonly Dictionary<int, string> AnimalTypeNames = new()
        {
            { 1, "Cat" },
            { 2, "Snake" },
            { 3, "Parrot" }
        };

        public SimulationMenuController(AnimalSimulationService service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        public void Run()
        {
            Console.WriteLine("=== Animal Simulation ===");
            Console.WriteLine();

            while (true)
            {
                ShowMainMenu();
                var choice = Console.ReadLine() ?? "";

                switch (choice)
                {
                    case "1":
                        HandleAddOwnerWithAnimal();
                        break;
                    case "2":
                        HandleAddAnimalToExistingOwner();
                        break;
                    case "3":
                        HandleAddAnimalOnly();
                        break;
                    case "4":
                        HandleFeedPet();
                        break;
                    case "5":
                        HandleCleanPet();
                        break;
                    case "6":
                        HandleExecuteAction();
                        break;
                    case "7":
                        HandleUpdateStatus();
                        break;
                    case "8":
                        HandleDisplayInfo();
                        break;
                    case "0":
                        Console.WriteLine("Goodbye!");
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }

                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
                Console.Clear();
            }
        }

        private void ShowMainMenu()
        {
            Console.WriteLine("=== Main Menu ===");
            Console.WriteLine("1. Add owner with animal");
            Console.WriteLine("2. Add animal to existing owner");
            Console.WriteLine("3. Add animal only (without owner)");
            Console.WriteLine("4. Feed pet");
            Console.WriteLine("5. Clean pet");
            Console.WriteLine("6. Execute action with pet");
            Console.WriteLine("7. Update status");
            Console.WriteLine("8. Display information");
            Console.WriteLine("0. Exit");
            Console.Write("\nSelect option: ");
        }

        private void HandleAddOwnerWithAnimal()
        {
            Console.WriteLine("\n=== Adding Owner with Animal ===");
            Console.Write("Enter owner name: ");
            var ownerName = Console.ReadLine() ?? "";

            var (animalTypeId, petName, success) = GetAnimalChoice();
            if (!success) return;

            _service.AddOwnerWithAnimal(ownerName, animalTypeId, petName);
        }

        private void HandleAddAnimalToExistingOwner()
        {
            Console.WriteLine("\n=== Adding Animal to Existing Owner ===");

            if (_service.Owners.Count == 0)
            {
                Console.WriteLine("No owners found! Please add an owner first.");
                return;
            }

            DisplayOwners();
            Console.Write("\nSelect owner number: ");
            if (!int.TryParse(Console.ReadLine(), out int ownerIndex))
            {
                Console.WriteLine("Invalid owner number!");
                return;
            }

            var selectedOwner = _service.Owners[ownerIndex - 1];
            if (selectedOwner.Pet != null)
            {
                Console.WriteLine($"Owner '{selectedOwner.Name}' already has a pet '{selectedOwner.Pet.Name}'.");
                Console.Write("Replace? (y/n): ");
                if (Console.ReadLine()?.ToLower() != "y")
                    return;
            }

            var (animalTypeId, petName, success) = GetAnimalChoice();
            if (!success) return;

            _service.AddAnimalToExistingOwner(ownerIndex, animalTypeId, petName);
        }

        private void HandleAddAnimalOnly()
        {
            Console.WriteLine("\n=== Adding Animal Only ===");

            var (animalTypeId, petName, success) = GetAnimalChoice();
            if (!success) return;

            _service.AddAnimalOnly(animalTypeId, petName);
        }

        private (int animalTypeId, string petName, bool success) GetAnimalChoice()
        {
            Console.WriteLine("\nSelect animal type:");
            foreach (var id in _service.AvailableAnimalTypeIds)
            {
                //asdasdasdasd
                var name = AnimalTypeNames.GetValueOrDefault(id, $"Type {id}");
                Console.WriteLine($"{id} — {name}");
            }
            Console.Write("Choice: ");

            if (!int.TryParse(Console.ReadLine(), out int animalChoice) || !_service.AvailableAnimalTypeIds.Contains(animalChoice))
            {
                Console.WriteLine("Invalid animal choice!");
                return (0, "", false);
            }

            Console.Write("Enter animal name: ");
            var petName = Console.ReadLine() ?? "";
            if (string.IsNullOrWhiteSpace(petName))
            {
                Console.WriteLine("Animal name cannot be empty!");
                return (0, "", false);
            }

            return (animalChoice, petName, true);
        }

        private void HandleFeedPet()
        {
            Console.WriteLine("\n=== Feeding Pet ===");
            var owner = SelectOwnerWithPet();
            if (owner == null) return;

            _service.FeedPet(owner);
        }

        private void HandleCleanPet()
        {
            Console.WriteLine("\n=== Cleaning Pet ===");
            var owner = SelectOwnerWithPet();
            if (owner == null) return;

            _service.CleanPet(owner);
        }

        private void HandleExecuteAction()
        {
            Console.WriteLine("\n=== Execute Action with Pet ===");
            var owner = SelectOwnerWithPet();
            if (owner == null || owner.Pet == null) return;

            Console.WriteLine("\nSelect action:");
            Console.WriteLine("1. Move");
            Console.WriteLine("2. Fly (if possible)");
            Console.WriteLine("3. Sleep");
            Console.Write("Choice: ");

            if (!int.TryParse(Console.ReadLine(), out int actionId))
            {
                Console.WriteLine("Invalid action choice!");
                return;
            }

            _service.ExecuteAction(owner, actionId);
        }

        private void HandleUpdateStatus()
        {
            Console.WriteLine("\n=== Update Status ===");

            if (_service.Owners.Count == 0)
            {
                Console.WriteLine("No owners with pets found!");
                return;
            }

            _service.UpdateStatus();
            Console.WriteLine("✓ Status updated!");
        }

        private void HandleDisplayInfo()
        {
            Console.WriteLine("\n=== Information about Owners and Animals ===");

            if (_service.Owners.Count == 0)
            {
                Console.WriteLine("No owners found!");
                return;
            }

            for (int i = 0; i < _service.Owners.Count; i++)
            {
                var owner = _service.Owners[i];
                Console.WriteLine($"\n--- Owner #{i + 1} ---");
                Console.WriteLine($"Name: {owner.Name}");
                Console.WriteLine($"ID: {owner.Id}");

                if (owner.Pet != null)
                {
                    Console.WriteLine($"Pet: {owner.Pet.Name} ({owner.Pet.GetType().Name})");
                    Console.WriteLine($"  Alive: {(owner.Pet.IsAlive ? "Yes" : "No")}");
                    Console.WriteLine($"  Happy: {(owner.Pet.IsHappy ? "Yes" : "No")}");
                    Console.WriteLine($"  Last feeding: {owner.Pet.LastFeedingTime:yyyy-MM-dd HH:mm:ss}");
                }
                else
                {
                    Console.WriteLine("Pet: none");
                }
            }
        }

        private IOwner? SelectOwnerWithPet()
        {
            var ownersWithPets = _service.GetOwnersWithPets();
            if (ownersWithPets.Count == 0)
            {
                Console.WriteLine("No owners with pets found!");
                return null;
            }

            Console.WriteLine("\nOwners with pets:");
            for (int i = 0; i < ownersWithPets.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {ownersWithPets[i].Name} - {ownersWithPets[i].Pet?.Name}");
            }

            Console.Write("Select number: ");
            if (!int.TryParse(Console.ReadLine(), out int index))
            {
                Console.WriteLine("Invalid number!");
                return null;
            }

            return _service.SelectOwnerWithPet(index);
        }

        private void DisplayOwners()
        {
            Console.WriteLine("\nList of owners:");
            for (int i = 0; i < _service.Owners.Count; i++)
            {
                var petInfo = _service.Owners[i].Pet != null
                    ? $" - {_service.Owners[i].Pet!.Name}"
                    : " (no pet)";
                Console.WriteLine($"{i + 1}. {_service.Owners[i].Name}{petInfo}");
            }
        }
    }
}
