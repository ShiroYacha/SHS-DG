using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI;

namespace IllustrationGenerator
{
    public class Pheromone
    {
        bool isFood;
        public bool IsFood
        {
            get
            {
                return isFood;
            }
        }

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

        public Pheromone(Color initialColor, bool isFood)
        {
            this.isFood = isFood;
            this.initialColor = initialColor;
            this.initialAmount = SimulationParameters.ANT_INIT_PHEROMONE_AMOUT;
        }
    }
}
