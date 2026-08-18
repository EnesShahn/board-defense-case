using System;

namespace ESF.Core.PriorityActions
{
    public class PriorityAction
    {
        private readonly Action[] _calls;
        private int _levelsCount;
        public PriorityAction()
        {
            _levelsCount = Enum.GetValues(typeof(PriorityLevel)).Length;
            _calls = new Action[_levelsCount];
        }
        public void AddListener(Action call, PriorityLevel priorityLevel = PriorityLevel.Default)
        {
            if (call == null) return;
            _calls[(int)priorityLevel] += call;
        }
        public void RemoveListener(Action call)
        {
            if (call == null) return;
            for (int i = 0; i < _levelsCount; i++)
            {
                if (_calls[i] == null) continue;
                int prevListenerCount = _calls[i].GetInvocationList().Length;
                _calls[i] -= call;
                int newListenerCount = _calls[i] != null ? _calls[i].GetInvocationList().Length : 0;
                if (newListenerCount != prevListenerCount)
                    return;
            }
        }
        public void Invoke()
        {
            for (int i = _levelsCount - 1; i >= 0; i--)
                _calls[i]?.Invoke();
        }
        public void Clear()
        {
            for (int i = 0; i < _levelsCount; i++)
                _calls[i] = null;
        }
    }

    public class PriorityAction<T0>
    {
        private readonly Action<T0>[] _calls;
        private int levels_Count;
        public PriorityAction()
        {
            levels_Count = Enum.GetValues(typeof(PriorityLevel)).Length;
            _calls = new Action<T0>[levels_Count];
        }
        public void AddListener(Action<T0> call, PriorityLevel priorityLevel = PriorityLevel.Default)
        {
            if (call == null) return;
            _calls[(int)priorityLevel] += call;
        }
        public void RemoveListener(Action<T0> call)
        {
            if (call == null) return;
            for (int i = 0; i < levels_Count; i++)
            {
                if (_calls[i] == null) continue;
                int prevListenerCount = _calls[i].GetInvocationList().Length;
                _calls[i] -= call;
                int newListenerCount = _calls[i] != null ? _calls[i].GetInvocationList().Length : 0;
                if (newListenerCount != prevListenerCount)
                    return;
            }
        }
        public void Invoke(T0 arg0)
        {
            for (int i = levels_Count - 1; i >= 0; i--)
                _calls[i]?.Invoke(arg0);
        }
        public void Clear()
        {
            for (int i = 0; i < levels_Count; i++)
                _calls[i] = null;
        }
    }

    public class PriorityAction<T0, T1>
    {
        private readonly Action<T0, T1>[] _calls;
        private int _levelsCount;
        public PriorityAction()
        {
            _levelsCount = Enum.GetValues(typeof(PriorityLevel)).Length;
            _calls = new Action<T0, T1>[_levelsCount];
        }
        public void AddListener(Action<T0, T1> call, PriorityLevel priorityLevel = PriorityLevel.Default)
        {
            if (call == null) return;
            _calls[(int)priorityLevel] += call;
        }
        public void RemoveListener(Action<T0, T1> call)
        {
            if (call == null) return;
            for (int i = 0; i < _levelsCount; i++)
            {
                if (_calls[i] == null) continue;
                int prevListenerCount = _calls[i].GetInvocationList().Length;
                _calls[i] -= call;
                int newListenerCount = _calls[i] != null ? _calls[i].GetInvocationList().Length : 0;
                if (newListenerCount != prevListenerCount)
                    return;
            }
        }
        public void Invoke(T0 arg0, T1 arg1)
        {
            for (int i = _levelsCount - 1; i >= 0; i--)
                _calls[i]?.Invoke(arg0, arg1);
        }
        public void Clear()
        {
            for (int i = 0; i < _levelsCount; i++)
                _calls[i] = null;
        }
    }

    public class PriorityAction<T0, T1, T2>
    {
        private readonly Action<T0, T1, T2>[] _calls;
        private int _levelsCount;
        public PriorityAction()
        {
            _levelsCount = Enum.GetValues(typeof(PriorityLevel)).Length;
            _calls = new Action<T0, T1, T2>[_levelsCount];
        }
        public void AddListener(Action<T0, T1, T2> call, PriorityLevel priorityLevel = PriorityLevel.Default)
        {
            if (call == null) return;
            _calls[(int)priorityLevel] += call;
        }
        public void RemoveListener(Action<T0, T1, T2> call)
        {
            if (call == null) return;
            for (int i = 0; i < _levelsCount; i++)
            {
                if (_calls[i] == null) continue;
                int prevListenerCount = _calls[i].GetInvocationList().Length;
                _calls[i] -= call;
                int newListenerCount = _calls[i] != null ? _calls[i].GetInvocationList().Length : 0;
                if (newListenerCount != prevListenerCount)
                    return;
            }
        }
        public void Invoke(T0 arg0, T1 arg1, T2 arg2)
        {
            for (int i = _levelsCount - 1; i >= 0; i--)
                _calls[i]?.Invoke(arg0, arg1, arg2);
        }
        public void Clear()
        {
            for (int i = 0; i < _levelsCount; i++)
                _calls[i] = null;
        }
    }
    
    public class PriorityAction<T0, T1, T2, T3>
    {
        private readonly Action<T0, T1, T2, T3>[] _calls;
        private int _levelsCount;
        public PriorityAction()
        {
            _levelsCount = Enum.GetValues(typeof(PriorityLevel)).Length;
            _calls = new Action<T0, T1, T2, T3>[_levelsCount];
        }
        public void AddListener(Action<T0, T1, T2, T3> call, PriorityLevel priorityLevel = PriorityLevel.Default)
        {
            if (call == null) return;
            _calls[(int)priorityLevel] += call;
        }
        public void RemoveListener(Action<T0, T1, T2, T3> call)
        {
            if (call == null) return;
            for (int i = 0; i < _levelsCount; i++)
            {
                if (_calls[i] == null) continue;
                int prevListenerCount = _calls[i].GetInvocationList().Length;
                _calls[i] -= call;
                int newListenerCount = _calls[i] != null ? _calls[i].GetInvocationList().Length : 0;
                if (newListenerCount != prevListenerCount)
                    return;
            }
        }
        public void Invoke(T0 arg0, T1 arg1, T2 arg2, T3 arg3)
        {
            for (int i = _levelsCount - 1; i >= 0; i--)
                _calls[i]?.Invoke(arg0, arg1, arg2, arg3);
        }
        public void Clear()
        {
            for (int i = 0; i < _levelsCount; i++)
                _calls[i] = null;
        }
    }

    public class PriorityAction<T0, T1, T2, T3, T4>
    {
        private readonly Action<T0, T1, T2, T3, T4>[] _calls;
        private int _levelsCount;
        public PriorityAction()
        {
            _levelsCount = Enum.GetValues(typeof(PriorityLevel)).Length;
            _calls = new Action<T0, T1, T2, T3, T4>[_levelsCount];
        }
        public void AddListener(Action<T0, T1, T2, T3, T4> call, PriorityLevel priorityLevel = PriorityLevel.Default)
        {
            if (call == null) return;
            _calls[(int)priorityLevel] += call;
        }
        public void RemoveListener(Action<T0, T1, T2, T3, T4> call)
        {
            if (call == null) return;
            for (int i = 0; i < _levelsCount; i++)
            {
                if (_calls[i] == null) continue;
                int prevListenerCount = _calls[i].GetInvocationList().Length;
                _calls[i] -= call;
                int newListenerCount = _calls[i] != null ? _calls[i].GetInvocationList().Length : 0;
                if (newListenerCount != prevListenerCount)
                    return;
            }
        }
        public void Invoke(T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            for (int i = _levelsCount - 1; i >= 0; i--)
                _calls[i]?.Invoke(arg0, arg1, arg2, arg3, arg4);
        }
        public void Clear()
        {
            for (int i = 0; i < _levelsCount; i++)
                _calls[i] = null;
        }
    }

}