using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StarshipSeven.GameEntities.Factories.Helpers
{
    public struct Segment
    {
        public Segment(int left, int right)
        {
            Left = left;
            Right = right;
        }

        public readonly int Right;

        public readonly int Left;

        public int Length
        {
            get { return Right - Left; }
        }
    }
}
