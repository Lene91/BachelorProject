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
        { }

        public override bool checkActualConstraints()
        {
            // zwei bestimmte Personen möchten nebeneinander sitzen
            if (sittingNextToEachOther(p1, p2))
                updateConstraint("c1", true);
            else constraintsFullfilled = false;

            // zwei bestimmte Personen möchten nicht nebeneinander sitzen
            if (notSittingNextToEachOther(p3, p4))
                updateConstraint("c2", true);

            return constraintsFullfilled;
        }
    }
}
