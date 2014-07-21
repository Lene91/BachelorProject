using System;

namespace BachelorProject.Trials
{
    class Trial3 : ExampleExercise
    {


        public Trial3(double pupilSize)
            : base(pupilSize)
        {
            Id = 3;
            _constraintsWithPersons.Add("c1", new Tuple<string, string>("Person3", null));
            _constraintsWithPersons.Add("c2", new Tuple<string, string>("Person1", null));
            _constraintsWithPersons.Add("c3", new Tuple<string, string>("Person2", "Person3"));
            _constraintsWithPersons.Add("c4", new Tuple<string, string>("Person5", null));
            _constraintsWithPersons.Add("c5", new Tuple<string, string>("Person4", null));
            _constraintsWithPersons.Add("c6", new Tuple<string, string>("Person2", null));
            _constraintsWithPersons.Add("c7", new Tuple<string, string>("Person1", "Person5"));
            _constraintsWithPersons.Add("c8", new Tuple<string, string>("Person5", "Person3"));
        }

        public override void CheckActualConstraints()
        {
            UpdateConstraint("c1", IsSeat(P3));
            UpdateConstraint("c2", NotSittingOnSomeone(P1));
            UpdateConstraint("c3", NotSittingNextToEachOther(P2, P3));
            UpdateConstraint("c4", SharingFood(P5));
            UpdateConstraint("c5", SharingFood(P4));
            UpdateConstraint("c6", SharingFood(P2));
            UpdateConstraint("c7", NotSharingFood(P1, P5));
            UpdateConstraint("c8", NotSittingNextToEachOther(P5, P3));
           
            /*
            if (SharingFood(P2, P3))
                UpdateConstraint("c1", true);
            else constraintsFullfilled = false;

            if (SittingOn(P4, P1))
                UpdateConstraint("c2", true);
            else constraintsFullfilled = false;

            if (SittingNextToEachOther(P6, P2))
                UpdateConstraint("c3", true);
            else constraintsFullfilled = false;

            if (NotSittingNextToEachOther(P5, P6))
                UpdateConstraint("c4", true);
            else constraintsFullfilled = false;
            */

            /*
            if (numberSharingFood(1))
                updateConstraint("c1", true);
            else constraintsFullfilled = false;

            if (numberSittingOn(0))
                updateConstraint("c2", true);
            else constraintsFullfilled = false;

            if (sittingNextToEachOther(p4,p5))
                updateConstraint("c3", true);
            else constraintsFullfilled = false;

            if (sittingNextToEachOther(p3,p4))
                updateConstraint("c4", true);
            else constraintsFullfilled = false;

            if (sharingFood(p2,p1))
                updateConstraint("c5", true);
            else constraintsFullfilled = false;

            if (sittingNextToEachOther(p3,p2))
                updateConstraint("c6", true);
            else constraintsFullfilled = false;

            if (atLeastOneNeighbourSharingFood(p5))
                updateConstraint("c7", true);
            else constraintsFullfilled = false;
            */

            //return constraintsFullfilled;
        }
    }
}
