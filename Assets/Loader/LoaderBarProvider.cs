using System.Collections.Generic;
using Assets;
using Cysharp.Threading.Tasks;
using Loader;

public class LoaderBarProvider : LocalAssetLoader
{
  public async UniTask LoadAndDestroy(Queue<ILoadingOperation> loadingOperations)
  {
    var loadingScreen = await Load();
    await loadingScreen.Load(loadingOperations);
    Unload();
  }

  public UniTask<LoaderBar> Load()
  {
    return LoadInternal<LoaderBar>("LoaderBar");
  }

  public void Unload()
  {
    UnloadInternal();
  }
}