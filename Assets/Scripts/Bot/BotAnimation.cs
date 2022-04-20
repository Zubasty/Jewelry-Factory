using UnityEngine;

[RequireComponent(typeof(Bot))]
[RequireComponent(typeof(Animator))]
public class BotAnimation : MonoBehaviour
{
    private Animator _animator;
    private Bot _bot;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _bot = GetComponent<Bot>();
    }

    private void OnEnable()
    {
        _bot.StartedIdle += () => _animator.Play(AnimatorBot.States.Idle);
        _bot.StartedMove += () => _animator.Play(AnimatorBot.States.Walk);
    }

    private void OnDisable()
    {
        _bot.StartedIdle -= () => _animator.Play(AnimatorBot.States.Idle);
        _bot.StartedMove -= () => _animator.Play(AnimatorBot.States.Walk);
    }
}
