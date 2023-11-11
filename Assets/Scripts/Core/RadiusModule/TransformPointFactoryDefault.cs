#nullable enable
using Core.Utils;
using UnityEngine;

namespace Core.RadiusModule
{
    public class TransformPointFactoryDefault : ITransformPointFactory
    {
        private readonly Transform _parentPoints;
        private readonly Vector3 _centerPoints;

        public TransformPointFactoryDefault(Transform parentPoints, Vector3 centerPoints)
        {
            _parentPoints = parentPoints;
            _centerPoints = centerPoints;
        }
        
        public Transform CreatePoint(float angle, float lengthRadius)
        {
            var point = new GameObject("RadiusPoint").transform;
            point.position = _centerPoints + MyUtils.GetVectorFromAngle(angle) * lengthRadius; // TODO MyUtils нужно внести сюда
            point.SetParent(_parentPoints, true);
            return point;
        }
    }
}