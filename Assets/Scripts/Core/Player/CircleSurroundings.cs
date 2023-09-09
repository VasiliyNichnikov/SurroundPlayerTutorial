#nullable enable
using System.Collections.Generic;
using Core.Utils;
using UnityEngine;

namespace Core.Player
{
    public static class CircleSurroundings
    {
        private static readonly int NumberEnemiesAroundPlayer = 6;
        
        private static readonly float AngleIncrease = 360.0f / NumberEnemiesAroundPlayer;

        public static Vector3[] CalculatePointsInCircleWithWalls(Vector3 center, float radius, int layerMask)
        {
            var readyPoints = new List<Vector3>();
            var points = CalculatePointsInCircle(center, radius);
            foreach (var point in points)
            {
                var direction = point - center;
                if (Physics.Raycast(center, direction, radius, layerMask))
                {
                    continue;
                }

                readyPoints.Add(point);
            }

            return readyPoints.ToArray();
        }
        
        public static Vector3[] CalculatePointsInCircle(Vector3 center, float radius)
        {
            var points = new List<Vector3>();
            var angle = .0f;
            for (var i = 0; i < NumberEnemiesAroundPlayer; i++)
            {
                var point = center + MyUtils.GetVectorFromAngle(angle) * radius;
                angle -= AngleIncrease;
                points.Add(point);
            }

            return points.ToArray();
        }
    }
}