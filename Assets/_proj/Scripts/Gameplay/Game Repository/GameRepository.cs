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
        private GameSettingsRepositoryData _data;

        public GameSettingsRepositoryData Data => _data;

        public GameRepository(string profileName) : base(profileName)
        {
            _serializerSettings = new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
            _saveKey = _profileName + "." + RepositorySaveKey;

            _data = new();
        }

        public override void Load()
        {
            if (PlayerPrefs.HasKey(_saveKey))
            {
                var dataString = PlayerPrefs.GetString(_saveKey);
                _data = JsonConvert.DeserializeObject<GameSettingsRepositoryData>(dataString, _serializerSettings);

                if (_data == null)
                    _data = new();
            }
        }
        public override void Save()
        {
            var dataString = JsonConvert.SerializeObject(_data, _serializerSettings);
            PlayerPrefs.SetString(_saveKey, dataString);
        }

        [Serializable]
        public class GameSettingsRepositoryData
        {
            public int CurrentLevelIndex;
        }
    }
}