namespace BachelorProject.Trials
{
    class Trial2 : ExampleExercise
    {

        public Trial2()
        { Id = 2; }

        public override bool CheckActualConstraints()
        {
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

            if (IsNotSeat(P2))
                UpdateConstraint("c7", true);
            else constraintsFullfilled = false;

            /*if (SharingFood(P2, P3))
                UpdateConstraint("c1", true);
            else constraintsFullfilled = false;

            if (SittingNextToEachOther(P1, P2))
                UpdateConstraint("c2", true);
            else constraintsFullfilled = false;

            if (SittingNextToEachOther(P3, P4))
                UpdateConstraint("c3", true);
            else constraintsFullfilled = false;

            if (NotSittingNextToEachOther(P1, P4))
                UpdateConstraint("c4", true);
            else constraintsFullfilled = false;
            */

            /*
            if (notSittingNextToEachOther(p2,p4))
                updateConstraint("c1", true);
            else constraintsFullfilled = false;

            if (sittingOnSomeone(p5))
                updateConstraint("c2", true);
            else constraintsFullfilled = false;

            if (sittingNextToEachOther(p5,p2))
                updateConstraint("c3", true);
            else constraintsFullfilled = false;

            if(isNotSeat(p1))
                updateConstraint("c4", true);
            else constraintsFullfilled = false;
             */

            return constraintsFullfilled;
        }
    }
}
