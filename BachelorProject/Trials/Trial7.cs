using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BachelorProject
{
    class Trial7 : ExampleExercise
    {
        public Trial7()
            : base()
        { id = 7; }

        public override bool checkActualConstraints()
        {
            if (sharingFood(p2))
                updateConstraint("c1", true);
            else constraintsFullfilled = false;

            if (sittingOnSomeone(p4))
                updateConstraint("c2", true);
            else constraintsFullfilled = false;

            if (oneNeighbourSharingFood(p1))
                updateConstraint("c3", true);
            else constraintsFullfilled = false;

            if (oneNeighbourIsSeat(p3))
                updateConstraint("c4", true);
            else constraintsFullfilled = false;

            if (oneNeighbourIsSeat(p1))
                updateConstraint("c5", true);
            else constraintsFullfilled = false;

            /*
            if (sittingNextToEachOther(p2, p1))
                updateConstraint("c1", true);
            else constraintsFullfilled = false;

            if (atLeastOneNeighbourSharingFood(p3))
                updateConstraint("c2", true);
            else constraintsFullfilled = false;

            if (noNeighbourSharingFood(p5))
                updateConstraint("c3", true);
            else constraintsFullfilled = false;

            if (sittingNextToEachOther(p6, p1))
                updateConstraint("c4", true);
            else constraintsFullfilled = false;

            if (notSittingNextToEachOther(p3, p5))
                updateConstraint("c5", true);
            else constraintsFullfilled = false;

            if (sittingNextToEachOther(p5, p6))
                updateConstraint("c6", true);
            else constraintsFullfilled = false;
            */

            return constraintsFullfilled;
        }
    }
}

