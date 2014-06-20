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
        private int overlapEpsilon = 10;
        private int enterEpsilon = 5;
        private int leaveEpsilon = 40;
        private static HashSet<Circle> sittingPersons = new HashSet<Circle>();
        public bool isSitter = false;
        public bool isSittingOnSomeone = false;
        private Circle seat = null; // Person, auf der dieser Kreis drauf sitzt

        public Circle(Ellipse ellipse,int id)
        {
            this.ellipse = ellipse;
            this.name = ellipse.Name;
            this.radius = ellipse.Width/2;
            this.position = new Point(ellipse.Margin.Left + radius, ellipse.Margin.Top + radius);
            id++;
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
            ellipse.Height = 2*r;
            ellipse.Width = 2*r;
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
            var dist = actualDist - touchDist;
            if (dist > - 15 &&  dist < 10)
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
            if (seatLeaves(c))
                updateSitting(c);
            if (enters(c))
                isSittingOn(c);
            else if (leaves(c))
                stopsSittingOn(c);
        }

        private bool seatLeaves(Circle c)
        {
            if (distance(c.getPosition(), position) > leaveEpsilon && this.Equals(c.seat))
                return true;
            return false;
        }

        private void updateSitting(Circle c)
        {
            c.updateRadius(50);
            Canvas.SetZIndex(c.getEllipse(), 0);
            c.isSittingOnSomeone = false;
            c.seat = null;
            this.isSitter = false;
        }
        
        private bool enters(Circle c)
        {
            if (distance(c.getPosition(), position) < enterEpsilon) // || c.getRadius() > radius)
                return true;
            return false;
        }

        private void isSittingOn(Circle c)
        {
            Canvas.SetZIndex(ellipse, 50);
            ellipse.Width = 80;
            ellipse.Height = 80;
            var newRadius = 40;
            ellipse.Margin = new Thickness(ellipse.Margin.Left + radius - newRadius, ellipse.Margin.Top + radius - newRadius, 0, 0);
            //position = new Point(position.X - radius + newRadius, position.Y - radius + newRadius);
            radius = newRadius;

            c.isSitter = true;
            this.isSittingOnSomeone = true;
            seat = c;
        }

        private bool leaves(Circle c)
        {
            if (seat != null && distance(c.getPosition(), position) > leaveEpsilon && seat.Equals(c))
                return true;
            return false;
        }

        public void stopsSittingOn(Circle c)
        {
            Canvas.SetZIndex(ellipse, 0);
            ellipse.Width = 100;
            ellipse.Height = 100;
            var oldRadius = 50;
            ellipse.Margin = new Thickness(ellipse.Margin.Left - oldRadius + radius, ellipse.Margin.Top - oldRadius + radius, 0, 0);
            //position = new Point(position.X + oldRadius - radius, position.Y + oldRadius - radius);
            radius = oldRadius;
            c.isSitter = false;
            this.isSittingOnSomeone = false;
            seat = null;
        }


        public double distance(Point p1, Point p2)
        {
            return Math.Sqrt( Math.Pow(p2.X-p1.X,2) + Math.Pow(p2.Y-p1.Y,2) );
        }

    }
}
