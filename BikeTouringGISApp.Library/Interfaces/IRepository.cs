using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikeTouringGISApp.Library.Interfaces
{
    public interface IRepository<T> where T : IEntity
    {
        IEnumerable<T> Entities { get; }

        Task AddEntity(T entityToAdd);

        Task DeleteEntity(T entityToDelete);

        Task LoadData();

        Task ModifyEntity(T entityToModify);
    }
}