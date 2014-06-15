using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;

namespace BachelorProject
{
    class TutTrial3 : ExampleExercise
    {


        public TutTrial3()
            : base()
        { id = 3; }

        public override bool checkActualConstraints()
        {
            if (oneNeighbourSharingFood(p3))
                updateConstraint("c1", true);
            else constraintsFullfilled = false;

            if (sittingNextToEachOther(p1, p4))
                updateConstraint("c2", true);
            else constraintsFullfilled = false;

            if (sharingFood(p2))
                updateConstraint("c3", true);
            else constraintsFullfilled = false;

            return constraintsFullfilled;
        }
    }
}
