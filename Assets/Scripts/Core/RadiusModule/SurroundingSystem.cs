#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Core.RadiusModule
{
    // TODO вынести все проверки с радиусом в отдельный метод и вызывать везде в начале
    /// <summary>
    /// Система для работы с радиусами и определения точек
    /// </summary>
    public class SurroundingSystem
    {
        // int - номер радиуса, int - индекс точки 
        // RadiusPointData - данные точки 
        private readonly Dictionary<int, Dictionary<int, RadiusPointData>> _pointData = new();

        // ключ - номер радиуса, значение - длина радиуса
        private readonly Dictionary<int, float> _surroundingLength = new();

        private readonly ITransformPointFactory _pointFactory;

        public SurroundingSystem(ITransformPointFactory pointFactory)
        {
            _pointFactory = pointFactory;
            InitializeRadius();
        }

        /// <summary>
        /// Вовзращает точки для движения в радиусе
        /// </summary>
        public IEnumerable<Vector3> GetPathToPointInRadius(ISurroundingObject surroundingObject)
        {
            // 1. Сначала получаем радиус
            // 2. Затем из этого радиуса получаем все доступные точки
            // 3. Сортируем точки относительно surroundingObject
            // 4. Выбираем самую первую точку
            // 5. Прокладываем маршрут относительно не занятых точек в радиусе
            if (!TryGetNearestAvailableRadius(out var radiusIndexRaw))
            {
                Debug.LogError("SurroundingSystem.GetPathToPointInRadius: radiusIndex is null");
                return Array.Empty<Vector3>();
            }

            var radiusIndex = radiusIndexRaw!.Value;
            if (!TryGetNearestFreeOrOccupiedPointInRadius(surroundingObject, radiusIndex, out var startPointIndexRaw))
            {
                Debug.LogError("SurroundingSystem.GetPathToPointInRadius: startPointIndex is null");
                return Array.Empty<Vector3>();
            }

            var startPointIndex = startPointIndexRaw!.Value;
            if (!TryGetNearestFreePointInRadius(surroundingObject, radiusIndex, out var endPointIndexRaw))
            {
                Debug.LogError("SurroundingSystem.GetPathToPointInRadius: endPointIndex is null");
                return Array.Empty<Vector3>();
            }

            var endPointIndex = endPointIndexRaw!.Value;

            var firstPoint = GetPositionPoint(radiusIndex, startPointIndex);
            if (endPointIndex == startPointIndex)
            {
                return new[] { firstPoint };
            }

            var points = new List<Vector3>();
            var pointsCount = _pointData[radiusIndex].Count;
            // Расстояние если идти по часовой стрелки
            var clockwiseDistance = endPointIndex - startPointIndex > 0
                ? endPointIndex - startPointIndex
                : pointsCount - startPointIndex + endPointIndex;
            // Расстояние если идти против часовой стрелки
            var counterclockwiseDistance = endPointIndex - startPointIndex > 0
                ? pointsCount - endPointIndex + startPointIndex
                : Mathf.Abs(endPointIndex - startPointIndex);

            var step = clockwiseDistance < counterclockwiseDistance ? 1 : -1;
            var currentPointIndex = GetPointIndexInRadius(radiusIndex, startPointIndex + step);
            var lastPointIndex = GetPointIndexInRadius(radiusIndex, endPointIndex);
            while (currentPointIndex != lastPointIndex)
            {
                points.Add(_pointData[radiusIndex][currentPointIndex].Transform.position);
                currentPointIndex = GetPointIndexInRadius(radiusIndex, currentPointIndex + step);
            }

            // В конце добавляем последнюю точку, так как цикл ее не добавляет
            points.Add(GetPositionPoint(radiusIndex, lastPointIndex));
            return points;
        }

        /// <summary>
        /// Проверяет может ли заданный объект попасть в радиус
        /// </summary>
        public bool CanJoinRadius(ISurroundingObject surroundingObject)
        {
            if (!TryGetNearestAvailableRadius(out var result))
            {
                return false;
            }

            var radius = _surroundingLength[result!.Value];
            return radius >= surroundingObject.GetDistanceToCenter();
        }

        /// <summary>
        /// Пока тестовый код, в будущем нужно брать из настроек
        /// </summary>
        private void InitializeRadius()
        {
            // Для теста создадим один радиус
            _surroundingLength[0] = 2f; // TODO будет браться из настроек
            const float angleIncrease = 35f; // TODO будет браться из настроек

            foreach (var kvp in _surroundingLength)
            {
                var angle = 0f;
                var pointIndex = 0;
                _pointData[kvp.Key] = new Dictionary<int, RadiusPointData>();
                var numberPoints = Mathf.FloorToInt(360f / angleIncrease);
                for (var i = 0; i < numberPoints; i++)
                {
                    var pointTransform = _pointFactory.CreatePoint(angle, kvp.Value);
#if UNITY_EDITOR
                    pointTransform.name += $"_{i}";
#endif
                    if (_pointData[kvp.Key].ContainsKey(pointIndex))
                    {
                        Debug.LogError(
                            $"SurroundingSystem.InitializeRadius: point with index {pointIndex} contains in radius with id {kvp.Key}");
                        pointIndex++;
                        continue;
                    }

                    _pointData[kvp.Key][pointIndex] = RadiusPointData.Default(pointTransform, angle);

                    angle -= angleIncrease;
                    pointIndex++;
                }
            }
        }

        private Vector3 GetPositionPoint(int radiusIndex, int pointIndex) =>
            GetPointDataInRadius(radiusIndex, pointIndex).Transform.position;

        /// <summary>
        /// Передаем pointIndex
        /// На выходе конвертирует индекс в корректный, например если передать -1,
        /// то результат будет кол-во элементов в радиусе - 1
        /// </summary>
        private int GetPointIndexInRadius(int radiusIndex, int pointIndex)
        {
            if (!_pointData.ContainsKey(radiusIndex))
            {
                Debug.LogError($"SurroundingSystem.GetPointIndexInRadius: not found radius with index: {radiusIndex}");
                return 0;
            }

            var points = _pointData[radiusIndex];
            var pointIndexResult = pointIndex < 0
                ? points.Count - Mathf.Abs(pointIndex % points.Count)
                : pointIndex % points.Count;
            return pointIndexResult;
        }

        /// <summary>
        /// Возвращает точки по кругу при этом можно с любой стороны выбрать
        /// </summary>
        private RadiusPointData GetPointDataInRadius(int radiusIndex, int pointIndex)
        {
            var points = _pointData[radiusIndex];
            var pointIndexResult = GetPointIndexInRadius(radiusIndex, pointIndex);
            return points[pointIndexResult];
        }

        /// <summary>
        /// Возвращает ближайшую свободную точку к объекту
        /// </summary>
        private bool TryGetNearestFreePointInRadius(ISurroundingObject surroundingObject, int radiusIndex,
            out int? result) =>
            TryGetNearestPointInRadiusWithCheck(surroundingObject, radiusIndex, state => state == RadiusPointState.Free,
                out result);

        /// <summary>
        /// Возвращает ближайшую свободную или занятую точку (врагом)
        /// </summary>
        private bool TryGetNearestFreeOrOccupiedPointInRadius(ISurroundingObject surroundingObject, int radiusIndex,
            out int? result) =>
            TryGetNearestPointInRadiusWithCheck(surroundingObject, radiusIndex,
                state => state == RadiusPointState.Free || state == RadiusPointState.Occupied, out result);

        private bool TryGetNearestPointInRadiusWithCheck(ISurroundingObject surroundingObject, int radiusIndex,
            Func<RadiusPointState, bool> onCheckPoint, out int? result)
        {
            if (!_pointData.ContainsKey(radiusIndex))
            {
                Debug.LogError(
                    $"SurroundingSystem.TryGetNearestPointInRadiusWithCheck: not found radius with index {radiusIndex}");
                result = null;
                return false;
            }

            var availablePoints = GetPointsInRadiusWithCheck(radiusIndex, onCheckPoint);
            result = FindNearestPoint(radiusIndex, surroundingObject, availablePoints);
            return true;
        }

        private int? FindNearestPoint(int radiusIndex, ISurroundingObject surroundingObject, IEnumerable<int> points)
        {
            if (!_pointData.ContainsKey(radiusIndex))
            {
                Debug.LogError(
                    $"SurroundingSystem.FindNearestPoint: not found radius with index {radiusIndex}");
                return null;
            }

            var radius = _pointData[radiusIndex];
            var minDistance = Mathf.Infinity;
            int? nearestPointIndex = null;
            foreach (var pointIndex in points)
            {
                var point = radius[pointIndex];
                var distance = Vector3.Distance(point.Transform.position, surroundingObject.Position);
                if (minDistance > distance)
                {
                    minDistance = distance;
                    nearestPointIndex = pointIndex;
                }
            }

            return nearestPointIndex;
        }

        private List<int> GetPointsInRadiusWithCheck(int radiusIndex, Func<RadiusPointState, bool> onCheckPoint)
        {
            if (!_pointData.ContainsKey(radiusIndex))
            {
                Debug.LogError($"SurroundingSystem.TryGetPointInRadius: not found radius with index {radiusIndex}");
                return new List<int>();
            }

            var radius = _pointData[radiusIndex];
            var selectedPoints = radius.Where(kvp => onCheckPoint(kvp.Value.State)).Select(kvp => kvp.Key).ToList();
            return selectedPoints;
        }

        private bool TryGetNearestAvailableRadius(out int? result)
        {
            foreach (var kvp in _pointData)
            {
                var isAvailableRadius = kvp.Value.Any(d => d.Value.IsFree());
                if (isAvailableRadius)
                {
                    result = kvp.Key;
                    return true;
                }
            }

            result = null;
            return false;
        }
    }
}