using System;

namespace Assets.Sources.Data.World
{
    [Serializable]
    public class PointsData
    {
        public uint[] Goals;

        public uint Goal;
        public uint PointsCount;

        public bool IsGoalsOvered;

        public PointsData(uint[] goals)
        {
            Goals = goals;

            Goal = Goals[0];
            PointsCount = 0;
            IsGoalsOvered = false;
        }

        public event Action Scorred;
        public event Action GoalAchieved;
        public event Action GoalsOvered;

        public void Give(uint count)
        {
            if (IsGoalsOvered)
                return;

            PointsCount += count;

            if (PointsCount >= Goal)
            {
                PointsCount = PointsCount - Goal;

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
        }
    }
}