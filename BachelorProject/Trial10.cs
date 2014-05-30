using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BachelorProject
{
    class Trial10 : ExampleExercise
    {
        public Trial10()
            : base()
        { id = 10; }

        public override bool checkActualConstraints()
        {
            if (numberSharingFood(3))
                updateConstraint("c1", true);
            else constraintsFullfilled = false;

            if (sittingNextToEachOther(p3,p2))
                updateConstraint("c2", true);
            else constraintsFullfilled = false;

            if (sittingNextToEachOther(p6, p5))
                updateConstraint("c3", true);
            else constraintsFullfilled = false;

            if (notSharingFood(p6, p1))
                updateConstraint("c4", true);
            else constraintsFullfilled = false;

            if (notSharingFood(p4, p1))
                updateConstraint("c5", true);
            else constraintsFullfilled = false;

            if (sittingNextToEachOther(p1, p6))
                updateConstraint("c6", true);
            else constraintsFullfilled = false;

            if (sittingNextToEachOther(p4, p5))
                updateConstraint("c7", true);
            else constraintsFullfilled = false;

            if (notSharingFood(p5, p4))
                updateConstraint("c8", true);
            else constraintsFullfilled = false;

            if (notSharingFood(p2, p4))
                updateConstraint("c9", true);
            else constraintsFullfilled = false;

            return constraintsFullfilled;
        }
    }
}


