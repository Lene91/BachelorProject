using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Diagnostics;

namespace BachelorProject
{
    /// <summary>
    /// Hilfsklasse Kreis, repräsentiert eine Person, die am Tisch platziert werden muss, bzw. den Tisch
    /// </summary>
    class Circle
    {
        private string name;
        private double radius;
        private Point position;
        private Ellipse ellipse;
        private int epsilon = 5;

        public Circle(Ellipse ellipse)
        {
            this.ellipse = ellipse;
            this.name = ellipse.Name;
            this.radius = ellipse.Width/2;
            this.position = new Point(ellipse.Margin.Left + radius, ellipse.Margin.Top + radius);
        }

        public Point getPosition()
        {
            return position;
        }

        public string getName()
        {
            return name;
        }

        public void updatePosition(Point pos)
        {
            this.position = pos;
        }

        public bool touches(Circle c)
        {
            var actualDist = distance(c.getPosition(), position);
            var touchDist = c.radius + radius;
            var dist = Math.Max(actualDist, touchDist) - Math.Min(actualDist, touchDist);
            if( dist < epsilon)
                return true;
            return false;
        }

        public bool overlaps(Circle c)
        {
            if (distance(c.getPosition(),position) < (c.radius + radius))
                return true;
            return false;
        }

        private double distance(Point p1, Point p2)
        {
            return Math.Sqrt( Math.Pow(p2.X-p1.X,2) + Math.Pow(p2.Y-p1.Y,2) );
        }

    }
}
