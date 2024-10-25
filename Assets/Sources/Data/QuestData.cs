using System;

namespace Assets.Sources.Data
{
    [Serializable]
    public class QuestData
    {
        public string Id;
        public uint Progress;
        public bool IsCompleted;

        public QuestData(string id)
        {
            Id = id;

            Progress = 0;
            IsCompleted = false;
        }

        public event Action Progressed;
        public event Action Completed;

        public void Perform(uint targetCount)
        {
            if (IsCompleted)
                return;

            Progress++;

            Progressed?.Invoke();

            if(Progress >= targetCount)
            {
                IsCompleted = true;
                Completed?.Invoke();
            }
        }
    }
}
