using System;
using System.Collections.Generic;

namespace ProducerConsumer.Core
{
    public class Producer
    {
        private int _minutesTillNextProduction;
        private Random _rnd;
        private int _minDuration;
        private int _maxDuration;
        private Queue<Task> _queue;
        private int _taskNumber = 0;
        private DateTime _lastProduction = new DateTime();
        private FastClock _clock;

        public Producer(int min, int max, Queue<Task> queue)
        {
            _minDuration = min;
            _maxDuration = max;
            _queue = queue;
            _rnd = new Random();
            _minutesTillNextProduction = _rnd.Next(_minDuration, _maxDuration);
            _clock = FastClock.Instance;
            _clock.OneMinuteIsOver += Instance_OneMinuteIsOver;
        }

        private void Instance_OneMinuteIsOver(object sender, DateTime e)
        {
            if(e > _lastProduction.AddMinutes(_minutesTillNextProduction))
            {
                _queue.Enqueue(new Task(_taskNumber));
                string s = $"Queuelength: {_queue.Count}, Task {_taskNumber} erzeugt!";
                _minutesTillNextProduction = _rnd.Next(_minDuration, _maxDuration);
                _lastProduction = _clock.Time;
                _taskNumber++;
                _queue.Peek().MakeLogEntry(s);
            }
        }
    }
}
