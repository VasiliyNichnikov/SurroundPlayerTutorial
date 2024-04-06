#nullable enable
using UnityEngine;

namespace RadiusModule
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

        /// <summary>
        /// Проверяем возможно ли дойти до заданной точки
        /// </summary>
        bool CheckAvailabilityOfPoint(Vector3 pointPosition);
    }
}