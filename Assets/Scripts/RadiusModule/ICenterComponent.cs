#nullable enable
using System;
using UnityEngine;

namespace RadiusModule
{
    public interface ICenterComponent
    {
        event Action? OnPositionChanged;
        
        Vector3 Position { get; }
    }
}