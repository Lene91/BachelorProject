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
    public class Circle
    {
        private string name;
        private double radius;
        private Point position;
        private Ellipse ellipse;
        private int touchEpsilon = 5;
        private int overlapEpsilon = 20;
        private int enterEpsilon = 5;
        private int leaveEpsilon = 40;
        private static HashSet<Circle> sittingPersons = new HashSet<Circle>();
        public bool isSitter = false;
        public bool isSittingOnSomeone = false;
        private Circle seat = null; // Person, auf der man drauf sitzt

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

        public Ellipse getEllipse()
        {
            return ellipse;
        }

        public double getRadius()
        {
            return radius;
        }

        public void updateRadius(double r)
        {
            this.radius = r;
        }

        public void updatePosition(Point pos)
        {
            this.position = pos;
        }

        public void setSeat(Circle c)
        {
            this.seat = c;
        }

        public void unSetSeat(Circle c)
        {
            this.seat = null;
        }

        public Circle getSeat()
        {
            return seat;
        }

        public bool touches(Circle c)
        {
            var actualDist = distance(c.getPosition(), position);
            var touchDist = c.radius + radius;
            //var dist = Math.Max(actualDist, touchDist) - Math.Min(actualDist, touchDist);
            var dist = actualDist - touchDist;
            if (dist > - 5 &&  dist < 10)
            {
                sittingPersons.Add(this);
                return true;
            }
            sittingPersons.Remove(this);
            return false;
        }

        public bool sitsNextTo(Circle c)
        {
            return false;
        }

        public bool overlaps(Circle c)
        {
            if (distance(c.getPosition(),position) + overlapEpsilon < (c.getRadius() + radius))
                return true;
            return false;
        }

        public bool sitsOn(Circle c)
        {
            if (seat != null && seat.Equals(c))
                return true;
            return false;
        }

        public void checkSitting(Circle c)
        {
            if (enters(c))
                isSittingOn(c);
            else if (leaves(c))
                stopsSittingOn(c);
        }

        private bool enters(Circle c)
        {
            if (distance(c.getPosition(), position) < enterEpsilon)
                return true;
            return false;
        }

        private void isSittingOn(Circle c)
        {
            Canvas.SetZIndex(ellipse, 50);
            ellipse.Width = 80;
            ellipse.Height = 80;
            radius = 40;
            c.isSitter = true;
            isSittingOnSomeone = true;
            seat = c;
        }

        private bool leaves(Circle c)
        {
            if (distance(c.getPosition(), position) > leaveEpsilon && c.isSitter)
                return true;
            return false;
        }

        private void stopsSittingOn(Circle c)
        {
            Canvas.SetZIndex(ellipse, 0);
            ellipse.Width = 100;
            ellipse.Height = 100;
            radius = 50;
            c.isSitter = false;
            isSittingOnSomeone = false;
            seat = null;
        }


        public double distance(Point p1, Point p2)
        {
            return Math.Sqrt( Math.Pow(p2.X-p1.X,2) + Math.Pow(p2.Y-p1.Y,2) );
        }

    }
}
