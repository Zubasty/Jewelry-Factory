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

    private void OnEnable()
    {
        _player.StartedMove += SetMove;
        _player.StoppedMove += SetMove;
        _player.TakedResources += (player) => SetCarry();
        _player.DroppedResources += SetCarry;
    }

    private void OnDisable()
    {
        _player.StartedMove -= SetMove;
        _player.StoppedMove -= SetMove;
        _player.TakedResources -= (player) => SetCarry();
        _player.DroppedResources -= SetCarry;
    }

    private void SetMove()
    {
        _animator.SetBool(AnimatorPlayer.Params.Walk, _player.IsMove);
    }

    private void SetCarry()
    {
        _animator.SetBool(AnimatorPlayer.Params.Carry, _player.HaveRocksInArms);
    }
}
