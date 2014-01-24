using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using StarshipSeven.ViewModel.Wrappers;

namespace StarshipSeven.Presentation.Styles
{
    public class PlanetStyleSelector : StyleSelector
    {
        public Style EmptyStyle { get; set; }

        public Style WeakPlanetStyle { get; set; }

        public Style NormalPlanetStyle { get; set; }

        public Style DifficultPlanetStyle { get; set; }

        public override Style SelectStyle(object item, DependencyObject container)
        {
            var planetWrapper = (MapTile)item;
            if (planetWrapper.IsEmpty)
                return EmptyStyle;
            if (planetWrapper.Planet.KillPercentage < 0.5)
                return WeakPlanetStyle;
            else if (planetWrapper.Planet.KillPercentage < 0.7)
                return NormalPlanetStyle;
            else
                return DifficultPlanetStyle;
        }
    }
}
