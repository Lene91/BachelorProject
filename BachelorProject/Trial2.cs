﻿using System;
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
            if (p1.overlaps(p2))
                updateConstraint("c1", true);
            else constraintsFullfilled = false;
            if (!p3.overlaps(p4))
                updateConstraint("c2", true);
            else constraintsFullfilled = false;
            return constraintsFullfilled;
        }
    }
}
