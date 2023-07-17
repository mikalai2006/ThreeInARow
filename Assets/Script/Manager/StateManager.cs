using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class StateManager : MonoBehaviour
{
  public static event Action<StateGame> OnChangeState;
  public static event Action OnGenerateBonus;
  // public static event Action<GameTheme> OnChangeUserSetting;
  public DataGame dataGame;
  public StateGame stateGame;
  public string ActiveWordConfig;

  private LevelManager _levelManager => GameManager.Instance.LevelManager;
  private GameManager _gameManager => GameManager.Instance;
  private GameSetting _gameSetting => GameManager.Instance.GameSettings;

  public async UniTask<StateGame> Init(StateGame _stateGame)
  {
    await LocalizationSettings.InitializationOperation.Task;
    string codeCurrentLang = LocalizationSettings.SelectedLocale.Identifier.Code;

    GamePlayerSetting playerSetting;
    DataGame dataGame;
    StateGameItem stateGameItemCurrentLang = _stateGame != null && _stateGame.items.Count > 0
      ? _stateGame.items.Find(t => t.lang == codeCurrentLang)
      : null;

    // init new state.
    if (_stateGame == null) _stateGame = new();


    return _stateGame;
  }


  public StateGame GetData()
  {
    return stateGame;
  }

  public void SetLastTime()
  {
    stateGame.lastTime = System.DateTime.Now.ToString();
  }

  public async UniTask Reset()
  {
    _gameManager.StateManager.stateGame = new StateGame();
    await _gameManager.StateManager.Init(null);

    OnChangeState?.Invoke(stateGame);
  }
}
