﻿#nullable enable
using UnityEngine;

namespace Core.RadiusModule
{
    /// <summary>
    /// Интерфейс для объектов, которые могут стоять в радиусе
    /// </summary>
    public interface ISurroundingObject
    {
        Vector3 Position { get; }
        /// <summary>
        /// Возвращает расстояние до центра
        /// Центр - точка из которой считаем радиусы
        /// </summary>
        float GetDistanceToCenter();
    }
}