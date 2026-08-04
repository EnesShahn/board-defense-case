using ESF.Core.DataRepository;
using UnityEngine;
using ESF.Core.Services;

namespace Game.GameRepositorySystem
{
    [DefaultExecutionOrder(-150)]
    public class GameRepositoryInstaller : MonoBehaviour
    {
        private void Awake()
        {
            var repositoryService = Service.Resolve<RepositoryService>();

            GameRepository gameRepository = new GameRepository("Game");
            gameRepository.Load();
            repositoryService.AddRepositoryForSync(gameRepository);
            Service.Register(gameRepository);

            Debug.Log("Game Repository " + gameRepository.Data.CurrentLevelIndex);
        }
    }
}