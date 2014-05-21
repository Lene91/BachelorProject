using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;

namespace BachelorProject
{
    class Trial3 : ExampleExercise
    {
        public Trial3()
            : base()
        { }

        public override bool checkActualConstraints()
        {

            if (p1.sitsOn(p2))
                updateConstraint("c1", true);
            else constraintsFullfilled = false;
            if (!p3.sitsOn(p4))
                updateConstraint("c2", true);
            return constraintsFullfilled;
        }
    }
}
