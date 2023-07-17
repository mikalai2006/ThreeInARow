using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Loader;
using UnityEngine;
using UnityEngine.UI;

public class UIDashboard : MonoBehaviour
{
  private GameManager _gameManager => GameManager.Instance;
  [SerializeField] private Button _buttonRun;
  [SerializeField] private TMPro.TextMeshProUGUI _buttonRunText;
  private TaskCompletionSource<DataDialogResult> _processCompletionSource;
  private DataDialogResult _result;

  private void Awake()
  {
    _buttonRun.image.color = _gameManager.Theme.colorBgButton;
    _buttonRunText.color = _gameManager.Theme.colorTextInput;

    _buttonRun.onClick.AddListener(StartGame);
  }

  private void OnDestroy()
  {
    _buttonRun.onClick.RemoveListener(StartGame);
  }

  public async UniTask<DataDialogResult> ProcessAction()
  {
    _result = new DataDialogResult();

    _processCompletionSource = new TaskCompletionSource<DataDialogResult>();

    return await _processCompletionSource.Task;
  }

  private async void StartGame()
  {
    AudioManager.Instance.Click();
    _result.isOk = true;

    _processCompletionSource.SetResult(_result);

    var operations = new Queue<ILoadingOperation>();
    operations.Enqueue(new GameInitOperation());
    await _gameManager.LoaderBarProvider.LoadAndDestroy(operations);


    Debug.Log("Start game");
  }
}
