using AnimalSM.Core.interfaces;

namespace AnimalSM.Core.interfaces
{
    public interface IOwner
    {
        string Name { get; }
        Guid Id { get; }
        IAnimal? Pet { get; }
        void Feed();
        void AdoptPet(IAnimal pet);
    }
}
