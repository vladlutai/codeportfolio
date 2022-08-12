using System.Collections.Generic;
using UnityEngine;

namespace Tools
{
    public abstract class Factory<T> : MonoBehaviour where T : MonoBehaviour
    {
#pragma warning disable 649
        [SerializeField] private GameObject prefab;
        [SerializeField] private Transform parentRectTransform;
        [SerializeField] private List<T> itemList;
#pragma warning restore 649

        protected readonly Queue<T> ItemQueue = new Queue<T>();

        private void Awake()
        {
            Init();
        }

        private void Init()
        {
            foreach (var t in itemList)
            {
                t.gameObject.SetActive(false);
                ItemQueue.Enqueue(t);
            }
            itemList = null;
        }

        protected virtual void Push(T item)
        {
            item.gameObject.SetActive(false);
            ItemQueue.Enqueue(item);
        }
    
        public virtual T Pull()
        {
            T t = ItemQueue.Count > 0 ? ItemQueue.Dequeue() : Instantiate(prefab).GetComponent<T>();
            t.transform.SetParent(parentRectTransform, false);
            t.transform.SetAsLastSibling();
            t.gameObject.SetActive(true);
            return t;
        }
    }
}