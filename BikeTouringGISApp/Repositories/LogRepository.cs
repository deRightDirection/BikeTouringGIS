using BikeTouringGISApp.Library.Enumerations;
using BikeTouringGISApp.Library.Interfaces;
using BikeTouringGISApp.Library.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinUX;

namespace BikeTouringGISApp.Repositories
{
    public class LogRepository : Repository<Log>, IRepository<Log>
    {
        private IEnumerable<Log> _entities = null;

        public LogRepository() : base("logs")
        {
        }

        public IEnumerable<Log> Entities => _entities;

        public async Task AddEntity(Log entityToAdd)
        {
            await Save(entityToAdd);
        }

        public async Task DeleteEntity(Log entityToDelete)
        {
            await Delete(entityToDelete);
        }

        public async Task LoadData()
        {
            _entities = await GetData();
            _entities.ForEach(x => x.Source = RepositorySource.Local);
        }

        public Task ModifyEntity(Log entityToModify)
        {
            throw new NotImplementedException();
        }
    }
}