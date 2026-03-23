using AnimalSM.Core.models;
using AnimalSM.Core.interfaces;
using AnimalSM.Core.factories;
using System;

namespace AnimalSM.Core.services
{
    public class Owner : IOwner
    {
        public string Name { get; }
        public Guid Id { get; }
        public Animal? Pet { get; private set; }

        public Owner()
        {
            this.Name = "Guest_" + Guid.NewGuid();
            this.Id = Guid.NewGuid();
        }

        public Owner(string name ,Animal pet)
        {
            this.Name = name;
            this.Pet = pet;
            Id = Guid.NewGuid();
        }

        public void AdoptPet(Animal pet)
        {
            this.Pet = pet;
        }

        public void Feed()
        {
            Pet?.Eat();
        }
    }
}