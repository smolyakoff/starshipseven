using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StarshipSeven.GameInterfaces;
using System.ComponentModel;

namespace StarshipSeven.ViewModel.Wrappers
{
    public class MapTile : BaseViewModel
    {
        private IPlanet _planet;
        private bool _isSourcePlanet;
        private bool _isDestinationPlanet;

        public MapTile(IPlanet planet)
        {
            Planet = planet;
        }

        public IPlanet Planet
        {
            get { return _planet; }
            set
            {
                if (value != null)
                    value.PropertyChanged += OnPlanetChanged;
                SetProperty<IPlanet>(ref _planet, value, "Planet");
                OnPropertyChanged("IsEmpty");
            }
        }

        public void OnPlanetChanged(object sender, PropertyChangedEventArgs args)
        {
            OnPropertyChanged("Planet");
        }

        public bool IsSourcePlanet
        {
            get { return _isSourcePlanet; }
            set 
            { 
                SetProperty<bool>(ref _isSourcePlanet, value, "IsSourcePlanet");
                if (IsSourcePlanet && IsDestinationPlanet)
                {
                    _isDestinationPlanet = false;
                    OnPropertyChanged("IsDestinationPlanet");
                }
            }
        }

        public bool IsDestinationPlanet
        {
            get { return _isDestinationPlanet; }
            set
            {
                SetProperty<bool>(ref _isDestinationPlanet, value, "IsDestinationPlanet");
                if (IsSourcePlanet && IsDestinationPlanet)
                {
                    _isSourcePlanet = false;
                    OnPropertyChanged("IsSourcePlanet");
                }
            }
        }

        public bool IsEmpty
        {
            get { return Planet == null; }
        }

        public bool HasPlanet
        {
            get { return !IsEmpty; }
        }
    }
}
