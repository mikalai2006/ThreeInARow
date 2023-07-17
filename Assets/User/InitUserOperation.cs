using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Loader;
using UnityEngine.Localization.Settings;
using System.Threading.Tasks;

namespace User
{
  public class InitUserOperation : ILoadingOperation
  {
    private AppInfoContainer _playPrefData;
    private TaskCompletionSource<UserInfo> _getUserInfoCompletionSource;
    private TaskCompletionSource<StateGame> _getUserDataCompletionSource;
    private TaskCompletionSource<UserSettings> _getUserSettingCompletionSource;
    private Action<float> _onProgress;
    private Action<string> _onSetNotify;

    public async UniTask Load(Action<float> onProgress, Action<string> onSetNotify)
    {
      var _gameManager = GameManager.Instance;

      _playPrefData = new();

      _onProgress = onProgress;
      _onSetNotify = onSetNotify;

      // var t = await Helpers.GetLocaledString("loading");
      _onSetNotify?.Invoke("...");
      _onProgress?.Invoke(0.3f);

      string namePlaypref = GameManager.Instance.KeyPlayPref;

      GameManager.Instance.DataManager.Init(namePlaypref);

      if (PlayerPrefs.HasKey(namePlaypref))
      {
        _playPrefData = JsonUtility.FromJson<AppInfoContainer>(PlayerPrefs.GetString(namePlaypref));
      }
      else
      {
        _playPrefData.uid = Convert.ToBase64String(Guid.NewGuid().ToByteArray()); //DeviceInfo.GetDeviceId();

        await LocalizationSettings.InitializationOperation.Task;
        var setting = new UserSettings()
        {
          auv = _gameManager.GameSettings.Audio.volumeEffect,
          lang = LocalizationSettings.SelectedLocale.name,
          muv = _gameManager.GameSettings.Audio.volumeMusic,
          theme = _gameManager.GameSettings.ThemeDefault.name,
          dod = true,
          td = _gameManager.GameSettings.timeDelayOverChar // time delay
        };
        _playPrefData.setting = setting;
      }

      // Get user info.
      DataManager.OnLoadUserInfo += SetUserInfo;
      _playPrefData.UserInfo = await GetUserInfo();
      DataManager.OnLoadUserInfo -= SetUserInfo;

      // Get game data.
      DataManager.OnLoadData += InitData;
      var dataGame = await GetUserData();
      DataManager.OnLoadData -= InitData;

      await GameManager.Instance.SetAppInfo(_playPrefData);
      // Set user setting to PlayPref.
      // string jsonData = JsonUtility.ToJson(_playPrefData);
      // PlayerPrefs.SetString(namePlaypref, jsonData);


      _onProgress?.Invoke(.5f);
    }


    private void SetUserInfo(UserInfo userInfo)
    {
      _playPrefData.UserInfo = userInfo;
      _getUserInfoCompletionSource.SetResult(userInfo);
    }


    public async Task<UserInfo> GetUserInfo()
    {
      _getUserInfoCompletionSource = new TaskCompletionSource<UserInfo>();

#if android
      UserInfo userInfo = _playPrefData.UserInfo;
      if (string.IsNullOrEmpty(userInfo.name))
      {
        // Load form for input name.
        var result = await GameManager.Instance.InitUserProvider.ShowAndHide();
        userInfo = result.UserInfo;
      }
      SetUserInfo(userInfo);
#endif

#if ysdk
      GameManager.Instance.DataManager.LoadUserInfoAsYsdk();
#endif

      return await _getUserInfoCompletionSource.Task;
    }


    private async UniTask<StateGame> GetUserData()
    {
      _getUserDataCompletionSource = new();

#if android
      StateGame stateGame;
      stateGame = await GameManager.Instance.DataManager.Load();
#endif

#if ysdk
      GameManager.Instance.DataManager.LoadAsYsdk();
#endif

      return await _getUserDataCompletionSource.Task;
    }

    private async void InitData(StateGame stateGame)
    {
      var _gameManager = GameManager.Instance;

      stateGame = await _gameManager.StateManager.Init(stateGame);

      _getUserDataCompletionSource.SetResult(stateGame);
    }

  }
}