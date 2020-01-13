using System;

namespace ProducerConsumer.Core
{
    public class Task
    {
        public DateTime BeginConsumptionTime { get; set; }
        public DateTime CreationTime { get; set; }

        public DateTime EndOfConsumption { get; set; }
        public int TaskNumber { get; set; } = 0;

        public bool IsBeeingConsumed { get; set; }

        private FastClock clock = FastClock.Instance;

        public static event EventHandler<string> LogTask;

        public Task(int number)
        {
            TaskNumber = number;
            Start();
        }

        public void Start()
        {
            CreationTime = clock.Time;
        }

        public void MakeLogEntry(string s)
        {
            OnLogTask(s);
        }

        public void BeginConsumption()
        {
            BeginConsumptionTime = clock.Time;
            IsBeeingConsumed = true;
        }

        public void Finish()
        {
            IsBeeingConsumed = false;
            EndOfConsumption = clock.Time;
        }

        protected virtual void OnLogTask(string text)
        {
            LogTask?.Invoke(this, text); 
        }
    }
}
