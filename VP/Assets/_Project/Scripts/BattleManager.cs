
using System;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

public class BattleManager : MonoBehaviour
{
    [Inject(Id = "PlayerArmy")] private ArmyController _playerArmy;
    [Inject(Id = "EnemyArmy")] private ArmyController _enemyArmy;
    
    [SerializeField] private Transform _playerSpawnPoint;
    [SerializeField] private Transform _enemySpawnPoint;
    [SerializeField] private float battleDuration = 60f;
    
    private float timer;
    private bool battleStarted;


    private void Start()
    {
        // Null kontrolü ekleyin
        if (_playerArmy == null || _enemyArmy == null)
        {
            Debug.LogError("ArmyController'lar enjekte edilemedi!");
            return;
        }
        
        StartBattle();
    }

    [Button]
    public void StartBattle()
    {
        _playerArmy.SpawnArmy(_playerSpawnPoint.position);
        _enemyArmy.SpawnArmy(_enemySpawnPoint.position);
        
        // Düşman ordusunun yönünü ayarla
        _enemyArmy.MoveArmy(Vector3.back);
        
        timer = battleDuration;
        battleStarted = true;
    }
    
    private void Update()
    {
        if (!battleStarted) return;
        
        timer -= Time.deltaTime;
        
        // Oyun sonu kontrolü
        if (timer <= 0 || _playerArmy.GetActiveUnitCount() == 0 || _enemyArmy.GetActiveUnitCount() == 0)
        {
            EndBattle();
            return;
        }
    }
    
    private void EndBattle()
    {
        battleStarted = false;
        
        int playerCount = _playerArmy.GetActiveUnitCount();
        int enemyCount = _enemyArmy.GetActiveUnitCount();
        
        if (playerCount > enemyCount)
        {
            Debug.Log("Player Wins! Remaining units: " + playerCount);
        }
        else if (enemyCount > playerCount)
        {
            Debug.Log("Enemy Wins! Remaining units: " + enemyCount);
        }
        else
        {
            Debug.Log("Draw! Both armies destroyed.");
        }
    }
}