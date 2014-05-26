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
            // zwei bestimmte Personen möchten sich ein Essen teilen
            if (sharingFood(p1,p2))
                updateConstraint("c1", true);
            else constraintsFullfilled = false;

            // zwei bestimmte Personen möchten sich kein Essen teilen
            if (notSharingFood(p3,p4))
                updateConstraint("c2", true);
            else constraintsFullfilled = false;

            // Anzahl, wieviele Personenpaare sich ein Essen teilen
            if (numberSharingFood(1))
                updateConstraint("c3", true);
            else constraintsFullfilled = false;

            // eine bestimmte Person teilt sich ein Essen mit einer unbestimmten anderen
            if(sharingFood(p1))
                updateConstraint("c4", true);
            else constraintsFullfilled = false;

            // eine bestimmte Person möchte sich mit niemandem ein Essen teilen
            if (notSharingFood(p3))
                updateConstraint("c5", true);
            else constraintsFullfilled = false;

            // mindestens ein Nachbar teilt sich ein Essen
            if (neighbourSharingFood(p4))
                updateConstraint("c6", true);

            // kein Nachbar teilt sich ein Essen
            if (neighbourNotSharingFood(p3))
                updateConstraint("c7", true);

            return constraintsFullfilled;
        }
    }
}
