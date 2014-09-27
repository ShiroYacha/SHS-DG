using System;
using System.Collections.Generic;
using System.Text;
using Windows.Foundation;
using Windows.UI;

namespace IllustrationGenerator
{
    public class Ant
    {
        private Color FOOD_PHEROMONE_COLOR = Colors.White;

        private City city;
        private Point position;
        private Map map;

        private int pheromoneSupply;

        private double antStep;

        private RandomGenerator rng = new RandomGenerator();
    
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

        public void RandomMove()
        {
            var oldPosition = position;
            // Compute move
            var deltaX = rng.RandomDouble(antStep, -antStep);
            var deltaY = rng.RandomDouble(antStep, -antStep);
            if (position.X + deltaX >= 0 && position.X + deltaX < map.Width)
                position.X += deltaX;
            if (position.Y + deltaY >= 0 && position.Y + deltaY < map.Height)
                position.Y += deltaY;
            // Paint pheromone on map
            if (pheromoneSupply > 0)
            {
                var newPheromone =new Pheromone(city.Color);
                map.DropPheromoneOnMap(newPheromone, position);
                map.ConnectPheromoneOnMap(newPheromone, oldPosition, position);
                pheromoneSupply--;
            }
        }

        
    }
}
