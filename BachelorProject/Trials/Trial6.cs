using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BachelorProject
{
    class Trial6 : ExampleExercise
    {
        public Trial6()
            : base()
        { id = 6; }

        public override bool checkActualConstraints()
        {
            if (notSittingOnSomeone(p1))
                updateConstraint("c1", true);
            else constraintsFullfilled = false;

            if (isSeat(p5))
                updateConstraint("c2", true);
            else constraintsFullfilled = false;

            if (noNeighbourIsSeat(p2))
                updateConstraint("c3", true);
            else constraintsFullfilled = false;

            if (notSharingFood(p1))
                updateConstraint("c4", true);
            else constraintsFullfilled = false;

            if (sharingFood(p3))
                updateConstraint("c5", true);
            else constraintsFullfilled = false;

            /*
            if (sittingNextToEachOther(p2, p4))
                updateConstraint("c1", true);
            else constraintsFullfilled = false;

            if (isSeat(p3))
                updateConstraint("c2", true);
            else constraintsFullfilled = false;

            if (sittingNextToEachOther(p1, p5))
                updateConstraint("c3", true);
            else constraintsFullfilled = false;

            if (sittingNextToEachOther(p5, p4))
                updateConstraint("c4", true);
            else constraintsFullfilled = false;

            if (sittingNextToEachOther(p1, p3))
                updateConstraint("c5", true);
            else constraintsFullfilled = false;

            if (sittingNextToEachOther(p2, p3))
                updateConstraint("c6", true);
            else constraintsFullfilled = false;

            if (sittingOnSomeone(p2))
                updateConstraint("c7", true);
            else constraintsFullfilled = false;

            if (notSittingOnSomeone(p5))
                updateConstraint("c8", true);
            else constraintsFullfilled = false;
            */

            return constraintsFullfilled;
        }
    }
}

