using BikeTouringGISLibrary.Enumerations;
using BikeTouringGISLibrary.Model;
using Esri.ArcGISRuntime.Geometry;
using GPX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinUX;
namespace BikeTouringGISLibrary.Services
{
    public sealed class GeometryFactory
    {
        private GpxInformation _gpxInformation;
        public GeometryFactory(GpxInformation gpxData)
        {
            _gpxInformation = gpxData;
        }
        public GeometryFactory() { }
        public void CreateGeometries()
        {
            _gpxInformation.WayPoints.ForEach(wp => CreateWayPointGeometry(wp));
            var tracksToConvert = _gpxInformation.Tracks.Where(t => t.IsConvertedToRoute);
            _gpxInformation.Routes.ForEach(r =>
            {
                CreateRouteGeometry(r);
            });
            var routes = new List<Route>();
            tracksToConvert.ForEach(t =>
            {
                Route newRoute = t.ConvertTrack();
                CreateRouteGeometry(newRoute);
                routes.Add(newRoute);
            });
            _gpxInformation.Routes.AddRange(routes);
            var tracks = _gpxInformation.Tracks.Where(t => !t.IsConvertedToRoute);
            tracks.ForEach(t => CreateTrackGeometry(t));
            _gpxInformation.Tracks = tracks.ToList();
        }

        private void CreateTrackGeometry(Track track)
        {
            CreateGeometryAndExtentForTrackOrRoute(track, GraphicType.GPXTrack);
        }
        public void CreateRouteGeometry(Route route)
        {
            CreateGeometryAndExtentForTrackOrRoute(route, GraphicType.GPXRoute);
            route.StartLocation = CreateBikeTouringGISPointGraphic(route.Points.First(), route.Name, GraphicType.GPXRouteStartLocation);
            route.EndLocation = CreateBikeTouringGISPointGraphic(route.Points.Last(), route.Name, GraphicType.GPXRouteEndLocation);
            if (string.IsNullOrEmpty(route.FileName))
            {
                route.FileName = _gpxInformation.FileName;
            }
        }

        private void CreateWayPointGeometry(WayPoint wayPoint)
        {
            var graphic = CreateBikeTouringGISPointGraphic(wayPoint.Points[0], wayPoint.Name, GraphicType.PointOfInterest);
            graphic.Attributes["filename"] = wayPoint.FileName;
            wayPoint.Geometry = graphic;
            wayPoint.FileName = _gpxInformation.FileName;
            wayPoint.Extent = graphic.Geometry.Extent;
        }

        private BikeTouringGISGraphic CreateBikeTouringGISPointGraphic(wptType location, string nameAttribute, GraphicType typeOfPoint)
        {
            var mappoint = new MapPoint((double)location.lon, (double)location.lat, new SpatialReference(4326));
            var g = new BikeTouringGISGraphic(mappoint, typeOfPoint)
            {
                ZIndex = 1
            };
            g.Attributes["name"] = nameAttribute;
            g.Attributes["type"] = typeOfPoint;
            return g;
        }
        private void CreateGeometryAndExtentForTrackOrRoute(GeometryData data, GraphicType typeOfGraphic)
        {
            var builder = new PolylineBuilder(new SpatialReference(4326));
            foreach (var wayPoint in data.Points)
            {
                builder.AddPoint(new MapPoint((double)wayPoint.lon, (double)wayPoint.lat));
            }
            var esriGeometry = builder.ToGeometry();
            data.Extent = esriGeometry.Extent;
            var geometry = new BikeTouringGISGraphic(esriGeometry, typeOfGraphic);
            geometry.Attributes["name"] = data.Name;
            geometry.Attributes["filename"] = data.FileName;
            geometry.Attributes["type"] = typeOfGraphic;
            data.Geometry = geometry;
            if (string.IsNullOrEmpty(data.FileName))
            {
                data.FileName = _gpxInformation.FileName;
            }
        }

    }
}
