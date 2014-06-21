using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace BachelorProject.Helper
{
    /// <summary>
    /// Hilfsklasse Kreis, repräsentiert eine Person, die am Tisch platziert werden muss, bzw. den Tisch
    /// </summary>
    public class Circle
    {
        private readonly string _name;
        private double _radius;
        private Point _position;
        private readonly Ellipse _ellipse;
        //private int touchEpsilon = 5;
        private const int OverlapEpsilon = 10;
        private const int EnterEpsilon = 5;
        private const int LeaveEpsilon = 40;
        private static readonly HashSet<Circle> SittingPersons = new HashSet<Circle>();
        public bool IsSitter = false;
        public bool IsSittingOnSomeone = false;
        private Circle _seat; // Person, auf der dieser Kreis drauf sitzt

        public Circle(Ellipse ellipse)
        {
            _ellipse = ellipse;
            _name = ellipse.Name;
            _radius = ellipse.Width/2;
            _position = new Point(ellipse.Margin.Left + _radius, ellipse.Margin.Top + _radius);
        }

        public Point GetPosition()
        {
            return _position;
        }

        public string GetName()
        {
            return _name;
        }

        public Ellipse GetEllipse()
        {
            return _ellipse;
        }

        public double GetRadius()
        {
            return _radius;
        }

        public void UpdateRadius(double r)
        {
            _radius = r;
            _ellipse.Height = 2*r;
            _ellipse.Width = 2*r;
        }

        public void UpdatePosition(Point pos)
        {
            _position = pos;
        }

        public void SetSeat(Circle c)
        {
            _seat = c;
        }

        public void UnSetSeat(Circle c)
        {
            _seat = null;
        }

        public Circle GetSeat()
        {
            return _seat;
        }

        public bool Touches(Circle c)
        {
            var actualDist = Distance(c.GetPosition(), _position);
            var touchDist = c._radius + _radius;
            var dist = actualDist - touchDist;
            if (dist > -25 &&  dist < 20)
            {
                SittingPersons.Add(this);
                return true;
            }
            SittingPersons.Remove(this);
            return false;
        }

        public bool SitsNextTo(Circle c)
        {
            return false;
        }

        public bool Overlaps(Circle c)
        {
            return Distance(c.GetPosition(),_position) + OverlapEpsilon < (c.GetRadius() + _radius);
        }

        public bool SitsOn(Circle c)
        {
            return _seat != null && _seat.Equals(c);
        }

        public void CheckSitting(Circle c)
        {
            if (SeatLeaves(c))
                UpdateSitting(c);
            if (Enters(c))
                IsSittingOn(c);
            else if (Leaves(c))
                StopsSittingOn(c);
        }

        private bool SeatLeaves(Circle c)
        {
            return Distance(c.GetPosition(), _position) > LeaveEpsilon && Equals(c._seat);
        }

        private void UpdateSitting(Circle c)
        {
            c.UpdateRadius(50);
            Panel.SetZIndex(c.GetEllipse(), 0);
            c.IsSittingOnSomeone = false;
            c._seat = null;
            IsSitter = false;
        }
        
        private bool Enters(Circle c)
        {
            return Distance(c.GetPosition(), _position) < EnterEpsilon;
        }

        private void IsSittingOn(Circle c)
        {
            Panel.SetZIndex(_ellipse, 50);
            _ellipse.Width = 80;
            _ellipse.Height = 80;
            const int newRadius = 40;
            _ellipse.Margin = new Thickness(_ellipse.Margin.Left + _radius - newRadius, _ellipse.Margin.Top + _radius - newRadius, 0, 0);
            //position = new Point(position.X - radius + newRadius, position.Y - radius + newRadius);
            _radius = newRadius;

            c.IsSitter = true;
            IsSittingOnSomeone = true;
            _seat = c;
        }

        private bool Leaves(Circle c)
        {
            return _seat != null && Distance(c.GetPosition(), _position) > LeaveEpsilon && _seat.Equals(c);
        }

        public void StopsSittingOn(Circle c)
        {
            Panel.SetZIndex(_ellipse, 0);
            _ellipse.Width = 100;
            _ellipse.Height = 100;
            const int oldRadius = 50;
            _ellipse.Margin = new Thickness(_ellipse.Margin.Left - oldRadius + _radius, _ellipse.Margin.Top - oldRadius + _radius, 0, 0);
            //position = new Point(position.X + oldRadius - radius, position.Y + oldRadius - radius);
            _radius = oldRadius;
            c.IsSitter = false;
            IsSittingOnSomeone = false;
            _seat = null;
        }


        public double Distance(Point p1, Point p2)
        {
            return Math.Sqrt( Math.Pow(p2.X-p1.X,2) + Math.Pow(p2.Y-p1.Y,2) );
        }

    }
}
