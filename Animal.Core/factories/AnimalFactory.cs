using AnimalSM.Core.interfaces;
using AnimalSM.Core.models;
using System.Collections.Generic;

namespace AnimalSM.Core.factories
{
    public class AnimalFactory : IAnimalFactory
    {
        private readonly Dictionary<int, Func<string?, IAnimal>> _creators;

        public AnimalFactory()
        {
            _creators = new Dictionary<int, Func<string?, IAnimal>>
            {
                { 1, name => string.IsNullOrEmpty(name) ? new Cat() : new Cat(name) },
                { 2, name => string.IsNullOrEmpty(name) ? new Snake() : new Snake(name) },
                { 3, name => string.IsNullOrEmpty(name) ? new Parrot() : new Parrot(name) }
            };
        }

        public void RegisterAnimalType(int typeId, Func<string?, IAnimal> creator)
        {
            _creators[typeId] = creator;
        }

        public IAnimal? CreateAnimal(int typeId, string? name = null)
        {
            if (_creators.TryGetValue(typeId, out var creator))
            {
                return creator(name);
            }
            return null;
        }

        public IEnumerable<int> GetAvailableAnimalTypeIds() => _creators.Keys;
    }
}
