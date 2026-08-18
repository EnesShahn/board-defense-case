using System;

namespace ESF.Core.PriorityEvents
{
    public class PriorityEvent
    {
        private readonly EventHandler[] _handlers = new EventHandler[7];

        public void AddListener(EventHandler handler, PriorityLevel priorityLevel = PriorityLevel.Default)
        {
            if (handler == null) return;
            _handlers[(int)priorityLevel] += handler;
        }

        public void RemoveListener(EventHandler handler)
        {
            if (handler == null) return;
            for (int i = 0; i < 7; i++)
            {
                if (_handlers[i] == null) continue;
                int prevListenerCount = _handlers[i].GetInvocationList().Length;
                _handlers[i] -= handler;
                int newListenerCount = _handlers[i] != null ? _handlers[i].GetInvocationList().Length : 0;
                if (newListenerCount != prevListenerCount)
                    return;
            }
        }

        public void Invoke(object sender, EventArgs args)
        {
            for (int i = 7 - 1; i >= 0; i--)
                _handlers[i]?.Invoke(sender, args);
        }

        public void Clear()
        {
            for (int i = 0; i < 7; i++)
                _handlers[i] = null;
        }
    }

    public class PriorityEvent<TArgs>
    {
        private readonly EventHandler<TArgs>[] _handlers = new EventHandler<TArgs>[7];

        public void AddListener(EventHandler<TArgs> call, PriorityLevel priorityLevel = PriorityLevel.Default)
        {
            if (call == null) return;
            _handlers[(int)priorityLevel] += call;
        }

        public void RemoveListener(EventHandler<TArgs> call)
        {
            if (call == null) return;
            for (int i = 0; i < 7; i++)
            {
                if (_handlers[i] == null) continue;
                int prevListenerCount = _handlers[i].GetInvocationList().Length;
                _handlers[i] -= call;
                int newListenerCount = _handlers[i] != null ? _handlers[i].GetInvocationList().Length : 0;
                if (newListenerCount != prevListenerCount)
                    return;
            }
        }

        public void Invoke(object sender, TArgs args)
        {
            for (int i = 7 - 1; i >= 0; i--)
                _handlers[i]?.Invoke(sender, args);
        }

        public void Clear()
        {
            for (int i = 0; i < 7; i++)
                _handlers[i] = null;
        }
    }
}