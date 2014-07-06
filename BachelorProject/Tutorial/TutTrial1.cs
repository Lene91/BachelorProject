namespace BachelorProject.Tutorial
{
    class TutTrial1 : ExampleExercise
    {


        public TutTrial1()
        { Id = 1001; }

        public override void CheckActualConstraints()
        {
            if (SittingNextToEachOther(P3, P1))
                UpdateConstraint("c1", true);
            else constraintsFullfilled = false;

            if (NumberSharingFood(1))
                UpdateConstraint("c2", true);
            else constraintsFullfilled = false;

            if (SittingNextToEachOther(P2, P3))
                UpdateConstraint("c3", true);
            else constraintsFullfilled = false;

            if (NotSharingFood(P3))
                UpdateConstraint("c4", true);
            else constraintsFullfilled = false;


            //return constraintsFullfilled;
        }
    }
}
