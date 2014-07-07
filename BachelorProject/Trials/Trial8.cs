namespace BachelorProject.Trials
{
    class Trial8 : ExampleExercise
    {
        public Trial8(double pupilSize)
            : base(pupilSize)
        { Id = 8; }

        public override void CheckActualConstraints()
        {
            if (SittingNextToEachOther(P2, P1))
                UpdateConstraint("c1", true);
            else constraintsFullfilled = false;

            if (NotSittingNextToEachOther(P3, P1))
                UpdateConstraint("c2", true);
            else constraintsFullfilled = false;

            if (OneNeighbourSharingFood(P5))
                UpdateConstraint("c3", true);
            else constraintsFullfilled = false;

            if (OneNeighbourIsSeat(P1))
                UpdateConstraint("c4", true);
            else constraintsFullfilled = false;

            if (NotSittingNextToEachOther(P2,P4))
                UpdateConstraint("c5", true);
            else constraintsFullfilled = false;

            if (NotSharingFood(P1))
                UpdateConstraint("c6", true);
            else constraintsFullfilled = false;

            if (NotSittingOnSomeone(P3))
                UpdateConstraint("c7", true);
            else constraintsFullfilled = false;

            if (IsSeat(P5))
                UpdateConstraint("c8", true);
            else constraintsFullfilled = false;


            /*
            if (sittingNextToEachOther(p2, p4))
                updateConstraint("c1", true);
            else constraintsFullfilled = false;

            if (notSittingNextToEachOther(p1, p2))
                updateConstraint("c2", true);
            else constraintsFullfilled = false;

            if (notSittingNextToEachOther(p3, p4))
                updateConstraint("c3", true);
            else constraintsFullfilled = false;

            if (notSittingOnSomeone(p3))
                updateConstraint("c4", true);
            else constraintsFullfilled = false;

            if (isNotSeat(p2))
                updateConstraint("c5", true);
            else constraintsFullfilled = false;
            */

            //return constraintsFullfilled;
        }
    }
}

