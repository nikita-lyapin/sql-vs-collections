using System;

namespace CollectionsBenchmark
{
    internal sealed class Person : IComparable<Person>
    {
        internal int Id { get; set; }
        internal string FirstName { get; set; }
        internal string LastName { get; set; }
        internal bool IsActive { get; set; }

        public int CompareTo(Person other)
        {
            return Id.CompareTo(other.Id);
        }
    }
}