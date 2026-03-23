using AnimalSM.Core.models;

namespace AnimalSM.Core.interfaces
{
    public interface IAnimalFactory
    {
        Animal? CreateAnimal(int typeId, string? name = null);
        IEnumerable<int> GetAvailableAnimalTypeIds();
    }
}
