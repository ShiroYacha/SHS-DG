using System;
using System.Collections.Generic;
using System.Text;
using Windows.Foundation;
using Windows.UI;

namespace IllustrationGenerator
{
    public abstract class Ant
    {
        protected Color FOOD_PHEROMONE_COLOR = Colors.White;

        protected City city;
        protected Point position;
        protected Map map;

        protected int pheromoneSupply;

        protected double antStep;

        protected RandomGenerator rng = new RandomGenerator();
    
        public Point Position
        {
            get
            {
                return position;
            }
        }

        public Ant(City city, Map map)
        {
            this.position = city.Position;
            this.map = map;
            this.city = city;
            this.antStep = map.Diag * SimulationParameters.ANT_STEP_SIZE_RATIO;
            this.pheromoneSupply = SimulationParameters.ANT_PHEROMONE_SUPPLY;
        }

        public abstract void Move();

        public bool CheckPheromone()
        {
            return pheromoneSupply > 0;
        }
    }
}
