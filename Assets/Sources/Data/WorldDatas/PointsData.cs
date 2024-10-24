using System;

namespace Assets.Sources.Data.WorldDatas
{
    [Serializable]
    public class PointsData
    {
        public uint[] Goals;

        public uint Goal;
        public uint pointsCount;

        public bool IsGoalsOvered;

        public PointsData(uint[] goals)
        {
            Goals = goals;

            Goal = Goals[0];
            pointsCount = 0;
            IsGoalsOvered = false;
        }

        public event Action Scorred;
        public event Action GoalAchieved;
        public event Action GoalsOvered;

        public void Give(uint count)
        {
            if (IsGoalsOvered)
                return;

            pointsCount += count;

            if(pointsCount >= Goal)
            {
                pointsCount = pointsCount - Goal;

                int currentGoalIndex = Array.IndexOf(Goals, Goal);

                if (currentGoalIndex >= Goals.Length - 1)
                {
                    IsGoalsOvered = true;
                    GoalsOvered?.Invoke();
                }
                else
                {
                    Goal = Goals[currentGoalIndex + 1];
                }

                GoalAchieved?.Invoke();
            }

            Scorred?.Invoke();

            //if(Scored >= Goal)
            //{
            //    Scored = Goal - Scored;

            //    int currentGoalIndex = Array.IndexOf(Goals, Goal);

            //    if (currentGoalIndex >= Goals.Length - 1)
            //        GoalsOvered?.Invoke();
            //    else
            //        Goal = Goals[currentGoalIndex + 1];

            //    GoalScored?.Invoke();
            //}
        }
    }
}