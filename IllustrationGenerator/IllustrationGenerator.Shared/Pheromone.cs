using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI;

namespace IllustrationGenerator
{
    public class Pheromone
    {
        public Color Color
        {
            get
            {
                return initialColor;
            }
        }

        public double Amount
        {
            get
            {
                initialAmount-=SimulationParameters.ANT_PHEROMONE_EVAPORATE_AMOUT;
                return initialAmount;
            }
        }

        private Color initialColor;
        private double initialAmount;

        public Pheromone(Color initialColor)
        {
            this.initialColor = initialColor;
            this.initialAmount = SimulationParameters.ANT_INIT_PHEROMONE_AMOUT;
        }
    }
}
