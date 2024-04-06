#nullable enable
using Core.Utils;
using RadiusModule;
using UnityEngine;

namespace Core.Surroundings
{
    public class TransformPointFactoryDefault : ITransformPointFactory
    {
        private readonly Transform _parentPoints;

        public TransformPointFactoryDefault(Transform parentPoints)
        {
            _parentPoints = parentPoints;
        }
        
        public Transform CreatePoint(Vector3 centerPoint, float angle, float lengthRadius)
        {
            var point = new GameObject("RadiusPoint").transform;
            point.position = centerPoint + MyUtils.GetVectorFromAngle(angle) * lengthRadius;
            point.SetParent(_parentPoints, true);
            return point;
        }
    }
}