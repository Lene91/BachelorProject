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
        { id = 3; }

        public override bool checkActualConstraints()
        {
            if (sharingFood(p2, p3))
                updateConstraint("c1", true);
            else constraintsFullfilled = false;

            if (sittingOn(p4, p1))
                updateConstraint("c2", true);
            else constraintsFullfilled = false;

            if (sittingNextToEachOther(p6, p2))
                updateConstraint("c3", true);
            else constraintsFullfilled = false;

            if (notSittingNextToEachOther(p5, p6))
                updateConstraint("c4", true);
            else constraintsFullfilled = false;
            

            /*
            if (numberSharingFood(1))
                updateConstraint("c1", true);
            else constraintsFullfilled = false;

            if (numberSittingOn(0))
                updateConstraint("c2", true);
            else constraintsFullfilled = false;

            if (sittingNextToEachOther(p4,p5))
                updateConstraint("c3", true);
            else constraintsFullfilled = false;

            if (sittingNextToEachOther(p3,p4))
                updateConstraint("c4", true);
            else constraintsFullfilled = false;

            if (sharingFood(p2,p1))
                updateConstraint("c5", true);
            else constraintsFullfilled = false;

            if (sittingNextToEachOther(p3,p2))
                updateConstraint("c6", true);
            else constraintsFullfilled = false;

            if (atLeastOneNeighbourSharingFood(p5))
                updateConstraint("c7", true);
            else constraintsFullfilled = false;
            */

            return constraintsFullfilled;
        }
    }
}
