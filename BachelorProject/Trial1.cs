﻿using System;
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

            if (p1.touches(table))
                updateConstraint("c1", true);
            else constraintsFullfilled = false;
            return constraintsFullfilled;
        }
    }
}