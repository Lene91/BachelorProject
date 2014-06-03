using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eyetracker;

namespace BachelorProject
{
    class MyAOI : AreaOfInterest
    {
        public MyAOI(Circle person)
            : base(AoiType.Ellipse, getPoints(person), person.getName())
        {
        }

        public static Point[] getPoints(Circle person)
        {
            var p = person.getPosition();
            var topLeft = new Point((int)(p.X - person.getRadius()), (int)(p.Y - person.getRadius()));
            var bottomRight = new Point((int)(p.X + person.getRadius()), (int)(p.Y + person.getRadius()));
            return new Point[] {topLeft, bottomRight};
        }

        public void Set(int x, int y, double r)
        {
            var topLeft = new Point((int)(x - r), (int)(y - r));
            var bottomRight = new Point((int)(x + r), (int)(y + r));
            Points[0] = topLeft;
            Points[1] = bottomRight;
        }
    }
}
