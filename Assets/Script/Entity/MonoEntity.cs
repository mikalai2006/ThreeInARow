using UnityEngine;

public class MonoEntity : MonoBehaviour
{
  private BaseEntity _entityClass;
  public BaseEntity EntityClass => _entityClass;

  [SerializeField] private SpriteRenderer _spriteEntity;

  public void Init(BaseEntity baseEntity)
  {
    _entityClass = baseEntity;

    _spriteEntity.sprite = EntityClass.configEntity.sprite;
  }
}
