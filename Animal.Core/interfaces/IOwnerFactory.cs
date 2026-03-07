using AnimalSM.Core.interfaces;

namespace AnimalSM.Core.interfaces
{
    public interface IOwnerFactory
    {
        IOwner Create();
        IOwner Create(string name, IAnimal pet);
    }
}
