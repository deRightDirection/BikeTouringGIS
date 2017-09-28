using BikeTouringGISApp.Library.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinUX;

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