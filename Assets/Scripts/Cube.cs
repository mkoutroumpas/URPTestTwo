using Unity.Entities;
using UnityEngine;

public class Cube : MonoBehaviour, IConvertGameObjectToEntity
{
    public void Convert(Entity entity, EntityManager entityManager, GameObjectConversionSystem conversionSystem)
    {
        entityManager.AddComponent(entity, typeof(CubeComponent));
        entityManager.AddComponentData(entity, new MoveSpeedComponentData { Value = Random.Range(0.1f, 1.0f) });
    }
}
