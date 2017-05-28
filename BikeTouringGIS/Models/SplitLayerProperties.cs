using BikeTouringGIS.Controls;

namespace BikeTouringGIS.Models
{
    public class SplitLayerProperties
    {
        public BikeTouringGISLayer Layer { get; set; }
        public int Distance { get; set; }

        public bool CanSplit
        {
            get { return Distance < Layer.TotalLength && Distance > 0; }
        }

        public bool CanReSplit
        {
            get
            {
                if (Layer == null)
                {
                    return false;
                }
                return Layer.IsSplitted && Distance > 0;
            }
        }
    }
}