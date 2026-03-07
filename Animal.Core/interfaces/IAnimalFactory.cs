namespace AnimalSM.Core.interfaces
{
    public interface IAnimalFactory
    {
        IAnimal? CreateAnimal(int typeId, string? name = null);
        IEnumerable<int> GetAvailableAnimalTypeIds();
    }
}
