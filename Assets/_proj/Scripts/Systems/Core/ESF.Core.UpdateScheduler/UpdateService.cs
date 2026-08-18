using System;
using UnityEngine;

namespace ESF.Core.UpdateScheduler
{
    public class UpdateService
    {
        internal const int PreUpdateHandlerExecutionOrder = -1000;
        internal const int NormalUpdateHandlerExecutionOrder = 0;
        internal const int PostUpdateHandlerExecutionOrder = 1000;

        private readonly PreUpdateHandler _preUpdateHandler;
        private readonly NormalUpdateHandler _normalUpdateHandler;
        private readonly PostUpdateHandler _postUpdateHandler;


        public event Action OnPreUpdate;
        public event Action OnUpdate;
        public event Action OnPostUpdate;

        public event Action OnFixedPreUpdate;
        public event Action OnFixedUpdate;
        public event Action OnFixedPostUpdate;

        public event Action OnLatePreUpdate;
        public event Action OnLateUpdate;
        public event Action OnLatePostUpdate;


        public UpdateService()
        {
            var updateHandlersContainer = new GameObject("Update Handler");
            GameObject.DontDestroyOnLoad(updateHandlersContainer);
            _preUpdateHandler = updateHandlersContainer.AddComponent<PreUpdateHandler>();
            _normalUpdateHandler = updateHandlersContainer.AddComponent<NormalUpdateHandler>();
            _postUpdateHandler = updateHandlersContainer.AddComponent<PostUpdateHandler>();

            _preUpdateHandler.Init(this);
            _normalUpdateHandler.Init(this);
            _postUpdateHandler.Init(this);
        }

        public void AddUpdateListener(Action action, UpdateOrder updateOrder)
        {
            switch (updateOrder)
            {
                case UpdateOrder.PreUpdate:
                    OnPreUpdate += action;
                    return;
                case UpdateOrder.Update:
                    OnUpdate += action;
                    return;
                case UpdateOrder.PostUpdate:
                    OnPostUpdate += action;
                    return;
                case UpdateOrder.LatePreUpdate:
                    OnLatePreUpdate += action;
                    return;
                case UpdateOrder.LateUpdate:
                    OnLateUpdate += action;
                    return;
                case UpdateOrder.LatePostUpdate:
                    OnLatePostUpdate += action;
                    return;
                case UpdateOrder.FixedPreUpdate:
                    OnFixedPreUpdate += action;
                    return;
                case UpdateOrder.FixedUpdate:
                    OnFixedUpdate += action;
                    return;
                case UpdateOrder.FixedPostUpdate:
                    OnFixedPostUpdate += action;
                    return;
                default:
                    throw new ArgumentOutOfRangeException(nameof(updateOrder), updateOrder, null);
            }
        }
        public void RemoveUpdateListener(Action action, UpdateOrder updateOrder)
        {
            switch (updateOrder)
            {
                case UpdateOrder.PreUpdate:
                    OnPreUpdate -= action;
                    return;
                case UpdateOrder.Update:
                    OnUpdate -= action;
                    return;
                case UpdateOrder.PostUpdate:
                    OnPostUpdate -= action;
                    return;
                case UpdateOrder.LatePreUpdate:
                    OnLatePreUpdate -= action;
                    return;
                case UpdateOrder.LateUpdate:
                    OnLateUpdate -= action;
                    return;
                case UpdateOrder.LatePostUpdate:
                    OnLatePostUpdate -= action;
                    return;
                case UpdateOrder.FixedPreUpdate:
                    OnFixedPreUpdate -= action;
                    return;
                case UpdateOrder.FixedUpdate:
                    OnFixedUpdate -= action;
                    return;
                case UpdateOrder.FixedPostUpdate:
                    OnFixedPostUpdate -= action;
                    return;
                default:
                    throw new ArgumentOutOfRangeException(nameof(updateOrder), updateOrder, null);
            }
        }

        internal void PreUpdate() => OnPreUpdate?.Invoke();
        internal void Update() => OnUpdate?.Invoke();
        internal void PostUpdate() => OnPostUpdate?.Invoke();

        internal void PreLateUpdate() => OnLatePreUpdate?.Invoke();
        internal void LateUpdate() => OnLateUpdate?.Invoke();
        internal void PostLateUpdate() => OnLatePostUpdate?.Invoke();

        internal void PreFixedUpdate() => OnFixedPreUpdate?.Invoke();
        internal void FixedUpdate() => OnFixedUpdate?.Invoke();
        internal void PostFixedUpdate() => OnFixedPostUpdate?.Invoke();
    }

    public enum UpdateOrder
    {
        PreUpdate,
        Update,
        PostUpdate,
        LatePreUpdate,
        LateUpdate,
        LatePostUpdate,
        FixedPreUpdate,
        FixedUpdate,
        FixedPostUpdate,
    }
}