using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;

namespace BachelorProject
{
    class TutTrial1 : ExampleExercise
    {


        public TutTrial1()
            : base()
        { id = 1; }

        public override bool checkActualConstraints()
        {
            if (sittingNextToEachOther(p3, p1))
                updateConstraint("c1", true);
            else constraintsFullfilled = false;

            if (numberSharingFood(1))
                updateConstraint("c2", true);
            else constraintsFullfilled = false;

            if (sittingNextToEachOther(p2, p3))
                updateConstraint("c3", true);
            else constraintsFullfilled = false;

            if (notSharingFood(p3))
                updateConstraint("c4", true);
            else constraintsFullfilled = false;


            return constraintsFullfilled;
        }
    }
}
