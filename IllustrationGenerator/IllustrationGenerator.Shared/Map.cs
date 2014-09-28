using System;
using System.Collections.Generic;
using System.Text;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace IllustrationGenerator
{
    public class Map
    {
        private RandomGenerator rng = new RandomGenerator();

        private double height;
        private double width;
        private double diag;

        private Canvas canvas;

        private List<City> cities;
        private double[][] distances;

        private Dictionary<Pheromone, Ellipse> pheromones;
        private Dictionary<Pheromone, Path> paths;

        #region Public properties
        public List<City> Cities
        {
            get
            {
                return cities;
            }
        }

        public double Height
        {
            get
            {
                return height;
            }
        }

        public double Width
        {
            get
            {
                return width;
            }
        }

        public double Diag
        {
            get
            {
                return diag;
            }
        } 

        public int PheromeneCount
        {
            get
            {
                return pheromones.Keys.Count;
            }
        }
        #endregion

        public Map(Canvas canvas, int numCities)
        {
            // Initialization
            pheromones = new Dictionary<Pheromone, Ellipse>();
            paths = new Dictionary<Pheromone, Path>();
            // Affectation
            this.canvas = canvas;
            this.width = canvas.Width;
            this.height = canvas.Height;
            this.diag = Math.Sqrt(Math.Pow(width, 2) + Math.Pow(height, 2));
            // Data generation
            GenerateCities(numCities);
            GenerateFood();
            // Data computation
            ComputeDistance();

        }

        private void GenerateCities(int numCities)
        {
            cities = new List<City>();
            for (int i = 0; i < numCities; i++)
            {
                var position = rng.RandomPoint(width, height);
                var color = rng.RandomColor();
                cities.Add(new City() { Position = position, Color = color});
                AddEllipse(position, color, PaintParameters.CITY_ELLIPSE_CANVAS_RATIO * diag);
            }
        }

        private void GenerateFood()
        {
            AddEllipse(new Point(canvas.Width/2,canvas.Height/2), Colors.White, PaintParameters.CITY_ELLIPSE_CANVAS_RATIO * diag);
        }

        private void ComputeDistance()
        {
            var numCities = cities.Count;
            distances = new double[numCities][];
            for (int i = 0; i < distances.Length; ++i)
                distances[i] = new double[numCities];
            for (int i = 0; i < numCities; ++i)
                for (int j = i + 1; j < numCities; ++j)
                {
                    var distance = cities[i].DistanceFrom(cities[j]);
                    distances[i][j] = distance;
                    distances[j][i] = distance;
                }
        }

        public void RefreshPheromones()
        {
            List<Pheromone> shadowPheromones = new List<Pheromone>();
            foreach(var pheromone in pheromones.Keys)
            {
                pheromones[pheromone].Stroke = new SolidColorBrush(pheromone.Color);
                if (pheromone.Amount <= 0)
                    shadowPheromones.Add(pheromone);
                else
                {
                    pheromones[pheromone].Opacity = pheromone.Amount;
                    paths[pheromone].Opacity = pheromone.Amount;
                }
            }
            foreach (var pheromone in shadowPheromones)
            {
                canvas.Children.Remove(pheromones[pheromone]);
                canvas.Children.Remove(paths[pheromone]);
                pheromones.Remove(pheromone);
                paths.Remove(pheromone);
            }
        }

        public bool CheckBoundary(Point testPosition)
        {
            var testX = testPosition.X - canvas.Width / 2.0;
            var testY = testPosition.Y - canvas.Height / 2.0;
            return Math.Pow(testX, 2) + Math.Pow(testY, 2) <= Math.Pow(diag, 2); 
        }

        private int ScaleCanvas(double ratio)
        {
            double length = Math.Min(canvas.Width, canvas.Height);
            return (int)Math.Round(length * ratio);
        }  

        public void DropPheromoneOnMap(Pheromone pheromone, Point position)
        {
            double diameter = rng.RandomDouble(PaintParameters.PHEOMONE_ELLIPSE_CANVAS_MAX_RATIO * diag, PaintParameters.PHEOMONE_ELLIPSE_CANVAS_MIN_RATIO * diag);
            var ellipse = AddEllipse(position, pheromone.Color, diameter);
            pheromones.Add(pheromone, ellipse);
        }    

        public Point GetReturnCityDirection(Point position, City city)
        {
            var step = diag * SimulationParameters.ANT_STEP_SIZE_RATIO;
            var deltaX = city.Position.X - position.X;
            var deltaY = city.Position.Y - position.Y;
            var deltaDiag = Math.Sqrt(Math.Pow(deltaX, 2) + Math.Pow(deltaY, 2));
            return new Point(position.X + deltaX * step / deltaDiag, position.Y + deltaY * step / deltaDiag);
        }

        public Point GetNearbyPheromoneDirection(Point position)
        {
            var step = diag * SimulationParameters.ANT_STEP_SIZE_RATIO;
            foreach(var pheromone in pheromones.Keys)
            {
                var ellipse = pheromones[pheromone];
                double x = Canvas.GetLeft(ellipse);
                double y = Canvas.GetTop(ellipse);
                if ((Math.Pow(x - position.X, 2) + Math.Pow(y - position.Y, 2)) <= step)
                    return new Point(x, y);
            }
            var deltaX = rng.RandomDouble(step, -step);
            var deltaY = rng.RandomDouble(step, -step);
            position.X += deltaX;
            position.Y += deltaY;
            return position;
        }

        public bool CheckIsNearFood(Point position)
        {
            var step = diag * SimulationParameters.ANT_STEP_SIZE_RATIO;
            double x = canvas.Width/2;
            double y = canvas.Height/2;
            return Math.Sqrt(Math.Pow(x - position.X, 2) + Math.Pow(y - position.Y, 2)) <= step;
        }

        public void ConnectPheromoneOnMap(Pheromone toPheromone,Point from, Point to)
        {
            var path = CreateSegment(from, to, toPheromone.Color, toPheromone.IsFood ? 
                rng.RandomDouble(PaintParameters.PATH_STROKE_THICKNESS_FOOD_MAX_RATIO * diag, PaintParameters.PATH_STROKE_THICKNESS_FOOD_MIN_RATIO * diag) 
                : rng.RandomDouble(PaintParameters.PATH_STROKE_THICKNESS_MAX_RATIO * diag, PaintParameters.PATH_STROKE_THICKNESS_MIN_RATIO * diag));
            paths.Add(toPheromone, path);
        }

        private Ellipse AddEllipse(Point position, Color color, double diameter)
        {
            Ellipse ellipse = new Ellipse();
            ellipse.Width = diameter;
            ellipse.Height = diameter;
            var fillBrush = new SolidColorBrush(color);
            var strokeBrush = new SolidColorBrush(color);
            strokeBrush.Opacity = 0.4;
            ellipse.Fill = fillBrush;
            ellipse.Stroke = strokeBrush;
            ellipse.StrokeThickness = PaintParameters.ELLIPSE_STROKE_THICKNESS_RATIO * diag;
            canvas.Children.Add(ellipse);
            Canvas.SetLeft(ellipse, position.X - diameter/2);
            Canvas.SetTop(ellipse, position.Y - diameter / 2);
            return ellipse;
        }

        private Path CreateArcSegment(Point start, Point end, Color color, double thickness)
        {
            PathGeometry pathGeometry = new PathGeometry();
            PathFigure pathFigure = new PathFigure();
            pathFigure.StartPoint = start;
            var arc = new ArcSegment();
            arc.Point = end;
            var size =rng.RandomDouble(5.5, 0.5);
            arc.Size = new Size(size, size);
            arc.IsLargeArc = rng.RandomBool();
            arc.SweepDirection = rng.RandomBool() ? SweepDirection.Clockwise : SweepDirection.Counterclockwise;
            pathFigure.Segments.Add(arc);
            pathGeometry.Figures.Add(pathFigure);
            Path path = new Path();
            path.Stroke = new SolidColorBrush(color);
            path.StrokeThickness = thickness;
            path.Data = pathGeometry;
            canvas.Children.Add(path);
            return path;
        }

        private Path CreateSegment(Point start, Point end, Color color, double thickness)
        {
            PathGeometry pathGeometry = new PathGeometry();
            PathFigure pathFigure = new PathFigure();
            pathFigure.StartPoint = start;
            var line = new LineSegment();
            line.Point = end;
            pathFigure.Segments.Add(line);
            pathGeometry.Figures.Add(pathFigure);
            Path path = new Path();
            path.StrokeThickness = thickness;
            path.Stroke = new SolidColorBrush(color);
            path.Data = pathGeometry;
            canvas.Children.Add(path);
            return path;
        }
    }
}
