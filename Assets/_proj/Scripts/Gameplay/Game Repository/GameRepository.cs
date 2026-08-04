using System;
using ESF.Core.DataRepository;
using Newtonsoft.Json;
using UnityEngine;

namespace Game.GameRepositorySystem
{
    public class GameRepository : BaseRepository
    {
        private const string RepositorySaveKey = "GameRepository";
        private JsonSerializerSettings _serializerSettings;
        private string _saveKey;
        private GameRepositoryData _data = new();

        public GameRepositoryData Data => _data;

        public GameRepository(string profileName) : base(profileName)
        {
            _serializerSettings = new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
            _saveKey = _profileName + "." + RepositorySaveKey;
        }

        public override void Load()
        {
            if (PlayerPrefs.HasKey(_saveKey))
            {
                var dataString = PlayerPrefs.GetString(_saveKey);
                var loadedData = JsonConvert.DeserializeObject<GameRepositoryData>(dataString, _serializerSettings);
                if (loadedData != null)
                    _data = loadedData;
            }
        }
        public override void Save()
        {
            var dataString = JsonConvert.SerializeObject(_data, _serializerSettings);
            PlayerPrefs.SetString(_saveKey, dataString);
        }

        [Serializable]
        public class GameRepositoryData
        {
            public int CurrentLevelIndex;
        }
    }
}