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
        { }

        public override bool checkActualConstraints()
        {
            // zwei Personen möchten sich ein Essen teilen
            if (sharingFood(p1,p2))
                updateConstraint("c1", true);
            else constraintsFullfilled = false;

            // zwei Personen möchten sich kein Essen teilen
            if (notSharingFood(p3,p4))
                updateConstraint("c2", true);
            else constraintsFullfilled = false;

            return constraintsFullfilled;
        }
    }
}
