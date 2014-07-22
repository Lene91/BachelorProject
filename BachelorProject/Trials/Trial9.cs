namespace BachelorProject.Trials
{
    class Trial9 : ExampleExercise
    {
        public Trial9(double pupilSize)
            : base(pupilSize)
        { Id = 9; }

        protected override void CheckActualConstraints()
        {
            if (AtLeastOneNeighbourSharingFood(P3))
                UpdateConstraint("c1", true);
            else constraintsFullfilled = false;

            if (NotSharingFood(P1, P4))
                UpdateConstraint("c2", true);
            else constraintsFullfilled = false;

            if (NotSittingNextToEachOther(P2, P3))
                UpdateConstraint("c3", true);
            else constraintsFullfilled = false;

            if (SharingFood(P5))
                UpdateConstraint("c4", true);
            else constraintsFullfilled = false;

            if (SharingFood(P4))
                UpdateConstraint("c5", true);
            else constraintsFullfilled = false;

            if (SharingFood(P2))
                UpdateConstraint("c6", true);
            else constraintsFullfilled = false;

            if (NotSharingFood(P1,P5))
                UpdateConstraint("c7", true);
            else constraintsFullfilled = false;

            if (NotSittingNextToEachOther(P5,P3))
                UpdateConstraint("c8", true);
            else constraintsFullfilled = false;

            /*
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
            */

            //return constraintsFullfilled;
        }
    }
}
