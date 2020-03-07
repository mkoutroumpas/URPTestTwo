using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public class URPTestTwo : MonoBehaviour
{
    private EntityManager _entityManager;

    [SerializeField]
    public bool UseJobSystem = true;
    [SerializeField]
    public GameObject Prefab;
    [SerializeField]
    public int NumberOfObjects = 1000;
    [SerializeField]
    public int XSpread = 10;
    [SerializeField]
    public int YSpread = 10;
    [SerializeField]
    public int ZSpread = 10;

    private void Start()
    {
        var settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, null);
        _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        var entity = GameObjectConversionUtility.ConvertGameObjectHierarchy(Prefab, settings);

        for (var i = 0; i < NumberOfObjects; i++)
        {
            var position = new Vector3(
                Random.Range(-XSpread, XSpread),
                Random.Range(-YSpread, YSpread),
                Random.Range(-ZSpread, ZSpread));

            var entityInstance = _entityManager.Instantiate(entity);

            _entityManager.AddComponent(entityInstance, typeof(CubeComponent));
            _entityManager.AddComponentData(entityInstance, new MoveSpeedComponentData { Value = Random.Range(0.1f, 1.0f) });
            _entityManager.SetComponentData(entityInstance, new Translation { Value = position });
        }
    }
}
