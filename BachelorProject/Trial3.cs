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

            // Person möchte auf dem Schoß einer bestimmten anderen sitzen (erste Person sitzt auf dem Schoß
            if (sittingOn(p1,p2))
                updateConstraint("c1", true);
            else constraintsFullfilled = false;

            // Person möchte nicht auf dem Schoß einer bestimmten anderen sitzen
            if (notSittingOn(p3,p4))
                updateConstraint("c2", true);

            if (numberSittingOn(2))
                updateConstraint("c3", true);

            return constraintsFullfilled;
        }
    }
}
