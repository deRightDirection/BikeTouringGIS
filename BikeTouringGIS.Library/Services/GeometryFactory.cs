using BikeTouringGISLibrary.Enumerations;
using BikeTouringGISLibrary.GPX;
using BikeTouringGISLibrary.Model;
using Esri.ArcGISRuntime.Geometry;
using System.Collections.Generic;
using System.Linq;
using theRightDirection.Library;

namespace BikeTouringGISLibrary.Services
{
    public sealed class GeometryFactory
    {
        private GpxInformation _gpxInformation;
        public GeometryFactory(GpxInformation gpxData)
        {
            _gpxInformation = gpxData;
        }
        public void CreateGeometries()
        {
            _gpxInformation.WayPoints.ForEach(wp => CreateWayPointGeometry(wp));
            var tracksToConvert = _gpxInformation.Tracks.Where(t => t.IsConvertedToRoute);
            var routes = new List<Route>();
            tracksToConvert.ForEach(t =>
            {
                Route newRoute = t.ConvertTrack();
                CreateRouteGeometry(newRoute);
                routes.Add(newRoute);
            });
            _gpxInformation.Routes = routes;
            var tracks = _gpxInformation.Tracks.Where(t => !t.IsConvertedToRoute);
            tracks.ForEach(t => CreateTrackGeometry(t));
            _gpxInformation.Tracks = tracks.ToList();
        }

        private void CreateTrackGeometry(Track track)
        {
            CreateGeometryAndExtentForTrackOrRoute(track, GraphicType.GPXTrack);
        }
        private void CreateRouteGeometry(Route route)
        {
            CreateGeometryAndExtentForTrackOrRoute(route, GraphicType.GPXRoute);
            route.StartLocation = CreateBikeTouringGISPointGraphic(route.Points.First(), route.Name, GraphicType.GPXRouteStartLocation);
            route.EndLocation = CreateBikeTouringGISPointGraphic(route.Points.Last(), route.Name, GraphicType.GPXRouteEndLocation);
        }

        private void CreateWayPointGeometry(WayPoint wayPoint)
        {
            var graphic = CreateBikeTouringGISPointGraphic(wayPoint.Points[0], wayPoint.Name, GraphicType.PointOfInterest);
            graphic.Attributes["source"] = wayPoint.Source;
            wayPoint.Geometry = graphic;
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
            data.Geometry = geometry;
        }

    }
}