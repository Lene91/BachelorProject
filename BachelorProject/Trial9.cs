﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BachelorProject
{
    class Trial9 : ExampleExercise
    {
        public Trial9()
            : base()
        { id = 9; }

        public override bool checkActualConstraints()
        {
            if (notSittingNextToEachOther(p2, p4))
                updateConstraint("c1", true);
            else constraintsFullfilled = false;

            if (sittingNextToEachOther(p5, p1))
                updateConstraint("c2", true);
            else constraintsFullfilled = false;

            if (sittingNextToEachOther(p2, p3))
                updateConstraint("c3", true);
            else constraintsFullfilled = false;

            if (notSittingOn(p2, p4))
                updateConstraint("c4", true);
            else constraintsFullfilled = false;

            if (notSittingNextToEachOther(p1, p2))
                updateConstraint("c5", true);
            else constraintsFullfilled = false;

            if (sittingNextToEachOther(p4, p1))
                updateConstraint("c6", true);
            else constraintsFullfilled = false;


            return constraintsFullfilled;
        }
    }
}
