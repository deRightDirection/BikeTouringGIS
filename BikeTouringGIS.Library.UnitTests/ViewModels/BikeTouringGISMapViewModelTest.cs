using BikeTouringGIS.Controls;
using BikeTouringGIS.ViewModels;
using BikeTouringGISLibrary.Enumerations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using theRightDirection.Library.UnitTesting;
using FluentAssertions;
using Esri.ArcGISRuntime.Symbology;

namespace BikeTouringGISLibrary.UnitTests.ViewModels
{
    [TestClass]
    public class BikeTouringGISMapViewModelTest : UnitTestingBase
    {
        private BikeTouringGISMapViewModel _vm;

        [TestInitialize]
        public void Setup()
        {
            _vm = new BikeTouringGISMapViewModel(true);
            _vm.AddSymbol(GraphicType.GPXRoute, new SimpleLineSymbol());
            _vm.AddSymbol(GraphicType.GPXRouteEndLocation, new SimpleMarkerSymbol());
            _vm.AddSymbol(GraphicType.GPXRouteStartLocation, new SimpleMarkerSymbol());
            _vm.AddSymbol(GraphicType.PoILabelL, new TextSymbol());
            _vm.AddSymbol(GraphicType.PoILabelM, new TextSymbol());
            _vm.AddSymbol(GraphicType.PoILabelXL, new TextSymbol());
            _vm.AddSymbol(GraphicType.PointOfInterest, new SimpleMarkerSymbol());
            _vm.AddSymbol(GraphicType.SplitPoint, new SimpleMarkerSymbol());
            _vm.AddSymbol(GraphicType.SplitPointLabel, new TextSymbol());
            _vm.AddSymbol(GraphicType.SplitRoute, new SimpleLineSymbol());
        }

        [TestMethod]
        public void RemoveLayer_Removed_All_Poi_As_Well()
        {
            var file = "dwingeloo.gpx";
            var path = Path.Combine(UnitTestDirectory, file);
            var gpxFileInformation = new GpxFileReader().LoadFile(path);
            foreach (var track in gpxFileInformation.Tracks)
            {
                track.ConvertTrackToRoute();
            }
            gpxFileInformation.CreateGeometries();
            var route = gpxFileInformation.AllRoutes.First();
            var layer = new BikeTouringGISLayer(path, route);
            _vm.AddRoutes(layer);
            _vm.AddPoIs(gpxFileInformation.WayPoints);
            var poisCount = _vm.BikeTouringGISLayers.Where(x => x.Type == LayerType.PointsOfInterest).First().Graphics.Count;
            poisCount.ShouldBeEquivalentTo(3);
            _vm.RemoveLayerCommand.Execute(layer);
            poisCount = _vm.BikeTouringGISLayers.Where(x => x.Type == LayerType.PointsOfInterest).First().Graphics.Count;
            poisCount.ShouldBeEquivalentTo(0);
        }
    }
}