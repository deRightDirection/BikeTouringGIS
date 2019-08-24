using BikeTouringGISApp.Library.Interfaces;
using BikeTouringGISApp.Library.Enumerations;

namespace BikeTouringGISApp.Library.Model
{
    public class Log : IEntity<Log>, IEquatable<Log>
    {
        private List<LogStory> _stories;

        public Log()
        {
            Date = new DateTimeOffset(DateTime.Now);
            LastModificationDate = DateTime.Now;
            Identifier = Guid.NewGuid();
            Source = RepositorySource.Unknown;
            _stories = new List<LogStory>();
        }

        public DateTimeOffset Date { get; set; }
        public double Distance { get; set; }
        public string End { get; set; }
        public string FileName { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [Ignore]
        public Guid Identifier { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [Ignore]
        public DateTime LastModificationDate { get; set; }

        public double Latitude { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [Ignore]
        public Guid LogBook { get; set; }

        public double Longitude { get; set; }

        [Ignore]
        [JsonIgnore]
        public RepositorySource Source { get; set; }

        public string Start { get; set; }
        public IEnumerable<LogStory> Stories { get { return _stories; } }
        public int TravelTimeHours { get; set; }
        public int TravelTimeMinutes { get; set; }
        public int TravelTimeSeconds { get; set; }

        public void AddLogStory(LogStory story)
        {
            var findStoryWithSameLanguage = _stories.Where(x => x.Language == story.Language).FirstOrDefault();
            if (findStoryWithSameLanguage == null)
            {
                _stories.Add(story);
            }
            else
            {
                var index = _stories.IndexOf(findStoryWithSameLanguage);
                _stories[index] = story;
            }
        }

        public bool Equals(Log other)
        {
            return Identifier.Equals(other.Identifier);
        }

        public int IsNewerThen(Log otherItem)
        {
            if (LastModificationDate > otherItem.LastModificationDate) { return 1; }
            if (LastModificationDate == otherItem.LastModificationDate) { return 0; }
            return -1;
        }

        public void SetFileName()
        {
            FileName = $"{Guid.NewGuid().ToString()}.json";
        }
    }
}