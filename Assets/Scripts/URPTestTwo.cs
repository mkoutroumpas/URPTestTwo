using Assets.Scripts.Classes;
using System.Collections.Generic;
using UnityEngine;

public class URPTestTwo : MonoBehaviour
{
    private List<ZMoveableObject> _moveableObjects;

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
        _moveableObjects = new List<ZMoveableObject>();

        for (var i = 0; i < NumberOfObjects; i++)
        {
            var position = new Vector3(
                Random.Range(-XSpread, XSpread),
                Random.Range(-YSpread, YSpread),
                Random.Range(-ZSpread, ZSpread));

            var instance = Instantiate(Prefab, position, Quaternion.identity);

            _moveableObjects.Add(new ZMoveableObject
            {
                Instance = instance,
                Position = position,
                Velocity = Random.Range(0.1f, 1.0f)
            });
        }
    }

    private void Update()
    {
        float startTime = Time.realtimeSinceStartup;

        for (var i = 0; i < _moveableObjects.Count; i++)
        {
            _moveableObjects[i].Position += new Vector3(0f, 0f, -_moveableObjects[i].Velocity * Time.deltaTime);

            Helpers.AddDummyHeavyTask();

            _moveableObjects[i].Instance.transform.position = Vector3.MoveTowards(_moveableObjects[i].Instance.transform.position, 
                _moveableObjects[i].Position, _moveableObjects[i].Velocity * Time.deltaTime);
        }

        Debug.Log(((Time.realtimeSinceStartup - startTime) * 1000f) + "ms");
    }

    private class ZMoveableObject
    {
        public GameObject Instance;
        public Vector3 Position;
        public float Velocity;
    }
}
