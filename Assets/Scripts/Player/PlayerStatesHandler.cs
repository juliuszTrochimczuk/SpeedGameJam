using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class PlayerStatesHandler : MonoBehaviour
    {
        public enum PlayerState { Moving, Spinning }

        public PlayerState CurrentState { get; set; }
    }
}