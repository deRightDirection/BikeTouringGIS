using BikeTouringGISLibrary.Model;
using GPX;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
namespace BikeTouringGISLibrary.UnitTests.Library.Model
{
    [TestClass]
    public class WayPointTest
    {
        [TestMethod]
        public void WayPoint_Cast_To_WptType()
        {
            var waypoint = new WayPoint();
            waypoint.Name = "mannus";
            wptType wpt = waypoint;
            wpt.name.ShouldBeEquivalentTo("mannus");
        }
    }
}
