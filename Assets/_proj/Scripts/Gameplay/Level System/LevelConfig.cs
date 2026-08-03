using UnityEngine;

namespace Game.LevelSystem
{
    [CreateAssetMenu(menuName = "Game/Level System/Level Config")]
    public class LevelConfig : ScriptableObject
    {
        [SerializeField] private GameObject _prefab;

        public GameObject Prefab => _prefab;
    }
}