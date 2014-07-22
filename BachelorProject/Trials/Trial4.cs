using System;

namespace BachelorProject.Trials
{
    class Trial4 : ExampleExercise
    {
        public Trial4(double pupilSize)
            : base(pupilSize)
        {
            Id = 4;
            _constraintsWithPersons.Add("c1", new Tuple<string, string>("Person2", null));
            _constraintsWithPersons.Add("c2", new Tuple<string, string>("Person4", null));
            _constraintsWithPersons.Add("c3", new Tuple<string, string>("Person1", null));
            _constraintsWithPersons.Add("c4", new Tuple<string, string>("Person3", null));
            _constraintsWithPersons.Add("c5", new Tuple<string, string>("Person1", null));
            _constraintsWithPersons.Add("c6", new Tuple<string, string>("Person1", "Person2"));
            _constraintsWithPersons.Add("c7", new Tuple<string, string>("Person6", "Person3"));
            _constraintsWithPersons.Add("c8", new Tuple<string, string>("Person5", "Person6"));
        }

        protected override void CheckActualConstraints()
        {
            UpdateConstraint("c1", SharingFood(P2));
            UpdateConstraint("c2", SittingOnSomeone(P4));
            UpdateConstraint("c3", NoNeighbourSharingFood(P1));
            UpdateConstraint("c4", OneNeighbourIsSeat(P3));
            UpdateConstraint("c5", OneNeighbourIsSeat(P1));
            UpdateConstraint("c6", NotSharingFood(P1,P2));
            UpdateConstraint("c7", NotSittingNextToEachOther(P6,P3));
            UpdateConstraint("c8", NotSittingNextToEachOther(P5, P6));

            /*
            if (SittingNextToEachOther(P2, P3))
                UpdateConstraint("c1", true);
            else constraintsFullfilled = false;

            if (NotSittingNextToEachOther(P1, P4))
                UpdateConstraint("c2", true);
            else constraintsFullfilled = false;

            if (OneNeighbourSharingFood(P1))
                UpdateConstraint("c3", true);
            else constraintsFullfilled = false;

            if (OneNeighbourIsSeat(P5))
                UpdateConstraint("c4", true);
            else constraintsFullfilled = false;

            if (SittingNextToEachOther(P3, P5))
                UpdateConstraint("c5", true);
            else constraintsFullfilled = false;

            if (OneNeighbourSharingFood(P5))
                UpdateConstraint("c6", true);
            else constraintsFullfilled = false;

            if (NotSittingNextToEachOther(P2, P5))
                UpdateConstraint("c7", true);
            else constraintsFullfilled = false;

            if (SittingOnSomeone(P4))
                UpdateConstraint("c8", true);
            else constraintsFullfilled = false;

            if (NumberSharingFood(1))
                UpdateConstraint("c9", true);
            else constraintsFullfilled = false;
            */
            /*
            if (sittingNextToEachOther(p3, p2))
                updateConstraint("c1", true);
            else constraintsFullfilled = false;

            if (sittingNextToEachOther(p4, p2))
                updateConstraint("c2", true);
            else constraintsFullfilled = false;

            if (sittingNextToEachOther(p1, p3))
                updateConstraint("c3", true);
            else constraintsFullfilled = false;

            if (sittingNextToEachOther(p5, p4))
                updateConstraint("c4", true);
            else constraintsFullfilled = false;

            if (numberSittingOn(1))
                updateConstraint("c5", true);
            else constraintsFullfilled = false;
            */

            //return constraintsFullfilled;
        }
    }
}
