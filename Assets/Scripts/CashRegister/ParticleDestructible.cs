using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class ParticleDestructible : MonoBehaviour
{
    private ParticleSystem _particle;

    private void Awake()
    {
        _particle = GetComponent<ParticleSystem>();
    }

    private void Start()
    {
        ParticleSystem.MainModule main = _particle.main;
        Destroy(gameObject, main.duration + main.startLifetime.constant);
    }
}
