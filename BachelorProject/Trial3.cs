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
            // Person möchte auf dem Schoß einer bestimmten anderen sitzen (erste Person sitzt auf dem Schoß
            if (sittingOn(p1,p2))
                updateConstraint("c1", true);
            else constraintsFullfilled = false;

            // Person möchte nicht auf dem Schoß einer bestimmten anderen sitzen
            if (notSittingOn(p3,p4))
                updateConstraint("c2", true);
            else constraintsFullfilled = false;

            // Anzahl der Personen, die auf dem Schoß einer anderen sitzen
            if (numberSittingOn(1))
                updateConstraint("c3", true);
            else constraintsFullfilled = false;

            // bestimmte Person sitzt auf dem Schoß von einer beliebigen anderen
            if (sittingOnSomeone(p1))
                updateConstraint("c4", true);
            else constraintsFullfilled = false;

            // bestimmte Person sitzt auf niemandes Schoß
            if (notSittingOnSomeone(p3))
                updateConstraint("c5", true);
            else constraintsFullfilled = false;

            // jemand sitzt auf dem Schoß einer bestimmten Person
            if (isSeat(p4))
                updateConstraint("c6", true);
            else constraintsFullfilled = false;

            // niemand sitzt auf dem Schoß einer bestimmten Person
            if (isNotSeat(p3))
                updateConstraint("c7", true);
            else constraintsFullfilled = false;

            // mindestens ein Nachbar hat jmd auf dem Schoß
            if(neighbourIsSeat(p4))
                updateConstraint("c8", true);
            else constraintsFullfilled = false;

            // kein Nachbar hat jmd auf dem Schoß
            if (neighbourIsNotSeat(p3))
                updateConstraint("c9", true);
            else constraintsFullfilled = false;


            return constraintsFullfilled;
        }
    }
}
