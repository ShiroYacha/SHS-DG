using System;
using System.Collections.Generic;
using System.Text;

namespace IllustrationGenerator
{
    public static class SimulationParameters
    {
        public static int CITY_COUNT = 10;
        public static double ANT_STEP_SIZE_RATIO = 0.06;
        public static double ANT_INIT_PHEROMONE_AMOUT = 1;
        public static double ANT_PHEROMONE_EVAPORATE_AMOUT = 0.05;
        public static int ANT_PHEROMONE_SUPPLY = 50;
        public static double JUSTANT_ANT_EXPLORE_EXPLOIT_RATIO = 0.5;
    }
}
