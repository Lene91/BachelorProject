using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;

namespace BachelorProject
{
    class Trial11 : ExampleExercise
    {


        public Trial11()
            : base()
        { id = 11; }

        public override bool checkActualConstraints()
        {
            if (oneNeighbourSharingFood(p1))
                updateConstraint("c1", true);
            else constraintsFullfilled = false;

            if (atLeastOneNeighbourSharingFood(p1))
                updateConstraint("c2", true);
            else constraintsFullfilled = false;

            if (noNeighbourSharingFood(p1))
                updateConstraint("c3", true);
            else constraintsFullfilled = false;

            if (oneNeighbourIsSeat(p6))
                updateConstraint("c4", true);
            else constraintsFullfilled = false;

            if (atLeastOneNeighbourIsSeat(p6))
                updateConstraint("c5", true);
            else constraintsFullfilled = false;

            if (noNeighbourIsSeat(p6))
                updateConstraint("c6", true);
            else constraintsFullfilled = false;


            return constraintsFullfilled;
        }
    }
}
