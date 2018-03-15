using BikeTouringGIS.Comparers;
using BikeTouringGIS.Controls;
using BikeTouringGISLibrary.Enumerations;
using BikeTouringGISLibrary.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using theRightDirection.Library.UnitTesting;
using FluentAssertions;
namespace BikeTouringGISLibrary.UnitTests.Comparers
{
    [TestClass]
    public class BikeTouringGISLayerComparerTest : UnitTestingBase
    {
        private BikeTouringGISLayerComparer _comparer;

        [TestInitialize]
        public void Setup()
        {
            _comparer = new BikeTouringGISLayerComparer();
        }
        [TestMethod]
        public void PointOfInterestLayers_Comes_First()
        {
            var items = new List<BikeTouringGISLayer>() { GetPOILayer(), GetPOILayer() };
            var ordered = items.OrderBy(x => x.Type, _comparer);
            var first = ordered.First();
            first.Type.ShouldBeEquivalentTo(LayerType.PointsOfInterest);
        }

        [TestMethod]
        public void PointOfInterestLayers_Comes_First2()
        {
            var items = new List<BikeTouringGISLayer>() { GetRouteLayer(), GetPOILayer() };
            var ordered = items.OrderBy(x => x.Type, _comparer);
            var first = ordered.First();
            first.Type.ShouldBeEquivalentTo(LayerType.PointsOfInterest);
        }

        [TestMethod]
        public void PointOfInterestLayers_Comes_First3()
        {
            var items = new List<BikeTouringGISLayer>() { GetTrackLayer(), GetRouteLayer(), GetPOILayer() };
            var ordered = items.OrderBy(x => x.Type, _comparer);
            var first = ordered.First();
            first.Type.ShouldBeEquivalentTo(LayerType.PointsOfInterest);
        }

        private BikeTouringGISLayer GetPOILayer()
        {
            return new BikeTouringGISLayer("POI");
        }

        private BikeTouringGISLayer GetRouteLayer()
        {
            var route = new Route();
            route.Type = PathType.Route;
            var layer = new BikeTouringGISLayer("route", route);
            return layer;
        }

        private BikeTouringGISLayer GetTrackLayer()
        {
            var route = new Track();
            route.Type = PathType.Track;
            var layer = new BikeTouringGISLayer("route", route);
            return layer;
        }
    }
}
