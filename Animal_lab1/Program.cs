using AnimalSM.Core.factories;
using AnimalSM.Logic;

namespace Animal_lab1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var animalFactory = new AnimalFactory();
            var ownerFactory = new OwnerFactory();
            var service = new AnimalSimulationService(animalFactory, ownerFactory);
            var controller = new SimulationMenuController(service);

            service.AddOwnerWithAnimal("Mykola", 1, "Barsik");
            service.AddOwnerWithAnimal("Oleg", 3, "Kesha");
            service.AddAnimalOnly(2, "Loki");

            controller.Run();
        }
    }
}
