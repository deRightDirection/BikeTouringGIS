namespace BikeTouringGISLibrary.Model
{
    public class BikeTouringGISProject
    {
        public string Description { get; set; }

        public bool IsLoaded
        {
            get
            {
                return !string.IsNullOrEmpty(ProjectFileLocation) && !string.IsNullOrEmpty(Name);
            }
        }

        public string Name { get; set; }
        public string ProjectFileLocation { get; set; }
    }
}