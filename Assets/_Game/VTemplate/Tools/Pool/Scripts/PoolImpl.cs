using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace VTemplate
{
    [Serializable]
    public class PoolImpl<T> : IPool<T> where T : Component
    {
        public event PoolEvent<T> OnObjectPicked;
        public event PoolEvent<T> OnObjectReturned;

        public T Prefab { get; }
        public int NumberToInstantiate { get; }

        private MonoBehaviour _parent;
        private GameObject _poolParent, _pickedParent, _inPoolParent;
        private Queue<T> _pool;
        private List<T> _picked;
        private bool _isInit = false;


        public PoolImpl(T prefab, int numberToInstantiate, MonoBehaviour parent)
        {
            Precondition.CheckNotNull(prefab);
            Precondition.CheckNotNull(parent);

            Prefab = prefab;
            NumberToInstantiate = numberToInstantiate;
            _parent = parent;
            Init();
        }
        public void Init()
        {
            if (_isInit)
                throw new Exception($"{GetType()} Pool is already Init");

            _poolParent = new GameObject($"{Prefab.name} Pool");
            _pickedParent = new GameObject($"Picked {Prefab.name}");
            _inPoolParent = new GameObject($"In Pool {Prefab.name}");
            _poolParent.transform.SetParent(_parent.transform);
            _pickedParent.transform.SetParent(_poolParent.transform);
            _inPoolParent.transform.SetParent(_poolParent.transform);
            _pool = new Queue<T>();
            _picked = new List<T>();
            _isInit = true;
            BaseInstantiation();
        }
        public T Pick()
        {
            T instance = PickInstance();
            OnObjectPicked?.Invoke(instance);
            return instance;
        }
        public void Return(T instance)
        {
            if (IsInPool(instance))
                return;
            ReturnInstance(instance);
            OnObjectReturned?.Invoke(instance);
        }
        public void ReturnDelayed(T instance, float delay)
        {
            if (IsInPool(instance))
                return;
            _parent.StartCoroutine(ReturnDelayedRoutine(instance, delay));
        }
        public void ReturnAll()
        {
            for (int i = _picked.Count - 1; i >= 0; i--)
                Return(_picked[i]);
        }
        public void Clear()
        {
            ReturnAll();
            GameObject.Destroy(_poolParent);
            _pool.Clear();
            _picked.Clear();
            _isInit = false;
        }
        private void BaseInstantiation()
        {
            List<T> instances = new List<T>();
            for (int i = 0; i < NumberToInstantiate; i++)
                instances.Add(PickInstance());
            for (int i = 0; i < instances.Count; i++)
                ReturnInstance(instances[i]);
        }
        private bool IsInPool(T instance)
        {
            if (_pool.Contains(instance))
                return true;
            return false;
        }

        private T PickInstance()
        {
            if (!_isInit)
                Init();

            T instance;

            if (_pool.Count > 0)
                instance = _pool.Dequeue();
            else
            {
                instance = GameObject.Instantiate(Prefab) as T;
                instance.name = $"{Prefab.name} instance";
            }

            _picked.Add(instance);
            instance.transform.SetParent(_pickedParent.transform);
            instance.gameObject.SetActive(true);
            return instance;
        }

        private void ReturnInstance(T instance)
        {
            if (IsInPool(instance))
                return;

            _pool.Enqueue(instance);
            _picked.Remove(instance);
            instance.transform.SetParent(_inPoolParent.transform);
            instance.gameObject.SetActive(false);
        }

        private IEnumerator ReturnDelayedRoutine(T instance, float delay)
        {
            yield return new WaitForSeconds(delay);
            Return(instance);
        }
    }
}