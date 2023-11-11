#nullable enable
using UnityEngine;

namespace Core.RadiusModule
{
    /// <summary>
    /// Состояния радиуса на карте
    /// </summary>
    internal enum RadiusPointState
    {
        Free, // Свободен для врага
        Occupied, // Занята врагом
        Locked, // Заблокирована для движения (Как вариант из-за стены или потому что нельзя добраться)
    }
    
    internal readonly struct RadiusPointData
    {
        public RadiusPointState State { get; } 
        public Transform Transform { get; }
        public float Angle { get; }
        
        public bool IsFree() => State == RadiusPointState.Free;
        public bool IsOccupied() => State == RadiusPointState.Occupied;
        public bool IsLocked() => State == RadiusPointState.Locked;
        
        public static RadiusPointData Default(Transform pointTransform, float angle)
        {
            return new RadiusPointData(RadiusPointState.Free, pointTransform, angle);
        }

        public static RadiusPointData Locked(Transform pointTransform, float angle)
        {
            return new RadiusPointData(RadiusPointState.Locked, pointTransform, angle);
        }
        
        public static RadiusPointData Occupied(Transform pointTransform, float angle)
        {
            return new RadiusPointData(RadiusPointState.Occupied, pointTransform, angle);
        }

        private RadiusPointData(RadiusPointState state, Transform transform, float angle)
        {
            State = state;
            Transform = transform;
            Angle = angle;
        }
    }
}