using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu]
public class GameEntity : ScriptableObject
{
  public string idEntity;
  public TypeEntity typeEntity;
  public TextLocalize text;
  public Sprite sprite;
  [SerializeField] public AssetReferenceGameObject prefab;
  public ParticleSystem activateEffect;
  public ParticleSystem MoveEffect;
  public bool isUseGenerator;
  public AudioClip soundRunEntity;
  public AudioClip soundAddEntity;
}


[System.Serializable]
public enum TypeEntity
{
  None = 0,
  Banan = 1,
  Blueberry = 2,
  Apple = 3,
  Cocount = 4,
  Orange = 5,
  Watermelon = 6
}
