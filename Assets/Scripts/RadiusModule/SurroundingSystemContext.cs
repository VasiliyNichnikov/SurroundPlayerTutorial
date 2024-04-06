using System.Collections.ObjectModel;

namespace RadiusModule
{
    public struct SurroundingSystemContext
    {
        public ITransformPointFactory PointFactory { get; }
        public ICenterComponent CenterComponent { get; }
        public ReadOnlyCollection<RadiusSettings> RadiusSettings { get; }
        public int LayerMaskWalls { get; }


        public static SurroundingSystemContext CreateContext(
            ITransformPointFactory factory,
            ICenterComponent centerComponent,
            ReadOnlyCollection<RadiusSettings> radiusSettings, 
            int layerMaskWalls)
        {
            return new SurroundingSystemContext(factory, centerComponent, radiusSettings, layerMaskWalls);
        }
        
        private SurroundingSystemContext(ITransformPointFactory factory, 
            ICenterComponent centerComponent,
            ReadOnlyCollection<RadiusSettings> radiusSettings, 
            int layerMaskWalls)
        {
            PointFactory = factory;
            CenterComponent = centerComponent;
            RadiusSettings = radiusSettings;
            LayerMaskWalls = layerMaskWalls;
        }
    }
}