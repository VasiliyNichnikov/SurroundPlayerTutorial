#nullable enable
using UnityEngine;

namespace Core.RadiusModule
{
    public interface ITransformPointFactory
    {
        Transform CreatePoint(float angle, float lengthRadius);
    }
}