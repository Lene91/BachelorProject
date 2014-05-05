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
    /// 
    /// </summary>
    class Circle
    {
        private string name;
        private double radius;
        private Point position;
        private Ellipse ellipse;
        private int epsilon = 20;

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
            if (dist < epsilon)
                return true;
            return false;
        }

        private double distance(Point p1, Point p2)
        {
            return Math.Sqrt( Math.Pow(p1.X-p2.X,2) + Math.Pow(p1.Y-p2.Y,2) );
        }

    }
}
