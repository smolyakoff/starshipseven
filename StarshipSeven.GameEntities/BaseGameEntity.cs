using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using StarshipSeven.GameInterfaces;

namespace StarshipSeven.GameEntities
{
    public class BaseGameEntity<TKey> : IGameEntity, IEquatable<BaseGameEntity<TKey>>
    {
        private TKey _id;

        public BaseGameEntity()
        {
           
        }

        public BaseGameEntity(TKey id)
        {
            Id = id;
        }

        public TKey Id
        {
            get { return _id; }
            set { SetProperty<TKey>(ref _id, value, "Id"); }
        }

        protected void SetProperty<T>(ref T field, T value, string propertyName)
        {
            if (!EqualityComparer<T>.Default.Equals(field, value))
            {
                field = value;
                OnPropertyChanged(propertyName);
            }
        }

        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public bool Equals(BaseGameEntity<TKey> other)
        {
            if (other == null)
                return false;
            return EqualityComparer<TKey>.Default.Equals(this.Id, other.Id);
        }
    }
}
