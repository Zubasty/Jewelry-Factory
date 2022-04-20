using UnityEngine;

[RequireComponent(typeof(Player))]
[RequireComponent(typeof(Animator))]
public class PlayerAnimation : MonoBehaviour
{
    private Player _player;
    private Animator _animator;

    private void Awake()
    {
        _player = GetComponent<Player>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        _animator.SetBool(AnimatorPlayer.Params.Walk, _player.IsMove);
        _animator.SetBool(AnimatorPlayer.Params.Carry, _player.HaveRocksInArms);
    }
}
