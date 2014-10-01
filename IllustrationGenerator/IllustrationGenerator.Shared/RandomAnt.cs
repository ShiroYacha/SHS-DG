using System;
using System.Collections.Generic;
using System.Text;

namespace IllustrationGenerator
{
    public class RandomAnt:Ant
    {
        public RandomAnt(City city, Map map) : base(city, map) { }

        public override void Move()
        {
            var oldPosition = position;
            var testNextPosition = position;
            // Compute move
            var deltaX = rng.RandomDouble(antStep, -antStep);
            var deltaY = rng.RandomDouble(antStep, -antStep);
            testNextPosition.X += deltaX;
            testNextPosition.Y += deltaY;
            // Leave pheromone on map
            if (pheromoneSupply > 0 && map.CheckBoundary(testNextPosition))
            {
                position.X = testNextPosition.X;
                position.Y = testNextPosition.Y;
                var newPheromone = new Pheromone(city.Color,false);
                map.DropPheromoneOnMap(newPheromone, position);
                map.ConnectPheromoneOnMap(newPheromone, oldPosition, position);
                pheromoneSupply--;
            }
        }

    }
}
