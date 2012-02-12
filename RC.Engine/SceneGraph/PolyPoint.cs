using RC.Engine.Rendering;
using Microsoft.Xna.Framework.Graphics;

namespace RC.Engine.SceneGraph
{
    /// <summary>
    /// RCTriangles is a leaf object in the scen graph that has triangle data.
    /// </summary>
    public class RCPolyPoint : RCVisual
    {
        /// <summary>
        /// Abstract primitive type to which derived classes implement.
        /// </summary>
        public override PrimitiveType PrimitiveType { get { return PrimitiveType.PointList; } }

        public int StartIndex { get; set; }
        public int EndIndex { get; set; }
        public int NumPoints {get; set; }

        public RCPolyPoint(
            VertexDeclaration vertexFormat,
            VertexBuffer vertexBuffer,
            int streamOffset,
            int startIndex,
            int endIndex,
            int numPoints)
            :base(
                vertexFormat,
                vertexBuffer,
                null,
                streamOffset)
        {
            StartIndex = startIndex;
            EndIndex = endIndex;
            NumPoints = numPoints;
        }
    }
}