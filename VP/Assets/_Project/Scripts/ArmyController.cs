using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ArmyController : MonoBehaviour
{
    
    
    [Inject] private DiContainer container;
    
    [SerializeField] private Enums.ArmyFormationType formationType = Enums.ArmyFormationType.Line;
    [SerializeField] private int unitCount = 20;
    [SerializeField] private float unitSpacing = 1.5f;
    [SerializeField] private Vector3 moveDirection = new Vector3(0, 0, 1);
    
    [SerializeField] private UnitPool pooler;
    
    private List<GameObject> activeUnits = new List<GameObject>();
    
    public void SpawnArmy(Vector3 startPosition)
    {
        ClearArmy();
        
        for (int i = 0; i < unitCount; i++)
        {
            Vector3 spawnPosition = CalculateFormationPosition(startPosition, i);
            
            // UnitPool üzerinden birim oluştur
            GameObject unitObj = pooler.GetUnit(spawnPosition, Quaternion.identity);
        
            // Veya direkt olarak yeni birim oluştur
            // GameObject unitObj = container.InstantiatePrefab(unitPrefab, startPosition, Quaternion.identity, null);
        
            Unit unit = unitObj.GetComponent<Unit>();
            unit.Initialize(spawnPosition, moveDirection);
            
            
            if (unit != null)
            {
                activeUnits.Add(unitObj);
            }
        }
    }
    
    private Vector3 CalculateFormationPosition(Vector3 startPosition, int unitIndex)
    {
        switch (formationType)
        {
            case Enums.ArmyFormationType.Line:
                return startPosition + new Vector3(unitIndex * unitSpacing, 0, 0);
                
            case Enums.ArmyFormationType.Square:
                int rowSize = Mathf.CeilToInt(Mathf.Sqrt(unitCount));
                int row = unitIndex / rowSize;
                int col = unitIndex % rowSize;
                return startPosition + new Vector3(col * unitSpacing, 0, row * unitSpacing);
                
            case Enums.ArmyFormationType.Circle:
                float angle = unitIndex * (2f * Mathf.PI / unitCount);
                float radius = unitCount * unitSpacing / (2f * Mathf.PI);
                return startPosition + new Vector3(
                    Mathf.Cos(angle) * radius, 
                    0, 
                    Mathf.Sin(angle) * radius
                );
                
            case Enums.ArmyFormationType.Triangle:
                // Üçgen formasyonu hesaplaması
                break;
        }
        
        return startPosition;
    }
    
    public void MoveArmy(Vector3 direction)
    {
        foreach (GameObject unit in activeUnits)
        {
            if (unit.activeInHierarchy)
            {
                // Unit scriptinde hedef pozisyon güncellenebilir
                Unit unitComponent = unit.GetComponent<Unit>();
                // Hareket mantığı Unit.cs'de işlenecek
            }
        }
    }
    
    public void ClearArmy()
    {
        foreach (GameObject unit in activeUnits)
        {
            pooler.ReturnUnit(unit);
        }
        activeUnits.Clear();
    }
    
    public int GetActiveUnitCount()
    {
        int count = 0;
        foreach (GameObject unit in activeUnits)
        {
            if (unit.activeInHierarchy) count++;
        }
        return count;
    }
}