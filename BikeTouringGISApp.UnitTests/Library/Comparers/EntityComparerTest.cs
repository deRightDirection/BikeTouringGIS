using BikeTouringGISApp.Library.Comparers;
using BikeTouringGISApp.Library.Enumerations;
using BikeTouringGISApp.Library.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikeTouringGISApp.UnitTests.Library.Comparers
{
    [TestClass]
    public class EntityComparerTest
    {
        private EntityComparer<Log> _comparer;

        [TestMethod]
        public void CheckOfItemIsInOneDrive()
        {
            var log = new Log();
            var oneDrive = new List<Log>() { log };
            var local = new List<Log>() { log };
            var itemsNotInOneDrive = local.Except(oneDrive, _comparer);
            Assert.AreEqual(itemsNotInOneDrive.Count(), 0);
        }

        [TestMethod]
        public void CheckOfItemIsMissingInOneDrive()
        {
            var oneDrive = new List<Log>();
            var local = new List<Log>() { new Log() };
            var itemsNotInOneDrive = local.Except(oneDrive, _comparer);
            Assert.AreEqual(itemsNotInOneDrive.Count(), 1);
        }

        [TestInitialize]
        public void Setup()
        {
            _comparer = new EntityComparer<Log>();
        }
    }
}