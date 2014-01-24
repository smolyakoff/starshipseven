using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using StarshipSeven.ViewModel.EventsModel;

namespace StarshipSeven.Presentation.Styles
{
    public class EventStyleSelector : StyleSelector
    {

        public Style HighPriorityEventStyle { get; set; }
        public Style MediumPriorityEventStyle { get; set; }
        public Style LowPriorityEventStyle { get; set; }


        public override Style SelectStyle(object item, DependencyObject container)
        {
            var e = (IGameEvent)item;
            if (e.Importance <= 1)
                return HighPriorityEventStyle;
            else if (e.Importance <= 2)
                return MediumPriorityEventStyle;
            else
                return LowPriorityEventStyle;
        }
    }
}
