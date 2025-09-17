using UnityEngine;
using Unity.Cinemachine;

public class Rock : MonoBehaviour
{
    CinemachineImpulseSource cinemachineImpulseSource;
    [SerializeField] float shakeModifer = 10f;
    [SerializeField] ParticleSystem collisionParticleSystem;
    [SerializeField] AudioSource boulderSmashAudioSource;
    [SerializeField] float collisionCooldown = 1f;
    float collisionTimer = 1f;
    GameManager gameManager;

    private void Awake()
    {
        cinemachineImpulseSource = GetComponent<CinemachineImpulseSource>();
        gameManager = FindFirstObjectByType<GameManager>();
    }
    void Update()
    {
        collisionTimer += Time.deltaTime;
    }


    void OnCollisionEnter(Collision collision)
    {
        if (collisionTimer < collisionCooldown) return;
        FireImpuls();
        CollisionFX(collision);

        collisionTimer = 0f;

    }

    private void FireImpuls()
    {
        float distance = Vector3.Distance(transform.position, Camera.main.transform.position);
        float shakeIntensity = (1f / distance) * shakeModifer;
        shakeIntensity = Mathf.Min(shakeIntensity, 1);
        cinemachineImpulseSource.GenerateImpulse(shakeIntensity);
    }
    void CollisionFX(Collision collision)
    {
        if (gameManager.GameOver == true) return;
        ContactPoint contactPoint = collision.contacts[0];
        collisionParticleSystem.transform.position = contactPoint.point;
        collisionParticleSystem.Play();
        boulderSmashAudioSource.Play();
    }
}
