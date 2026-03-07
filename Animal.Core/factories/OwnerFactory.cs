using AnimalSM.Core.interfaces;
using AnimalSM.Core.services;

namespace AnimalSM.Core.factories
{
    public class OwnerFactory : IOwnerFactory
    {
        public IOwner Create() => new Owner();
        public IOwner Create(string name, IAnimal pet) => new Owner(name, pet);
    }
}
