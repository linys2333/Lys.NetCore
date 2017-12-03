using System;

namespace MyWebAPI.Models
{
    public interface IEntity
    {
    }

    public interface IEntity<T> : IEntity
    {
        T Id { get; set; }
    }

    public abstract class Entity<T> : IEntity<T>
    {
        public T Id { get; set; }
    }

    public class BazaEntityBase : Entity<string>
    {
        public BazaEntityBase()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}