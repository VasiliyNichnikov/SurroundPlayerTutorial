#nullable enable
using System.Collections.Generic;

namespace RadiusModule
{
    internal interface ICheckerForPointUpdates
    {
        void Check(Dictionary<int, Dictionary<int, RadiusPointData>> pointData);
    }
}