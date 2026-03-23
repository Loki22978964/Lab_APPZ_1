using AnimalSM.Core.models;

namespace AnimalSM.Core.interfaces
{
    public interface IOwner
    {
        string Name { get; }
        Guid Id { get; }
        Animal? Pet { get; }
        void Feed();
        void AdoptPet(Animal pet);
    }
}
