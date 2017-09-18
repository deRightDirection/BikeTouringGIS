using BikeTouringGISApp.Library.Interfaces;
using GalaSoft.MvvmLight;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.Data.Core;

namespace BikeTouringGISApp.Library.Model
{
    public class LogBook : ViewModelBase, IEntity
    {
        private string _name;

        public LogBook()
        {
            Identifier = Guid.NewGuid();
        }

        [Display(Header = "Description", PlaceholderText = "description of the trip")]
        public string Description { get; set; }

        [Display(Header = "End date")]
        [Required]
        public DateTime EndDate { get; set; }

        [Ignore]
        public Guid Identifier { get; set; }

        [Display(Header = "Name", PlaceholderText = "name of the logbook")]
        [Required]
        public string Name
        {
            get { return _name; }
            set { Set(() => Name, ref _name, value); }
        }

        [Display(Header = "Start date")]
        [Required]
        public DateTime StartDate { get; set; }
    }
}