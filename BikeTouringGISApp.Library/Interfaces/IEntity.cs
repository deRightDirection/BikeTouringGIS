using BikeTouringGISApp.Library.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinUX.Data;

namespace BikeTouringGISApp.Library.Interfaces
{
    public interface IEntity<T> : IEntity
    {
        DateTime LastModificationDate { get; set; }
        RepositorySource Source { get; set; }

        /// <summary>
        /// 1 = newer 0 = equal
        /// -1 = older
        /// </summary>
        /// <param name="otherItem"></param>
        /// <returns></returns>
        int IsNewerThen(T otherItem);
    }
}