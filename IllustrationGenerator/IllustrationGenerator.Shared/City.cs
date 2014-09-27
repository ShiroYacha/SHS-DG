using System;
using System.Collections.Generic;
using System.Text;
using Windows.Foundation;
using Windows.UI;

namespace IllustrationGenerator
{
    public class City
    {
        public Point Position
        {
            get;
            set;
        }

        public Color Color
        {
            get;
            set;
        }

        public double DistanceFrom(City anotherCity)
        {
            var to=anotherCity.Position;
            var from = this.Position;
            return Math.Sqrt(Math.Pow((to.X - from.X), 2) + Math.Pow((to.Y - from.Y), 2));
        }
    }
}
