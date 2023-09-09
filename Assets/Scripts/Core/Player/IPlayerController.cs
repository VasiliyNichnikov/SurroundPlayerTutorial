using System;
using UnityEngine;

namespace Core.Player
{
    public interface IPlayerController
    {
        event Action OnPlayerMovement;
        
        Transform Transform { get; }
    }
}