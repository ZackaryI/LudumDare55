using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class ObjectPoolItem
{
    public int amountToPool;
    public GameObject objectToPool;
    public GameObject parentOfObject;
    public bool shouldExpand;

}

/// <summary>
/// Create set of gameObjects at start of scene to use.
/// Keep at one instance for objectPool, as the reference is static and used scene-wide. 
/// </summary>
public class ObjectPool : MonoBehaviour
{
    /// <summary>
    /// Refer to this instance of objectPool for the scene
    /// </summary>
    public static ObjectPool sharedInstance;
    public List<GameObject> pooledObjects;
    public List<ObjectPoolItem> itemsToPool;

    /// <summary>
    /// 
    /// </summary>
    private void Awake()
    {
        sharedInstance = this;
    }

    private void Start()
    {
        pooledObjects = new List<GameObject>();
        foreach (ObjectPoolItem item in itemsToPool)
        {
            for (int i = 0; i < item.amountToPool; i++)
            {
                GameObject obj = (GameObject)Instantiate(item.objectToPool, item.parentOfObject.transform);
                obj.SetActive(false);
                pooledObjects.Add(obj);
            }
        }
    }

    public GameObject GetPooledObject(string tag)
    {
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].activeSelf && pooledObjects[i].CompareTag(tag))
            {
                return pooledObjects[i];
            }
        }
        foreach (ObjectPoolItem item in itemsToPool)
        {
            //We look up the item through tags
            if (item.objectToPool.CompareTag(tag))
            {
                if (item.shouldExpand)
                {
                    GameObject obj = (GameObject)Instantiate(item.objectToPool, item.parentOfObject.transform);
                    obj.SetActive(false);
                    pooledObjects.Add(obj);
                    return obj;
                }
            }
        }
        return null;
    }
}
