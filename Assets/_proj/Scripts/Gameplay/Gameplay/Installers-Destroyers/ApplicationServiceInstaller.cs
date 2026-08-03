using ESF.Core.DataRepository;
using UnityEngine;
using ESF.Core.Services;
using ESF.Core.UpdateScheduler;
using ESF.Misc.Billboarding;
using ESF.UI.ScreenFade;
using Game.LevelSystem;

namespace Game.Main
{
    [DefaultExecutionOrder(-200)]
    public class ApplicationServiceInstaller : MonoBehaviour
    {
        [SerializeField] private ScreenFadeService _screenFadeService;
        [SerializeField] private Transform _billboardTarget;
        [SerializeField] private LevelConfigCollection _levelConfigCollection;
        [SerializeField] private Camera _mainCamera;

        private void Awake()
        {
            Application.targetFrameRate = 240;

            Service.Register<Camera>(_mainCamera);

            Service.Register<ScreenFadeService>(_screenFadeService);

            var levelService = new LevelService(_levelConfigCollection.LevelConfigs);
            Service.Register<LevelService>(levelService);

            UpdateService updateService = new UpdateService();
            Service.Register<UpdateService>(updateService);

            var billboardService = new BillboardService(Service.ReadonlyServiceContainer);
            billboardService.SetTarget(_billboardTarget);
            Service.Register<BillboardService>(billboardService);

            var repositoryService = new RepositoryService(updateService, 10);
            Service.Register<RepositoryService>(repositoryService);
        }
    }
}