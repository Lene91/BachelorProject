namespace BachelorProject.Trials
{
    class Trial10 : ExampleExercise
    {
        public Trial10(double pupilSize)
            : base(pupilSize)
        { Id = 10; }

        protected override void CheckActualConstraints()
        {

            if (NotSittingNextToEachOther(P3, P6))
                UpdateConstraint("c1", true);
            else constraintsFullfilled = false;

            if (SittingNextToEachOther(P1,P2))
                UpdateConstraint("c2", true);
            else constraintsFullfilled = false;

            if (OneNeighbourSharingFood(P3))
                UpdateConstraint("c3", true);
            else constraintsFullfilled = false;

            if (NoNeighbourSharingFood(P5))
                UpdateConstraint("c4", true);
            else constraintsFullfilled = false;

            if (NotSittingNextToEachOther(P6, P4))
                UpdateConstraint("c5", true);
            else constraintsFullfilled = false;

            if (NotSittingNextToEachOther(P2, P6))
                UpdateConstraint("c6", true);
            else constraintsFullfilled = false;

            if (NotSharingFood(P3))
                UpdateConstraint("c7", true);
            else constraintsFullfilled = false;

            if (NotSharingFood(P4))
                UpdateConstraint("c8", true);
            else constraintsFullfilled = false;

            if (NumberSittingOn(0))
                UpdateConstraint("c9", true);
            else constraintsFullfilled = false;

            /*
            if (numberSharingFood(3))
                updateConstraint("c1", true);
            else constraintsFullfilled = false;

            if (sittingNextToEachOther(p3,p2))
                updateConstraint("c2", true);
            else constraintsFullfilled = false;

            if (sittingNextToEachOther(p6, p5))
                updateConstraint("c3", true);
            else constraintsFullfilled = false;

            if (notSharingFood(p6, p1))
                updateConstraint("c4", true);
            else constraintsFullfilled = false;

            if (notSharingFood(p4, p1))
                updateConstraint("c5", true);
            else constraintsFullfilled = false;

            if (sittingNextToEachOther(p1, p6))
                updateConstraint("c6", true);
            else constraintsFullfilled = false;

            if (sittingNextToEachOther(p4, p5))
                updateConstraint("c7", true);
            else constraintsFullfilled = false;

            if (notSharingFood(p5, p4))
                updateConstraint("c8", true);
            else constraintsFullfilled = false;

            if (notSharingFood(p2, p4))
                updateConstraint("c9", true);
            else constraintsFullfilled = false;
             */

            //return constraintsFullfilled;
        }
    }
}


