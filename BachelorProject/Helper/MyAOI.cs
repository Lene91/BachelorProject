using System.Drawing;
using Eyetracker;

namespace BachelorProject.Helper
{
    class MyAoi : AreaOfInterest
    {
        public MyAoi(Circle person)
            : base(AoiType.Ellipse, GetPoints(person), person.GetName())
        {
        }

        public static Point[] GetPoints(Circle person)
        {
            var p = person.GetPosition();
            var topLeft = new Point((int)(p.X - person.GetRadius()), (int)(p.Y - person.GetRadius()));
            var bottomRight = new Point((int)(p.X + person.GetRadius()), (int)(p.Y + person.GetRadius()));
            return new[] {topLeft, bottomRight};
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
