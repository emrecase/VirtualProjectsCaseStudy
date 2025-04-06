using System.Collections;
using DG.Tweening;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(Collider), typeof(Rigidbody))]
public class Unit : MonoBehaviour, IDamageable, ITeam
{
    // Değişkenler
    [Header("Unit Settings")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float attackPower = 10f;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private Enums.TeamType teamType;
    
    [Header("Movement Settings")]
    [SerializeField] private Enums.MovementType movementType = Enums.MovementType.Linear;
    
    [Header("Effects")]
    [SerializeField] private ParticleSystem hitParticle;
    [SerializeField] private ParticleSystem deathParticle;
    [SerializeField] private AudioClip hitSound;
    [SerializeField] private AudioClip deathSound;
    
    private float currentHealth;
    private bool isDead = false;
    private IMovable movementSystem;
    private Collider unitCollider;
    private Rigidbody unitRigidbody;
    
    [InjectOptional] private IUnitPool pool;
    
    public Enums.TeamType Team => teamType;
    public float CurrentHealth => currentHealth;
    public bool IsAlive => !isDead;

    private void Awake()
    {
        unitCollider = GetComponent<Collider>();
        unitRigidbody = GetComponent<Rigidbody>();
        
        InitializeMovementSystem();
        InitializePhysics();
    }

    private void InitializeMovementSystem()
    {
        switch (movementType)
        {
            case Enums.MovementType.Linear:
                movementSystem = new LinearMovement(transform, moveSpeed);
                break;
            case Enums.MovementType.Smooth:
                movementSystem = new SmoothMovement(transform, moveSpeed);
                break;
        }
    }

    private void InitializePhysics()
    {
        currentHealth = maxHealth;
        unitRigidbody.isKinematic = true;
        unitCollider.isTrigger = true;
    }

    public void Initialize(Vector3 startPosition, Vector3 moveDirection)
    {
        transform.position = startPosition;
        movementSystem.Move(moveDirection);
        isDead = false;
        currentHealth = maxHealth;
        unitCollider.enabled = true;
        gameObject.SetActive(true);
        transform.localScale = Vector3.one;
    }

    private void Update()
    {
        if (isDead) return;
        
        if (movementSystem is LinearMovement linearMovement)
        {
            linearMovement.Update();
        }
        else if (movementSystem is SmoothMovement smoothMovement)
        {
            smoothMovement.Update();
        }
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;
        
        currentHealth -= damage;
        PlayHitEffects();
        
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void PlayHitEffects()
    {
        if (hitParticle != null)
            hitParticle.Play();
        
        if (hitSound != null)
            AudioSource.PlayClipAtPoint(hitSound, transform.position);
    }

    private void Die()
    {
        isDead = true;
        unitCollider.enabled = false;
        movementSystem.Stop();
        
        PlayDeathEffects();
        StartCoroutine(DisappearCoroutine());
    }

    private void PlayDeathEffects()
    {
        if (deathParticle != null)
            deathParticle.Play();
            
        if (deathSound != null)
            AudioSource.PlayClipAtPoint(deathSound, transform.position);
    }

    private IEnumerator DisappearCoroutine()
    {
        yield return transform.DOScale(Vector3.zero, 0.3f).WaitForCompletion();
        
        if (pool != null)
            pool.ReturnUnit(this.gameObject);
        else
            Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isDead) return;
        
        var damageable = other.GetComponent<IDamageable>();
        var team = other.GetComponent<ITeam>();
        
        if (damageable != null && team != null && team.Team != this.Team)
        {
            damageable.TakeDamage(attackPower);
        }
    }
}