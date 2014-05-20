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

            if (p2.touches(table))
                updateConstraint("c1", true);
            else constraintsFullfilled = false;
            return constraintsFullfilled;
        }
    }
}
