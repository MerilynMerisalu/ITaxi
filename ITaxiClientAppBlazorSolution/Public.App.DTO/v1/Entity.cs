using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Public.App.DTO.v1
{

    public abstract class Entity : Entity<Guid>
    { }


    public abstract class Entity<TKey> where TKey : IEquatable<TKey>
    {
        public TKey Id { get; set; }
    }
}
