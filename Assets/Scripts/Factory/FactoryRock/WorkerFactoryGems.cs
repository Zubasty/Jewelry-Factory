using UnityEngine;

[RequireComponent(typeof(Animator))]
public class WorkerFactoryGems : MonoBehaviour
{
    [SerializeField] private FactoryGems _factory;

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        _factory.StartedSpawn += () => _animator.SetBool(AnimatorWorkerGems.Params.Work, true);
        _factory.Spawned += (gem) => _animator.SetBool(AnimatorWorkerGems.Params.Work, false);
    }

    private void OnDisable()
    {
        _factory.StartedSpawn -= () => _animator.SetBool(AnimatorWorkerGems.Params.Work, true);
        _factory.Spawned -= (gem) => _animator.SetBool(AnimatorWorkerGems.Params.Work, false);
    }
}
