using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StarshipSeven.GameInterfaces;

namespace StarshipSeven.ViewModel.EventsModel
{
    public abstract class BaseEvent<TArgs> : IGameEvent where TArgs : EventArgs
    {
        protected TArgs _eventArgs;

        public BaseEvent(TArgs args)
        {
            _eventArgs = args;
        }

        public abstract string Message { get; }

        public abstract IEnumerable<IPlayer> RelatesTo { get; }

        public abstract int Importance { get; }

        protected IEnumerable<T> AsEnumerable<T>(params T[] objects)
        {
            return new List<T>(objects);
        }

    }
}
