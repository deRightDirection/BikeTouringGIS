using BikeTouringGIS.Controls;
using BikeTouringGISLibrary.Enumerations;
using BikeTouringGISLibrary.Model;
using GPX;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using theRightDirection.Library.UnitTesting;

namespace BikeTouringGISLibrary.UnitTests
{
    [TestClass]
    public class BikeTouringGISLayerComparerTest : UnitTestingBase
    {
        [TestInitialize]
        public void Setup()
        {
        }

        [TestMethod]
        public void OrderBy()
        {
            Mock<IPath> mock = new Mock<IPath>();
            var list = new List<BikeTouringGISLayer>() { new BikeTouringGISLayer("abc", mock.Object), new BikeTouringGISLayer("Points of Interest"), new BikeTouringGISLayer("def", mock.Object) };
            var result = list.OrderBy(x => x.Type);
            var firstItem = result.First().Type == LayerType.PointsOfInterest;
            Assert.AreEqual(firstItem, true);
        }
    }
}