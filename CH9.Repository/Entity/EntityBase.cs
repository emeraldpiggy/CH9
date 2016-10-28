using System;

namespace CH9.Repository.Entity
{
    public abstract class EntityBase
    {
        public EntityStatus Status { get; set; }

        public Guid EntityId { get; set; }
    }
}