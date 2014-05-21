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

            if (sittingOn(p1,p2))
                updateConstraint("c1", true);
            else constraintsFullfilled = false;
            if (notSittingOn(p3,p4))
                updateConstraint("c2", true);
            return constraintsFullfilled;
        }
    }
}
