using BikeTouringGISLibrary.Model;
using System;
using System.Globalization;
using System.Windows.Data;

namespace BikeTouringGIS.Converters
{
    internal class ProjectIsNotNullConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return false;
            }
            var project = value as BikeTouringGISProject;
            return project.IsLoaded;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}