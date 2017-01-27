using Esri.ArcGISRuntime.Layers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BikeTouringGISLibrary.Model;
using BikeTouringGISLibrary;
using BikeTouringGISLibrary.Enumerations;
using System.Collections.Specialized;

namespace BikeTouringGIS.Controls
{
    public class BikeTouringGISLayer : GraphicsLayer
    {
        public BikeTouringGISLayer(string name)
        {
            Graphics.CollectionChanged += SetVisibility;
            DisplayName = name;
            IsVisible = false;
            Type = LayerType.Waypoints;
        }

        private void SetVisibility(object sender, NotifyCollectionChangedEventArgs e)
        {
            IsVisible = Graphics.Count > 0;
        }

        public LayerType Type { get;private set;}

        public BikeTouringGISLayer(string fileName, IEnumerable<IRoute> routes) : this(fileName)
        {
            foreach(var route in routes)
            {
                Graphics.Add(route.StartLocation);
                Graphics.Add(route.RouteGeometry);
                Graphics.Add(route.EndLocation);
            }
            Type = LayerType.Routes;
        }
    }
}