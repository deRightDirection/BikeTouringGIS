using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinUX;

namespace BikeTouringGISApp.Library.Model
{
    public class Log
    {
        private List<LogStory> _stories;

        public Log()
        {
            Date = new DateTimeOffset(DateTime.Now);
            _stories = new List<LogStory>();
        }

        public DateTimeOffset Date { get; set; }
        public double Distance { get; set; }
        public string End { get; set; }
        public string FileName { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
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

        public void SetFileName()
        {
            FileName = $"{Guid.NewGuid().ToString()}.json";
        }
    }
}