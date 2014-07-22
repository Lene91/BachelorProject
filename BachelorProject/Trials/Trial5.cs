using System;

namespace BachelorProject.Trials
{
    class Trial5 : ExampleExercise
    {
        public Trial5(double pupilSize)
            : base(pupilSize)
        {
            Id = 5;
            _constraintsWithPersons.Add("c1", new Tuple<string, string>("Person1", "Person5"));
            _constraintsWithPersons.Add("c2", new Tuple<string, string>("Person6", null));
            _constraintsWithPersons.Add("c3", new Tuple<string, string>("Person5", "Person4"));
            _constraintsWithPersons.Add("c4", new Tuple<string, string>("Person3", "Person1"));
            _constraintsWithPersons.Add("c5", new Tuple<string, string>("Person4", null));
            _constraintsWithPersons.Add("c6", new Tuple<string, string>("Person4", "Person1"));
            _constraintsWithPersons.Add("c7", new Tuple<string, string>("Person2", null));
            _constraintsWithPersons.Add("c8", new Tuple<string, string>("Person6", "Person1"));
        }

        protected override void CheckActualConstraints()
        {
            UpdateConstraint("c1", SittingNextToEachOther(P1, P5));
            UpdateConstraint("c2", NoNeighbourIsSeat(P6));
            UpdateConstraint("c3", NotSittingOn(P5,P4));
            UpdateConstraint("c4", NotSittingNextToEachOther(P3, P1));
            UpdateConstraint("c5", NotSittingOnSomeone(P4));
            UpdateConstraint("c6", SittingNextToEachOther(P4, P1));
            UpdateConstraint("c7", IsSeat(P2));
            UpdateConstraint("c8", SittingNextToEachOther(P6,P1));
            

            /*
            if (NotSittingNextToEachOther(P2, P4))
                UpdateConstraint("c1", true);
            else constraintsFullfilled = false;

            if (SittingNextToEachOther(P5, P1))
                UpdateConstraint("c2", true);
            else constraintsFullfilled = false;

            if (NotSittingOn(P2, P4))
                UpdateConstraint("c3", true);
            else constraintsFullfilled = false;

            if (NotSittingNextToEachOther(P1, P2))
                UpdateConstraint("c4", true);
            else constraintsFullfilled = false;

            if (SittingNextToEachOther(P3, P2))
                UpdateConstraint("c5", true);
            else constraintsFullfilled = false;

            if (SittingNextToEachOther(P4, P5))
                UpdateConstraint("c6", true);
            else constraintsFullfilled = false;
            */

            /*
            if (notSittingNextToEachOther(p4, p1))
                updateConstraint("c1", true);
            else constraintsFullfilled = false;

            if (sittingNextToEachOther(p5, p2))
                updateConstraint("c2", true);
            else constraintsFullfilled = false;

            if (notSittingNextToEachOther(p1, p5))
                updateConstraint("c3", true);
            else constraintsFullfilled = false;

            if (numberSharingFood(2))
                updateConstraint("c4", true);
            else constraintsFullfilled = false;

            if (notSharingFood(p3))
                updateConstraint("c5", true);
            else constraintsFullfilled = false;
            */

            //return constraintsFullfilled;
        }
    }
}
