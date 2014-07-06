namespace BachelorProject.Trials
{
    class Trial3 : ExampleExercise
    {


        public Trial3()
        { Id = 3; }

        public override void CheckActualConstraints()
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

            if (NotSharingFood(P1, P5))
                UpdateConstraint("c7", true);
            else constraintsFullfilled = false;

            if (NotSittingNextToEachOther(P5, P3))
                UpdateConstraint("c8", true);
            else constraintsFullfilled = false;
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
