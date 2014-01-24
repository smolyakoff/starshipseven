using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace StarshipSeven.GameInterfaces
{
    [DataContract]
    public struct Position : IEquatable<Position>
    {
        public Position(int x, int y)
        {
            X = x;
            Y = y;
        }

        [DataMember]
        public readonly int X;
        [DataMember]
        public readonly int Y;

        public override bool Equals(object obj)
        {
            if (obj is Position)
                return Equals((Position)obj);
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() * 2 + Y.GetHashCode();
        }

        public bool Equals(Position other)
        {
            return (X == other.X && Y == other.Y);
        }

        public static bool operator ==(Position pos1, Position pos2)
        {
            return pos1.Equals(pos2);
        }

        public static bool operator !=(Position pos1, Position pos2)
        {
            return !pos1.Equals(pos2);
        }
    }
}
