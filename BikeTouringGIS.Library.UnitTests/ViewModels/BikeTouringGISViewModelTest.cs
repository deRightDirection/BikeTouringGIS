using BikeTouringGIS.ViewModels;
using BikeTouringGISLibrary.Enumerations;
using Esri.ArcGISRuntime.Symbology;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using theRightDirection.Library.UnitTesting;
using FluentAssertions;
namespace BikeTouringGISLibrary.UnitTests.ViewModels
{
    [TestClass]
    public class BikeTouringGISViewModelTest : UnitTestingBase
    {
        private BikeTouringGISViewModel _vm;
        private BikeTouringGISMapViewModel _map;

        [TestInitialize]
        public void Setup()
        {
            _vm = new BikeTouringGISViewModel();
            _map = new BikeTouringGISMapViewModel(true);
            _map.AddSymbol(GraphicType.GPXRoute, new SimpleLineSymbol());
            _map.AddSymbol(GraphicType.GPXRouteEndLocation, new SimpleMarkerSymbol());
            _map.AddSymbol(GraphicType.GPXRouteStartLocation, new SimpleMarkerSymbol());
            _map.AddSymbol(GraphicType.PoILabelL, new TextSymbol());
            _map.AddSymbol(GraphicType.PoILabelM, new TextSymbol());
            _map.AddSymbol(GraphicType.PoILabelXL, new TextSymbol());
            _map.AddSymbol(GraphicType.PointOfInterest, new SimpleMarkerSymbol());
            _map.AddSymbol(GraphicType.SplitPoint, new SimpleMarkerSymbol());
            _map.AddSymbol(GraphicType.SplitPointLabel, new TextSymbol());
            _map.AddSymbol(GraphicType.SplitRoute, new SimpleLineSymbol());
        }

        [TestMethod]
        // #85
        public void OpenGPXFile_Cant_Load_Two_Times_Same_File()
        {
            var filename = "85.gpx";
            var path = Path.Combine(UnitTestDirectory, filename);
            _vm.OpenGpxFile(_map, path);
            _map.BikeTouringGISLayers.Count.ShouldBeEquivalentTo(1);
            _vm.OpenGpxFile(_map, path);
            _map.BikeTouringGISLayers.Count.ShouldBeEquivalentTo(1);
        }


    }
}