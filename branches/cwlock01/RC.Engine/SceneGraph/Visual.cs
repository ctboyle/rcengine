using System;
using System.Collections.Generic;
using System.Text;
using RC.Engine.Rendering;
using Microsoft.Xna.Framework;
using RC.Engine.SceneEffects;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using RC.Engine.ContentManagement;
using RC.Engine.SceneGraph.BoundingVolumes;
using RC.Engine.Utility;

namespace RC.Engine.SceneGraph
{
    /// <summary>
    /// Geometry is a leaf object in the scen graph that has renderable data.
    /// </summary>
    public abstract class RCVisual : RCSpatial
    {
        public VertexDeclaration VertexFormat;
        public VertexBuffer VertexBuffer;
        public IndexBuffer IndexBuffer;
        public int StreamOffset;

        /// <summary>
        /// Abstract primitive type to which derived classes implement.
        /// </summary>
        public abstract PrimitiveType PrimitiveType { get; }

        /// <summary>
        /// The list of accumulated render states this object should be rendered with.
        /// </summary>
        public RCRenderStateCollection RenderStates = new RCRenderStateCollection(false);

        private RCBoundingSphere _modelBound = new RCBoundingSphere();

        /// <summary>
        /// The bounding volume in object space for this object.
        /// </summary>
        public RCBoundingSphere ModelBound
        {
            get { return _modelBound; }
        }

        public RCVisualEffect VisualEffect {get; set;}


        public RCVisual(
            VertexDeclaration vertexFormat,
            VertexBuffer vertexBuffer,
            IndexBuffer indexBuffer,
            int streamOffset)
        {
            VertexFormat = vertexFormat;
            VertexBuffer = vertexBuffer;
            IndexBuffer  = indexBuffer;
            StreamOffset = streamOffset;
        }

        /// <summary>
        /// Draws the geometric data as a triangle list.
        /// </summary>
        /// <param name="render">The render manager</param>
        /// <param name="contentRqst">The content requester for on demand loading</param>
        public override void Draw(IRCRenderManager render)
        {
            render.Draw(this);
        }

        /// <summary>
        /// Tansforms the local bounding volume to the world bounding volume.
        /// </summary>
        protected override void UpdateWorldBound()
        {
            _worldBound = _modelBound.Transform(WorldTrans);
        }

        void UpdateModelBound ()
        {
            //int numVertices = mVBuffer->GetNumElements();
            //int stride = mVFormat->GetStride();

            //int posIndex = mVFormat->GetIndex(VertexFormat::AU_POSITION);
            //if (posIndex == -1)
            //{
            //    assertion(false, "Update requires vertex positions\n");
            //    return;
            //}

            //VertexFormat::AttributeType posType =
            //    mVFormat->GetAttributeType(posIndex);
            //if (posType != VertexFormat::AT_FLOAT3
            //&&  posType != VertexFormat::AT_FLOAT4)
            //{
            //    assertion(false, "Positions must be 3-tuples or 4-tuples\n");
            //    return;
            //}

            //char* data = mVBuffer->GetData();
            //int posOffset = mVFormat->GetOffset(posIndex);
            //mModelBound.ComputeFromData(numVertices, stride, data + posOffset);
        }

        
    }
}
