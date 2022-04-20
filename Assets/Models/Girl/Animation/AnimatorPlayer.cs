using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AnimatorPlayer
{
    public static class States
    {
        public const string Idle = nameof(Idle);
        public const string Walk = nameof(Walk);
        public const string Carry = nameof(Carry);
        public const string CarryIdle = nameof(CarryIdle);
    }

    public static class Params
    {
        public const string Walk = nameof(Walk);
        public const string Carry = nameof(Carry);
    }
}
