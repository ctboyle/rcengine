using RC.Engine.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RC.Engine.SceneGraph
{
    /// <summary>
    /// RCTriangles is a leaf object in the scen graph that has triangle data.
    /// </summary>
    public class RCTriangles : RCVisual
    {
        public int BaseVertex;
        public int MinVertexIndex;
        public int StartIndex;
        public int NumVertices;
        public int NumTriangles;

        /// <summary>
        /// Abstract primitive type to which derived classes implement.
        /// </summary>
        public override PrimitiveType PrimitiveType
        {
            get { return PrimitiveType.TriangleList; }
        }

        public RCTriangles(
            VertexDeclaration vertexFormat,
            VertexBuffer vertexBuffer,
            IndexBuffer indexBuffer,
            int streamOffset,
            int baseVertex,
            int minVertexIndex,
            int startIndex,
            int numVertices,
            int numTriangles)
            :base(
                vertexFormat,
                vertexBuffer,
                indexBuffer,
                streamOffset)
        {
            StreamOffset = streamOffset;
            MinVertexIndex = minVertexIndex;
            BaseVertex = baseVertex;
            StartIndex = startIndex;
            NumVertices = numVertices;
            NumTriangles = numTriangles;
        }
    }
}
