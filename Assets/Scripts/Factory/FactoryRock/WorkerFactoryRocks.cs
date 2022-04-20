using UnityEngine;

public class WorkerFactoryRocks : MonoBehaviour
{
    [SerializeField] private ParticleSystem _sparks;

    public void PlaySparks() => _sparks.Play();
}
