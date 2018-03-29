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
using BikeTouringGISLibrary.Model;
using System.Collections.ObjectModel;
using BikeTouringGIS.Extensions;
using BikeTouringGISLibrary.Services;
using BikeTouringGIS.Services;
using WinUX;
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
        // bug #88
        public void RemoveLayer_Remove_Correct_PoIs()
        {
            var filename = "dwingeloo.gpx";
            var filename2 = "88.gpx";
            var path = Path.Combine(UnitTestDirectory, filename);
            var path2 = Path.Combine(UnitTestDirectory, filename2);
            List<BikeTouringGISLayer> layers = GetLayers(path);
            layers = GetLayers(path2);
            GetPOIsCount().ShouldBeEquivalentTo(165);
            _vm.RemoveLayerCommand.Execute(layers.Where(x => x.Type == LayerType.GPXRoute).First());
            GetPOIsCount().ShouldBeEquivalentTo(162);
        }

        [TestMethod]
        public void RemoveLayer_Removed_All_Poi_As_Well()
        {
            var filename = "dwingeloo.gpx";
            var path = Path.Combine(UnitTestDirectory, filename);
            List<BikeTouringGISLayer> layers = GetLayers(path);
            GetPOIsCount().ShouldBeEquivalentTo(3);
            _vm.RemoveLayerCommand.Execute(layers.Where(x => x.Type == LayerType.GPXRoute).First());
            GetPOIsCount().ShouldBeEquivalentTo(0);
        }

        [TestMethod]
        public void RemoveLayer_And_Remove_Pois_Only_After_All_Layers_From_Same_File_Are_Removed()
        {
            var file = "Sample.gpx";
            var path = Path.Combine(UnitTestDirectory, file);
            List<BikeTouringGISLayer> layers = GetLayers(path);
            GetPOIsCount().ShouldBeEquivalentTo(49);
            layers.Where(x => x.Type == LayerType.GPXRoute).ForEach(x =>
            {
                GetPOIsCount().ShouldBeEquivalentTo(49);
                _vm.RemoveLayerCommand.Execute(x);
            });
            GetPOIsCount().ShouldBeEquivalentTo(0);
        }

        // bug #69
        [TestMethod]
        public void Save_And_Check_For_POIs()
        {
            string fileName = "65.gpx";
            var path = Path.Combine(UnitTestDirectory, fileName);
            var layers = GetLayers(path);
            GetPOIsCount().ShouldBeEquivalentTo(4);
            _vm.SaveLayerCommand.Execute(layers.Where(x => x.Type == LayerType.GPXRoute).First());
            var gpxInfo = new GpxFileReader().LoadFile(path);
            gpxInfo.WayPoints.Count.ShouldBeEquivalentTo(4);
        }


        private List<BikeTouringGISLayer> GetLayers(string path)
        {
            var gpxData = GetGPXData(path);
            _vm.AddPoIs(gpxData.WayPoints);
            var layerFactory = new LayerFactory(gpxData.WayPointsExtent);
            _vm.AddRoutes(layerFactory.CreateRoutes(gpxData.Routes));
            _vm.BikeTouringGISLayers = new ObservableCollection<BikeTouringGISLayer>(_vm.Map.GetBikeTouringGISLayers());
            return _vm.BikeTouringGISLayers.ToList();
        }

        private GpxInformation GetGPXData(string fileName)
        {
            var gpxFileInformation = new GpxFileReader().LoadFile(fileName);
            gpxFileInformation.Tracks.ForEach(t => t.IsConvertedToRoute = true);
            var factory = new GeometryFactory(gpxFileInformation);
            factory.CreateGeometries();
            return gpxFileInformation;
        }

        private int GetPOIsCount()
        {
            return _vm.BikeTouringGISLayers.Where(x => x.Type == LayerType.PointsOfInterest).First().Graphics.Count;
        }
    }
}