using BikeTouringGISApp.Library.Interfaces;

namespace BikeTouringGISApp.Library.Comparers
{
    public enum EntityComparerModificationDateMode
    {
        Unknown,
        ModificationDateIsGreaterThenOrEqualTo,
        ModificationDateIsSmallerThen
    }

    public class EntityComparer<T> : IEqualityComparer<T> where T : IEntity<T>
    {
        public bool Equals(T x, T y)
        {
            return x.Identifier.Equals(y.Identifier);
        }

        public int GetHashCode(T obj)
        {
            return obj.Identifier.GetHashCode();
        }
    }
}