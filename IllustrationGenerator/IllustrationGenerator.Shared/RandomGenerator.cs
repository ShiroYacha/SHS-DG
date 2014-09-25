using System;
using System.Collections.Generic;
using System.Text;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace IllustrationGenerator
{
    public class RandomGenerator
    {
        private Random rng = new Random();

        public Point RandomPoint(double right, double bottom)
        {
            return new Point(rng.NextDouble()*right, rng.NextDouble()*bottom);
        }

        public Color RandomColor()
        {
            return Color.FromArgb((byte)rng.Next(255), (byte)rng.Next(255), (byte)rng.Next(255), (byte)rng.Next(255));
        }

        public int RandomInt(int max, int min)
        {
            return rng.Next(min, max);
        }

        public double RandomDouble(double max, double min)
        {
            return rng.NextDouble() * (max - min) + min;
        }
        
        public bool RandomBool()
        {
            return rng.NextDouble() > 0.5;
        }
    }
}
