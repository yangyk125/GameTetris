using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using TMPro;
using UnityEngine;

namespace GameTetris
{
    public class GameObjectPool
    {
        private GameObject _root;
        private GameObject _prefab;
        private Queue<GameObject> _pool = new();

        public GameObjectPool(GameObject root, GameObject prefab)
        {
            _root = root;
            _prefab = prefab;
        }

        public GameObject Acquire()
        {
            GameObject obj = null;
            if (_pool.Count > 0)
            {
                obj = _pool.Dequeue();
                obj.SetActive(true);
            }
            else
            {
                obj = Object.Instantiate<GameObject>(_prefab);
            }
            return obj;
        }

        public void Release(GameObject obj)
        {
            obj.transform.parent = _root.transform;
            obj.SetActive(false);
            _pool.Enqueue(obj);
        }
    }
}

