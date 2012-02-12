using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using RC.Engine.ContentManagement;
using RC.Engine.Rendering;
using RC.Engine.SceneEffects;
using RC.Engine.SceneGraph.BoundingVolumes;
using Microsoft.Xna.Framework.Content;
using RC.Engine.SceneGraph;

namespace RC.Engine.ContentManagement.ContentTypes
{
    /// <summary>
    /// A model content type.
    /// </summary>
    public class RCModel : RCDefaultContent<Model>
    {
        public RCModel(string assetName) : base(assetName) { }

        public RCSceneNode ConvertToSceneGraph()
        {
            RCSceneNode model = new RCSceneNode();

            foreach (ModelMesh xnaMesh in Content.Meshes)
            {
                foreach (ModelMeshPart xnaPart in xnaMesh.MeshParts)
                {
                    foreach (EffectParameter p in xnaPart.Effect.Parameters)
                    {
                        Console.WriteLine(p.Name);
                    }

                    // Create the scene graph object that holds the drawable data.
                    RCTriangles newPart = new RCTriangles(
                        xnaPart.VertexDeclaration,
                        xnaMesh.VertexBuffer,
                        xnaMesh.IndexBuffer,
                        xnaPart.StreamOffset,
                        xnaPart.BaseVertex,
                        0,
                        xnaPart.StartIndex,
                        xnaPart.NumVertices,
                        xnaPart.PrimitiveCount);

                    if (xnaPart.Effect is BasicEffect)
                    {
                        newPart.VisualEffect = new RCBasicEffect(
                            (BasicEffect)xnaPart.Effect);
                            
                    }

                    //newPart.ModelBound = new RCBoundingSphere(xnaMesh.BoundingSphere);
                    newPart.LocalTrans = xnaMesh.ParentBone.Transform;
                    model.AddChild(newPart);
                }
            }

            return model;
        }
    }
}
