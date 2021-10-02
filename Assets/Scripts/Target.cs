using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
public class Target : MonoBehaviour
{
    [SerializeField] private float _maxHealth = 100f;
    [SerializeField] private float _currentHealth = 100f;
    [SerializeField] private AudioClip _deathClip;
    [SerializeField] private AudioClip _takeDamageClip;
    [SerializeField] private Heathbar _heathbar;

    private Animator _animator;
    private AudioSource _enemyAudio;
    private ParticleSystem _particleSystem;
    private Collider _collider;
    private bool isDead;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _enemyAudio = GetComponent<AudioSource>();
        _particleSystem = GetComponentInChildren<ParticleSystem>();
        _collider = GetComponent<Collider>();
    }

    private void Start()
    {
        _currentHealth = _maxHealth;
        _heathbar.SetMaxHeath(_maxHealth);
    }

    public void TakeDamage(int amount, Vector3 hitPoint)
    {
        if (isDead)
            return;

        PlayAudioClip(_takeDamageClip);

        _currentHealth -= amount;
        _heathbar.SetHeath(_currentHealth);
        ShowDamageEffect(hitPoint);

        if (_currentHealth <= 0)
        {
            Death();
        }
    }

    private void ShowDamageEffect(Vector3 hitPoint)
    {
        if (_particleSystem == null)
            return;

        _particleSystem.transform.position = hitPoint;
        _particleSystem.Play();
    }

    private void Death()
    {
        isDead = true;

        if (_collider != null)
            _collider.isTrigger = true;

        if (_animator != null)
            _animator.SetTrigger("Dead");

        PlayAudioClip(_deathClip);
    }

    private void PlayAudioClip(AudioClip clip)
    {
        if (_enemyAudio == null || clip == null)
            return;

        _enemyAudio.clip = clip;
        _enemyAudio.Play();
    }
}
