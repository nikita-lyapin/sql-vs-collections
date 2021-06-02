using System;

namespace CollectionsBenchmark
{
    internal sealed class Visit : IComparable<Visit>
    {
        internal int Id { get; set; }
        internal int PersonId { get; set; }

        internal DateTime Date { get; set; }
        internal decimal Spent { get; set; }

        public int CompareTo(Visit other)
        {
            return PersonId.CompareTo(other.PersonId);
        }
    }
}