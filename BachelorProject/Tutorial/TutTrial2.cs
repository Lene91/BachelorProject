namespace BachelorProject.Tutorial
{
    class TutTrial2 : ExampleExercise
    {


        public TutTrial2()
        { Id = 1002; }

        public override bool CheckActualConstraints()
        {
            if (NotSittingNextToEachOther(P4, P2))
                UpdateConstraint("c1", true);
            else constraintsFullfilled = false;

            if (SittingNextToEachOther(P5, P1))
                UpdateConstraint("c2", true);
            else constraintsFullfilled = false;

            if (SittingOn(P2,P1))
                UpdateConstraint("c3", true);
            else constraintsFullfilled = false;

            if (SharingFood(P3,P4))
                UpdateConstraint("c4", true);
            else constraintsFullfilled = false;


            return constraintsFullfilled;
        }
    }
}
