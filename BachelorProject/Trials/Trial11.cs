namespace BachelorProject.Trials
{
    class Trial11 : ExampleExercise
    {


        public Trial11()
        { Id = 11; }

        public override bool CheckActualConstraints()
        {
            if (OneNeighbourSharingFood(P1))
                UpdateConstraint("c1", true);
            else constraintsFullfilled = false;

            if (AtLeastOneNeighbourSharingFood(P1))
                UpdateConstraint("c2", true);
            else constraintsFullfilled = false;

            if (NoNeighbourSharingFood(P1))
                UpdateConstraint("c3", true);
            else constraintsFullfilled = false;

            if (OneNeighbourIsSeat(P6))
                UpdateConstraint("c4", true);
            else constraintsFullfilled = false;

            if (AtLeastOneNeighbourIsSeat(P6))
                UpdateConstraint("c5", true);
            else constraintsFullfilled = false;

            if (NoNeighbourIsSeat(P6))
                UpdateConstraint("c6", true);
            else constraintsFullfilled = false;


            return constraintsFullfilled;
        }
    }
}
