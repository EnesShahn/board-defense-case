using Cysharp.Threading.Tasks;
using ESF.Core.DataRepository;
using ESF.Core.Services;
using ESF.UI.ScreenFade;
using Game.Bases;
using Game.LevelSystem;
using Game.WaveSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Main
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private bool _overrideLevel;
        [SerializeField] private int _overrideLevelIndex;
        [SerializeField] private float _fadeInOutTime = 0.5f;

        [SerializeField] private Canvas _endLevelCanvas;
        [SerializeField] private TMP_Text _endText;
        [SerializeField] private Button _nextLevelButton;
        [SerializeField] private Button _retryLevelButton;
        [SerializeField] private Button _startFromZeroButton;

        private LevelService _levelService;
        private RepositoryService _repositoryService;
        private GameRepository _gameRepository;
        private ScreenFadeService _screenFadeService;

        private void Awake()
        {
            _levelService = Service.Resolve<LevelService>();
            _repositoryService = Service.Resolve<RepositoryService>();
            _screenFadeService = Service.Resolve<ScreenFadeService>();
            _gameRepository = Service.Resolve<GameRepository>();

            _screenFadeService.CancelAndSetState(true);

            if (_overrideLevel)
                SwitchLevel(_overrideLevelIndex);
            else
                SwitchLevel(_gameRepository.Data.CurrentLevelIndex);

            _endLevelCanvas.gameObject.SetActive(false);
            _nextLevelButton.onClick.AddListener(OnNextLevelButtonClicked);
            _retryLevelButton.onClick.AddListener(OnRetryLevelButtonClicked);
            _startFromZeroButton.onClick.AddListener(OnStartFromZeroButtonClicked);
        }


        private async UniTaskVoid SwitchLevel(int levelIndex)
        {
            await _screenFadeService.DOFadeIn(_fadeInOutTime);

            if (_levelService.ActiveLevel != null)
                _levelService.DestroyActiveLevel();
            await UniTask.WaitForSeconds(0.1f);

            var newLevel = _levelService.CreateLevel(levelIndex);
            NewLevelListenSetup(newLevel);
            await UniTask.WaitForSeconds(0.1f);

            await _screenFadeService.DOFadeOut(_fadeInOutTime);
        }


        private void NewLevelListenSetup(GameObject newLevel)
        {
            //TODO: improve the locating of dependency
            var newLevelWaveController = newLevel.GetComponent<WaveController>();
            var newLevelBaseController = newLevel.GetComponentInChildren<BaseRoot>();

            newLevelWaveController.AllWavesCompleted += OnActiveLevelAllWavesCompleted;
            var baseHealthController = newLevelBaseController.GetComponentInChildren<BaseHealthController>();

            baseHealthController.HealthReachedZero += OnBaseHealthReachedZero;
        }


        private void OnBaseHealthReachedZero()
        {
            _endText.text = "Level Failed";
            _nextLevelButton.gameObject.SetActive(false);
            _retryLevelButton.gameObject.SetActive(true);
            _startFromZeroButton.gameObject.SetActive(false);
            _endLevelCanvas.gameObject.SetActive(true); //TODO Animate
        }
        private void OnActiveLevelAllWavesCompleted()
        {
            if (_levelService.ActiveLevelIndex + 1 >= _levelService.LevelConfigs.Count) // no next level
            {
                _endText.text = "Game Completed";
                _nextLevelButton.gameObject.SetActive(false);
                _retryLevelButton.gameObject.SetActive(false);
                _startFromZeroButton.gameObject.SetActive(true);
                _endLevelCanvas.gameObject.SetActive(true);
            }
            else
            {
                _endText.text = "Level Completed";
                _nextLevelButton.gameObject.SetActive(true);
                _retryLevelButton.gameObject.SetActive(false);
                _startFromZeroButton.gameObject.SetActive(false);
                _endLevelCanvas.gameObject.SetActive(true);
            }
        }

        private void OnNextLevelButtonClicked()
        {
            int nextLevel = _levelService.ActiveLevelIndex + 1;
            _endLevelCanvas.gameObject.SetActive(false);
            _gameRepository.Data.CurrentLevelIndex = nextLevel;
            _repositoryService.SyncSave();
            SwitchLevel(nextLevel);
        }
        private void OnRetryLevelButtonClicked()
        {
            _endLevelCanvas.gameObject.SetActive(false);
            SwitchLevel(_levelService.ActiveLevelIndex);
        }
        private void OnStartFromZeroButtonClicked()
        {
            _endLevelCanvas.gameObject.SetActive(false);
            _gameRepository.Data.CurrentLevelIndex = 0;
            _repositoryService.SyncSave();
            SwitchLevel(0);
        }
    }
}