using BikeTouringGISApp.Library.Interfaces;
using BikeTouringGISApp.Library.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikeTouringGISApp.Repositories
{
    public class LogBookRepository : Repository<LogBook>, IRepository<LogBook>
    {
        private IEnumerable<LogBook> _entities = null;

        public LogBookRepository() : base("logbooks")
        {
        }

        public IEnumerable<LogBook> Entities => _entities;

        public async Task AddEntity(LogBook entityToAdd)
        {
            await Save(entityToAdd);
        }

        public async Task DeleteEntity(LogBook entityToDelete)
        {
            await Delete(entityToDelete);
        }

        public async Task LoadData()
        {
            _entities = await GetData();
        }

        public Task ModifyEntity(LogBook entityToModify)
        {
            throw new NotImplementedException();
        }
    }
}