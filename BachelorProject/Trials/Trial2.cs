using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BachelorProject
{
    class Trial2 : ExampleExercise
    {

        public Trial2()
            : base()
        { id = 2; }

        public override bool checkActualConstraints()
        {
            if (sharingFood(p2, p3))
                updateConstraint("c1", true);
            else constraintsFullfilled = false;

            if (sittingNextToEachOther(p1, p2))
                updateConstraint("c2", true);
            else constraintsFullfilled = false;

            if (sittingNextToEachOther(p3, p4))
                updateConstraint("c3", true);
            else constraintsFullfilled = false;

            if (notSittingNextToEachOther(p1, p4))
                updateConstraint("c4", true);
            else constraintsFullfilled = false;
            

            /*
            if (notSittingNextToEachOther(p2,p4))
                updateConstraint("c1", true);
            else constraintsFullfilled = false;

            if (sittingOnSomeone(p5))
                updateConstraint("c2", true);
            else constraintsFullfilled = false;

            if (sittingNextToEachOther(p5,p2))
                updateConstraint("c3", true);
            else constraintsFullfilled = false;

            if(isNotSeat(p1))
                updateConstraint("c4", true);
            else constraintsFullfilled = false;
             */

            return constraintsFullfilled;
        }
    }
}
