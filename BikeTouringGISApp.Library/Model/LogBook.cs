using BikeTouringGISApp.Library.Enumerations;
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
    public class LogBook : ViewModelBase, IEntity<LogBook>
    {
        private string _name;

        public LogBook()
        {
            Identifier = Guid.NewGuid();
            LastModificationDate = DateTime.Now;
            Source = RepositorySource.Unknown;
        }

        [Display(Header = "Description", PlaceholderText = "description of the trip")]
        public string Description { get; set; }

        [Display(Header = "End date")]
        [Required]
        public DateTime EndDate { get; set; }

        [Ignore]
        public Guid Identifier { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [Ignore]
        public DateTime LastModificationDate { get; set; }

        [Display(Header = "Name", PlaceholderText = "name of the logbook")]
        [Required]
        public string Name
        {
            get { return _name; }
            set { Set(() => Name, ref _name, value); }
        }

        [Ignore]
        [JsonIgnore]
        public RepositorySource Source { get; set; }

        [Display(Header = "Start date")]
        [Required]
        public DateTime StartDate { get; set; }

        public int IsNewerThen(LogBook otherItem)
        {
            if (LastModificationDate > otherItem.LastModificationDate) { return 1; }
            if (LastModificationDate == otherItem.LastModificationDate) { return 0; }
            return -1;
        }
    }
}