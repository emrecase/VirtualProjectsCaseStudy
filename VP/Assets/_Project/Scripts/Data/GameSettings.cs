using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "Settings/GameSettings")]
public class GameSettings : ScriptableObject
{
    [Header("Unit Settings")]
    public float playerUnitHealth = 100f;
    public float playerUnitDamage = 10f;
    public float enemyUnitHealth = 80f;
    public float enemyUnitDamage = 15f;
    
    [Header("Army Settings")]
    public int initialPlayerUnits = 20;
    public int initialEnemyUnits = 25;
}