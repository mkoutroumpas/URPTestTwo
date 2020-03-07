using Assets.Scripts.Classes;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class URPTestTwo : MonoBehaviour
{
    private List<ZMoveableObject> _moveableObjects;
    private EntityManager _entityManager;

    [SerializeField]
    public bool UseJobSystem = true;
    [SerializeField]
    public GameObject ObjectToMove;
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
        _moveableObjects = new List<ZMoveableObject>();

        var settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, null);
        _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        var entity = GameObjectConversionUtility.ConvertGameObjectHierarchy(ObjectToMove, settings);

        for (var i = 0; i < NumberOfObjects; i++)
        {
            var position = new Vector3(
                UnityEngine.Random.Range(-XSpread, XSpread),
                UnityEngine.Random.Range(-YSpread, YSpread),
                UnityEngine.Random.Range(-ZSpread, ZSpread));

            var entityInstance = _entityManager.Instantiate(entity);
            _entityManager.SetComponentData(entityInstance, new Translation { Value = position });

            _moveableObjects.Add(new ZMoveableObject
            {
                Entity = entityInstance,
                Position = position,
                Velocity = UnityEngine.Random.Range(0.1f, 1.0f)
            });
        }
    }

    private void Update()
    {
        float startTime = Time.realtimeSinceStartup;

        if (UseJobSystem)
        {
            var _velocities = new NativeArray<float>(_moveableObjects.Count, Allocator.TempJob);
            var _positions = new NativeArray<float3>(_moveableObjects.Count, Allocator.TempJob);

            for (var i = 0; i < _moveableObjects.Count; i++)
            {
                _positions[i] = _moveableObjects[i].Position;
                _velocities[i] = _moveableObjects[i].Velocity;
            }

            var job = new ZTranslationJob
            {
                MoveSpeeds = _velocities,
                Positions = _positions,
                DeltaTime = Time.deltaTime,
                MinusDirection = true
            };

            var jobHandle = job.Schedule(_moveableObjects.Count, 10);
            jobHandle.Complete();

            for (var i = 0; i < _moveableObjects.Count; i++)
            {
                _moveableObjects[i].Position = _positions[i];

                _entityManager.SetComponentData(_moveableObjects[i].Entity, new Translation { Value = _moveableObjects[i].Position });
            }
            
            _velocities.Dispose();
            _positions.Dispose();
        }
        else
        {
            for (var i = 0; i < _moveableObjects.Count; i++)
            {
                _moveableObjects[i].Position += new Vector3(0f, 0f, -_moveableObjects[i].Velocity * Time.deltaTime);
                Helpers.AddDummyHeavyTask();
            }
        }

        Debug.Log(((Time.realtimeSinceStartup - startTime) * 1000f) + "ms");
    }

    public class ZMoveableObject
    {
        public Entity Entity;
        public Vector3 Position;
        public float Velocity;
    }
}
