#nullable enable
using UnityEngine;

namespace RadiusModule
{
    public interface ITransformPointFactory
    {
        Transform CreatePoint(Vector3 centerPoint, float angle, float lengthRadius);
    }
}