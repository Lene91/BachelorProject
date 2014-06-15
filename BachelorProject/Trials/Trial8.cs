using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BachelorProject
{
    class Trial8 : ExampleExercise
    {
        public Trial8()
            : base()
        { id = 8; }

        public override bool checkActualConstraints()
        {
            if (sittingNextToEachOther(p2, p1))
                updateConstraint("c1", true);
            else constraintsFullfilled = false;

            if (notSittingNextToEachOther(p3, p1))
                updateConstraint("c2", true);
            else constraintsFullfilled = false;

            if (oneNeighbourSharingFood(p5))
                updateConstraint("c3", true);
            else constraintsFullfilled = false;

            if (oneNeighbourIsSeat(p1))
                updateConstraint("c4", true);
            else constraintsFullfilled = false;

            if (notSittingNextToEachOther(p2,p4))
                updateConstraint("c5", true);
            else constraintsFullfilled = false;

            if (notSharingFood(p1))
                updateConstraint("c6", true);
            else constraintsFullfilled = false;

            if (notSittingOnSomeone(p3))
                updateConstraint("c7", true);
            else constraintsFullfilled = false;

            if (isSeat(p5))
                updateConstraint("c8", true);
            else constraintsFullfilled = false;


            /*
            if (sittingNextToEachOther(p2, p4))
                updateConstraint("c1", true);
            else constraintsFullfilled = false;

            if (notSittingNextToEachOther(p1, p2))
                updateConstraint("c2", true);
            else constraintsFullfilled = false;

            if (notSittingNextToEachOther(p3, p4))
                updateConstraint("c3", true);
            else constraintsFullfilled = false;

            if (notSittingOnSomeone(p3))
                updateConstraint("c4", true);
            else constraintsFullfilled = false;

            if (isNotSeat(p2))
                updateConstraint("c5", true);
            else constraintsFullfilled = false;
            */

            return constraintsFullfilled;
        }
    }
}

