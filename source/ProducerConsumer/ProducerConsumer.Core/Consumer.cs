using System;
using System.Collections.Generic;

namespace ProducerConsumer.Core
{
    public class Consumer
    {
        private int _minutesTillConsumptionIsFinished;
        private Task _currentTask;
        private Random _rnd;
        private int _minDuration;
        private int _maxDuration;
        private Queue<Task> _queue;

        public bool IsBusy { get; set; }

        public Consumer(int min, int max, Queue<Task> queue)
        {
            _rnd = new Random();
            _minDuration = min;
            _maxDuration = max;
            _queue = queue;
            _minutesTillConsumptionIsFinished = _rnd.Next(_minDuration, _maxDuration);
            FastClock.Instance.OneMinuteIsOver += Instance_OneMinuteIsOver;
        }

        private void Instance_OneMinuteIsOver(object sender, DateTime e)
        {
            if (!IsBusy)
            {
                _currentTask = _queue.Peek();
                _queue.Dequeue();
                string s = $"Queuelength: {_queue.Count}, Task {_currentTask.TaskNumber} wird bearbeitet!";
                _currentTask.BeginConsumption();
                _currentTask.MakeLogEntry(s);
                IsBusy = true;
            }
            else
            {
                if (e > _currentTask.BeginConsumptionTime.AddMinutes(_minutesTillConsumptionIsFinished) 
                    && _queue.Count > 0)
                {
                    _currentTask.Finish();
                    string s = $"Queuelength: {_queue.Count}, Task {_currentTask.TaskNumber} wurde um {_currentTask.CreationTime.ToShortTimeString()} erzeugt und von " +
                        $"{_currentTask.BeginConsumptionTime.ToShortTimeString()} - {_currentTask.EndOfConsumption.ToShortTimeString()} bearbeitet!";
                    _currentTask.MakeLogEntry(s);
                    _minutesTillConsumptionIsFinished = _rnd.Next(_minDuration, _maxDuration);
                    IsBusy = false;
                }
            }
        }
    }
}
