using BikeTouringGIS.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using theRightDirection.Library.UnitTesting;
using FluentAssertions;
using BikeTouringGIS.Controls;
using BikeTouringGISLibrary.Enumerations;
using Moq;
using BikeTouringGISLibrary.Model;
using AutoFixture;
namespace BikeTouringGISLibrary.UnitTests.ViewModels
{
    [TestClass]
    public class LayerListViewModelTest : UnitTestingBase
    {
        private LayerListViewModel _vm;
        private IPath _route;

        [TestInitialize]
        public void Setup()
        {
            _vm = new LayerListViewModel();
            _route = new Mock<IPath>().Object;
        }

        [TestMethod]
        public void SelectedLayer_Is_Null_Initial()
        {
            _vm.SelectedLayer.Should().BeNull();
        }

        [TestMethod]
        public void SelectedLayer_Is_Not_Null_With_SplitRouteLayer()
        {
            var layer = new BikeTouringGISLayer("test", _route);
            _vm.SelectedLayer = layer.SplitLayer;
            _vm.SelectedLayer.Type.Should().Be(LayerType.SplitRoutes);
        }

        [TestMethod]
        public void SelectedLayer_Is_Not_Null_With_GPXLayer()
        {
            _route = _fixture.Build<Route>().OmitAutoProperties().Create();
            var layer = new BikeTouringGISLayer("test", _route);
            _vm.SelectedLayer = layer;
            _vm.SelectedLayer.Type.Should().Be(LayerType.GPXRoute);
        }

        [TestMethod]
        public void SelectedLayer_Is_Null_With_PoiLayer()
        {
            var layer = new BikeTouringGISLayer("poi");
            _vm.SelectedLayer = layer;
            _vm.SelectedLayer.Should().BeNull();
        }
    }
}