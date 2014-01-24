using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Globalization;
using StarshipSeven.GameInterfaces.Strategy.Attributes;
using System.Windows.Media;

namespace StarshipSeven.Presentation.Converters
{
    [ValueConversion(typeof(DifficultyLevel), typeof(Color))]
    public class DifficultyLevelToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DifficultyLevel level = (DifficultyLevel)value;
            switch (level)
            {
                case DifficultyLevel.Easy:
                    return Colors.Green;
                case DifficultyLevel.Medium:
                    return Colors.Purple;
                case DifficultyLevel.Hard:
                    return Colors.Red;
                default:
                    return Colors.Black;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
