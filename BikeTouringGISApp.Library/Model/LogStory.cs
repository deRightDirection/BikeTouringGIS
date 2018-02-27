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
        private LogStoryLanguage _language = LogStoryLanguage.Dutch;
        private string _storyText;

        [JsonConverter(typeof(StringEnumConverter))]
        public LogStoryLanguage Language
        {
            get { return _language; }
            set { _language = value; }
        }

        public string Text
        {
            get { return _storyText; }
            set { _storyText = value; }
        }
    }
}