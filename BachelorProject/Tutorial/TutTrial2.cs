using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;

namespace BachelorProject
{
    class TutTrial2 : ExampleExercise
    {


        public TutTrial2()
            : base()
        { id = 1002; }

        public override bool checkActualConstraints()
        {
            if (notSittingNextToEachOther(p4, p2))
                updateConstraint("c1", true);
            else constraintsFullfilled = false;

            if (sittingNextToEachOther(p5, p1))
                updateConstraint("c2", true);
            else constraintsFullfilled = false;

            if (sittingOn(p2,p1))
                updateConstraint("c3", true);
            else constraintsFullfilled = false;

            if (sharingFood(p3,p4))
                updateConstraint("c4", true);
            else constraintsFullfilled = false;


            return constraintsFullfilled;
        }
    }
}
