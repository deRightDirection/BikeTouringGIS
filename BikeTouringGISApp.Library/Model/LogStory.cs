using BikeTouringGISApp.Library.Enumerations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikeTouringGISApp.Library.Model
{
    public class LogStory
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public LogStoryLanguage Language { get; set; } = LogStoryLanguage.Dutch;

        public string Text { get; set; }
    }
}