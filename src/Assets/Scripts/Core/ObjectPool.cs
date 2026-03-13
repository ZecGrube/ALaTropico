using UnityEngine;
using System.Collections.Generic;

namespace CaudilloBay.Core
{
    public class ObjectPool : MonoBehaviour
    {
        public static ObjectPool Instance { get; private set; }

        private Dictionary<string, Queue<GameObject>> poolDictionary = new Dictionary<string, Queue<GameObject>>();

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        public void CreatePool(string key, GameObject prefab, int initialSize)
        {
            if (!poolDictionary.ContainsKey(key))
            {
                poolDictionary.Add(key, new Queue<GameObject>());

                for (int i = 0; i < initialSize; i++)
                {
                    GameObject obj = Instantiate(prefab);
                    obj.SetActive(false);
                    poolDictionary[key].Enqueue(obj);
                }
            }
        }

        public GameObject SpawnFromPool(string key, Vector3 position, Quaternion rotation)
        {
            if (!poolDictionary.ContainsKey(key))
            {
                Debug.LogWarning($"Pool with key {key} doesn't exist.");
                return null;
            }

            GameObject objToSpawn = poolDictionary[key].Count > 0 ? poolDictionary[key].Dequeue() : null;

            if (objToSpawn == null)
            {
                // Optionally expand pool here
                return null;
            }

            objToSpawn.SetActive(true);
            objToSpawn.transform.position = position;
            objToSpawn.transform.rotation = rotation;

            return objToSpawn;
        }

        public void ReturnToPool(string key, GameObject obj)
        {
            obj.SetActive(false);
            poolDictionary[key].Enqueue(obj);
        }
    }
}
