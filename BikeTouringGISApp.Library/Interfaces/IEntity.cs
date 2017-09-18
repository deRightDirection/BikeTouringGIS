using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikeTouringGISApp.Library.Interfaces
{
    public interface IEntity
    {
        Guid Identifier { get; set; }
    }
}