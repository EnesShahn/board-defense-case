using System.Collections.Generic;
using UnityEngine;
using System;
using ESF.Core.UpdateScheduler;

namespace ESF.Core.DataRepository
{
    /// <summary>\
    /// Features:
    /// Auto Save
    /// Sync Save and Sync Load (Unit Of Work)
    /// Holds all repositories in single service (simplifies usage instead of getting each repository separately you use one)
    /// </summary>
    public class RepositoryService
    {
        private Dictionary<Type, BaseRepository> _syncRepositories;
        private float _autoSaveInterval;

        private UpdateService _updateService;
        private float _currentAutoSaveTimer;

        public RepositoryService(UpdateService updateService, float autoSaveFrequency)
        {
            _syncRepositories = new();
            _updateService = updateService;
            _autoSaveInterval = autoSaveFrequency;

            _updateService.OnUpdate += OnUpdate;
            Application.focusChanged += FocusChanged;
            Application.quitting += Quitting;
        }
        public void Deinitialize()
        {
            _updateService.OnUpdate -= OnUpdate;
            Application.focusChanged -= FocusChanged;
            Application.quitting -= Quitting;
        }

        private void FocusChanged(bool focused)
        {
            SyncSave();
            WriteToDisk();
        }
        private void Quitting()
        {
            SyncSave();
            WriteToDisk();
        }

        private void OnUpdate()
        {
            _currentAutoSaveTimer += Time.deltaTime;
            if (_currentAutoSaveTimer >= _autoSaveInterval)
            {
                _currentAutoSaveTimer = 0;
                SyncSave();
            }
        }

        public bool AddRepositoryForSync<T>(T repository) where T : BaseRepository
        {
            Type t = typeof(T);
            if (_syncRepositories.ContainsKey(t))
                return false;
            _syncRepositories.Add(t, repository);
            return true;
        }

        public bool DeleteRepositoryFromSync<T>(T repository) where T : BaseRepository
        {
            Type t = typeof(T);
            if (!_syncRepositories.ContainsKey(t))
                return false;
            _syncRepositories.Remove(t);
            return true;
        }

        public T GetSyncedRepository<T>() where T : BaseRepository
        {
            Type t = typeof(T);
            if (!_syncRepositories.ContainsKey(t))
                return null;
            return (T)_syncRepositories[typeof(T)];
        }

        public void SyncSave()
        {
            foreach (var pair in _syncRepositories)
            {
                pair.Value.Save();
            }
        }

        public void WriteToDisk()
        {
            PlayerPrefs.Save();
        }
    }
}