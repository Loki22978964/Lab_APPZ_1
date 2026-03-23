using AnimalSM.Core.interfaces;
using AnimalSM.Core.models;

namespace AnimalSM.Core.interfaces
{
    public interface IOwnerFactory
    {
        IOwner Create();
        IOwner Create(string name, Animal pet);
    }
}
