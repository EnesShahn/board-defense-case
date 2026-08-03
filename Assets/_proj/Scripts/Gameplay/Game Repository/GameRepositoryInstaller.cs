using UnityEngine;
using ESF.Core.Services;

[DefaultExecutionOrder(-150)]
public class GameRepositoryInstaller : MonoBehaviour
{
    private void Awake()
    {
        GameRepository gameRepository = new GameRepository("Game");
        gameRepository.Load();
        Service.Register(gameRepository);
    }
}