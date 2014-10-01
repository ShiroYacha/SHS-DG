using System;
using System.Collections.Generic;
using System.Text;
using Windows.Foundation;

namespace IllustrationGenerator
{
    public class JustantAnt:Ant
    {
        bool returnMode = false;
        public JustantAnt(City city, Map map) : base(city, map) { }

        public override void Move()
        {
            var oldPosition = position;
            var testNextPosition = position;
            // Compute move

            var motive = rng.RandomDouble(1.0, 0.0);
            if (motive <= SimulationParameters.JUSTANT_ANT_EXPLORE_EXPLOIT_RATIO)
            {
                // Explore
                var deltaX = rng.RandomDouble(antStep, -antStep);
                var deltaY = rng.RandomDouble(antStep, -antStep);
                testNextPosition.X += deltaX;
                testNextPosition.Y += deltaY;
            }
            else
            {
                // Exploit
                if (returnMode)
                    testNextPosition = map.GetReturnCityDirection(position,city);
                else
                    testNextPosition = map.GetNearbyPheromoneDirection(position);
            }
            
            // Check if is food
            bool isFood = map.CheckIsNearFood(testNextPosition);
            if (!returnMode && isFood)
                returnMode = true;
            // Leave pheromone on map
            if (pheromoneSupply > 0 && map.CheckBoundary(testNextPosition))
            {
                position.X = testNextPosition.X;
                position.Y = testNextPosition.Y;
                var newPheromone = returnMode ? new Pheromone(FOOD_PHEROMONE_COLOR, true) : new Pheromone(city.Color, false);
                map.DropPheromoneOnMap(newPheromone, position);
                map.ConnectPheromoneOnMap(newPheromone, oldPosition, position);
                pheromoneSupply--;
            }
        }

    }
}
