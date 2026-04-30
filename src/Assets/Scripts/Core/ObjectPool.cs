using UnityEngine;
using System.Collections.Generic;

namespace CaudilloBay.Core
{
    public class ObjectPool : MonoBehaviour
    {
        public static ObjectPool Instance { get; private set; }

        private Dictionary<string, Queue<GameObject>> pools = new Dictionary<string, Queue<GameObject>>();

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        public GameObject Get(GameObject prefab, Vector3 position, Quaternion rotation)
        {
            string key = prefab.name;
            if (!pools.ContainsKey(key)) pools[key] = new Queue<GameObject>();

            GameObject obj;
            if (pools[key].Count > 0)
            {
                obj = pools[key].Dequeue();
                obj.SetActive(true);
                obj.transform.position = position;
                obj.transform.rotation = rotation;
            }
            else
            {
                obj = Instantiate(prefab, position, rotation);
            }
            return obj;
        }

        public void ReturnToPool(GameObject obj)
        {
            string key = obj.name.Replace("(Clone)", "");
            obj.SetActive(false);
            if (!pools.ContainsKey(key)) pools[key] = new Queue<GameObject>();
            pools[key].Enqueue(obj);
        }
    }
}
