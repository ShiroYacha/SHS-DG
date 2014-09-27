using System;
using System.Collections.Generic;
using System.Text;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace IllustrationGenerator
{
    public class Painter
    {
        #region Private fields
        private Canvas canvas;
        private RandomGenerator rng = new RandomGenerator();
        private List<Point> anchors = new List<Point>(); 
        #endregion

        #region Constructors
        public Painter(Canvas canvas)
        {
            this.canvas = canvas;
        }
        #endregion

        #region Helper methods
        private int ScaleCanvas(double ratio)
        {
            double length = Math.Min(canvas.Width, canvas.Height);
            return (int)Math.Round(length * ratio);
        }     
        #endregion

        #region Paint anchors
        public void GenerateAnchors(int count)
        {
            for (int i = 0; i < count; i++)
            {
                var anchor = rng.RandomPoint(canvas.Width, canvas.Height);
                anchors.Add(anchor);
                DrawEllipseOnCanvas(anchor);
            }
        }

        private void DrawEllipseOnCanvas(Point point)
        {
            Ellipse ellipse = new Ellipse();
            int diameter = ScaleCanvas(rng.RandomDouble(PaintParameters.PHEOMONE_ELLIPSE_CANVAS_MAX_RATIO, PaintParameters.PHEOMONE_ELLIPSE_CANVAS_MIN_RATIO));
            ellipse.Width = diameter;
            ellipse.Height = diameter;
            var brushColor = rng.RandomColor();
            var fillBrush = new SolidColorBrush(brushColor);
            var strokeBrush = new SolidColorBrush(brushColor);
            strokeBrush.Opacity = 0.4;
            ellipse.Fill = fillBrush;
            ellipse.Stroke = strokeBrush;
            ellipse.StrokeThickness = PaintParameters.ELLIPSE_STROKE_THICKNESS;
            canvas.Children.Add(ellipse);
            Canvas.SetLeft(ellipse, point.X);
            Canvas.SetTop(ellipse, point.Y);
        }    
        #endregion

        #region Connect anchors
        public void ConnectAnchors(int count)
        {
            for (int i = 0; i < count; i++)
            {
                var anchors = PickAnchors();
                var start = anchors.Item1;
                var end = anchors.Item2;
                CreateArcSegment(start, end);
            }
        }

        private void CreateArcSegment(Point start, Point end)
        {
            PathGeometry pathGeometry = new PathGeometry();
            PathFigure pathFigure = new PathFigure();
            pathFigure.StartPoint = start;
            var arc = new ArcSegment();
            arc.Point = end;
            arc.Size = new Size(rng.RandomInt(50, 10), rng.RandomInt(50, 10));
            arc.IsLargeArc = rng.RandomBool();
            arc.SweepDirection = rng.RandomBool() ? SweepDirection.Clockwise : SweepDirection.Counterclockwise;
            pathFigure.Segments.Add(arc);
            pathGeometry.Figures.Add(pathFigure);
            Path path = new Path();
            path.Stroke = new SolidColorBrush(rng.RandomColor());
            path.StrokeThickness = rng.RandomInt(10,5);
            path.Data = pathGeometry;
            canvas.Children.Add(path);
        }

        private Tuple<Point,Point> PickAnchors()
        {
            int first = rng.RandomInt(anchors.Count-1, 0);
            int second = rng.RandomInt(anchors.Count - 1, 0);
            while(second == first)
                second = rng.RandomInt(anchors.Count - 1, 0);
            return new Tuple<Point, Point>(anchors[first], anchors[second]);
        }
        #endregion

    }
}
