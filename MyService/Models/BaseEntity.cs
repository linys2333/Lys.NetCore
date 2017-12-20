using System;

namespace MyService.Models
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

    public class BaseEntity : Entity<string>
    {
        public BaseEntity()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}