using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BachelorProject
{
    class Trial4 : ExampleExercise
    {
        public Trial4()
            : base()
        { id = 4; }

        public override bool checkActualConstraints()
        {
            if (sittingNextToEachOther(p2, p3))
                updateConstraint("c1", true);
            else constraintsFullfilled = false;

            if (notSittingNextToEachOther(p1, p4))
                updateConstraint("c2", true);
            else constraintsFullfilled = false;

            if (oneNeighbourSharingFood(p1))
                updateConstraint("c3", true);
            else constraintsFullfilled = false;

            if (oneNeighbourIsSeat(p5))
                updateConstraint("c4", true);
            else constraintsFullfilled = false;

            if (sittingNextToEachOther(p3, p5))
                updateConstraint("c5", true);
            else constraintsFullfilled = false;

            if (oneNeighbourSharingFood(p5))
                updateConstraint("c6", true);
            else constraintsFullfilled = false;

            if (notSittingNextToEachOther(p2, p5))
                updateConstraint("c7", true);
            else constraintsFullfilled = false;

            if (sittingOnSomeone(p4))
                updateConstraint("c8", true);
            else constraintsFullfilled = false;

            if (numberSharingFood(1))
                updateConstraint("c9", true);
            else constraintsFullfilled = false;

            /*
            if (sittingNextToEachOther(p3, p2))
                updateConstraint("c1", true);
            else constraintsFullfilled = false;

            if (sittingNextToEachOther(p4, p2))
                updateConstraint("c2", true);
            else constraintsFullfilled = false;

            if (sittingNextToEachOther(p1, p3))
                updateConstraint("c3", true);
            else constraintsFullfilled = false;

            if (sittingNextToEachOther(p5, p4))
                updateConstraint("c4", true);
            else constraintsFullfilled = false;

            if (numberSittingOn(1))
                updateConstraint("c5", true);
            else constraintsFullfilled = false;
            */

            return constraintsFullfilled;
        }
    }
}
