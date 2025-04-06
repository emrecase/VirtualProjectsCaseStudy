using System.Collections.Generic;
using UnityEngine;

public class UnitPool : MonoBehaviour, IUnitPool
{
    [SerializeField] private GameObject unitPrefab;
    [SerializeField] private int initialPoolSize = 20;
    
    private Queue<GameObject> pool = new Queue<GameObject>();
    
    private void Awake()
    {
        for (int i = 0; i < initialPoolSize; i++)
        {
            CreateNewUnit();
        }
    }
    
    private GameObject CreateNewUnit()
    {
        GameObject newUnit = Instantiate(unitPrefab);
        newUnit.SetActive(false);
        newUnit.transform.SetParent(transform);
        pool.Enqueue(newUnit);
        return newUnit;
    }
    
    public GameObject GetUnit(Vector3 position, Quaternion rotation)
    {
        if (pool.Count == 0)
        {
            CreateNewUnit();
        }
        
        GameObject unit = pool.Dequeue();
        unit.transform.position = position;
        unit.transform.rotation = rotation;
        unit.SetActive(true);
        return unit;
    }
    
    public void ReturnUnit(GameObject unit)
    {
        unit.SetActive(false);
        pool.Enqueue(unit);
    }
}