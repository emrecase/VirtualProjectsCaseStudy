using UnityEngine;

public interface IUnitPool
{
    GameObject GetUnit(Vector3 position, Quaternion rotation);
    void ReturnUnit(GameObject unit);
}