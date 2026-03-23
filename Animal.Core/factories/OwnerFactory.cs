using AnimalSM.Core.interfaces;
using AnimalSM.Core.services;
using AnimalSM.Core.models;
namespace AnimalSM.Core.factories
{
    public class OwnerFactory : IOwnerFactory
    {
        public IOwner Create() => new Owner();
        public IOwner Create(string name, Animal pet) => new Owner(name, pet);
    }
}
