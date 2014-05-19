using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace BachelorProject
{
    class Trial1 : ExampleExercise
    {
        public Trial1()
            : base()
        { }

        public override bool checkActualConstraints()
        {

            if (p3.touches(table))
                getConstraint("c1").Background = Brushes.Green;
            else constraintsFullfilled = false;
            return constraintsFullfilled;
        }
    }
}
