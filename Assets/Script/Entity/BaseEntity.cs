
using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

public abstract class BaseEntity
{
  protected StateManager _stateManager => GameManager.Instance.StateManager;
  protected LevelManager _levelManager => GameManager.Instance.LevelManager;
  protected GameSetting _gameSetting => GameManager.Instance.GameSettings;
  protected GameManager _gameManager => GameManager.Instance;
  public GameEntity configEntity { get; protected set; }
  protected Vector3 initScale;
  protected Vector3 initPosition;
  [SerializeField] protected SpriteRenderer spriteRenderer;
  [SerializeField] protected SpriteRenderer spriteBg;
  // [SerializeField] private SortingGroup order;
  [SerializeField] public GameObject counterObject;
  [SerializeField] public TMPro.TextMeshProUGUI counterText;
  public GridNode OccupiedNode;
  // public List<GridNode> nodesForCascade = new();
  private UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<GameObject> asset;
  // [SerializeField] private TMPro.TextMeshProUGUI _countHintText;

  public void SetConfig(GameEntity configEntity)
  {
    this.configEntity = configEntity;
  }

  #region LoadAsset
  public async UniTask CreateEntity()
  {
    AssetReferenceGameObject gameObj = null;
    if (configEntity.prefab.RuntimeKeyIsValid())
    {
      gameObj = configEntity.prefab;
    }

    if (gameObj == null)
    {
      Debug.LogWarning($"Not found mapPrefab {configEntity.name}!");
      return;
    }
    var asset = Addressables.InstantiateAsync(
      gameObj,
      OccupiedNode.position,
      Quaternion.identity,
      _levelManager.tilemapEntities.transform
    );
    await asset.Task;
    asset.Result.GetComponent<MonoEntity>().Init(this);
  }
  #endregion

  public void SetNode(GridNode node)
  {
    OccupiedNode = node;
    node.SetOccupiedEntity(this);
  }
}
