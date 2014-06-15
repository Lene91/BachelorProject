using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;

namespace BachelorProject
{
    class Trial1 : ExampleExercise
    {


        public Trial1()
            : base()
        { id = 1; }

        public override bool checkActualConstraints()
        {
            if (sharingFood(p2, p3))
                updateConstraint("c1", true);
            else constraintsFullfilled = false;

            if (notSittingNextToEachOther(p2, p5))
                updateConstraint("c2", true);
            else constraintsFullfilled = false;

            if (sittingNextToEachOther(p1, p2))
                updateConstraint("c3", true);
            else constraintsFullfilled = false;

            if (sittingNextToEachOther(p2, p4))
                updateConstraint("c4", true);
            else constraintsFullfilled = false;
            /*
            if (sittingNextToEachOther(p1, p2))
                updateConstraint("c1", true);
            else constraintsFullfilled = false;

            if (sittingNextToEachOther(p3, p2))
                updateConstraint("c2", true);
            else constraintsFullfilled = false;

            if (sharingFood(p3, p4))
                updateConstraint("c3", true);
            else constraintsFullfilled = false;
            */

            return constraintsFullfilled;
        }
    }
}
