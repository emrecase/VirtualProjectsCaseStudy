using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [SerializeField] private BattleManager battleManager;
    [SerializeField] private ArmyController playerArmy;
    [SerializeField] private ArmyController enemyArmy;
    
    public override void InstallBindings()
    {
        // BattleManager'ı Singleton olarak bağla
        Container.BindInstance(battleManager).AsSingle();
        
        // ArmyController'ları ID'lerle bağla
        Container.BindInstance(playerArmy).WithId("PlayerArmy").WhenInjectedInto<BattleManager>();
        Container.BindInstance(enemyArmy).WithId("EnemyArmy").WhenInjectedInto<BattleManager>();
        
        // UnitPool'leri bağla (eğer kullanıyorsanız)
        Container.Bind<UnitPooler>().FromComponentInHierarchy().AsSingle();
    }
}