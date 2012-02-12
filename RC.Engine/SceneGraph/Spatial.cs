#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using RC.Engine.Animation;
using RC.Engine.Cameras;
using RC.Engine.SceneGraph.BoundingVolumes;
using RC.Engine.Rendering;
using RC.Engine.SceneEffects;

using RC.Engine.ContentManagement;
using RC.Engine.GameObject;

#endregion

namespace RC.Engine.SceneGraph
{
    /// <summary>
    /// This serves as the base scene graph object.
    /// </summary>
    /// <remarks>
    /// A spatial object by itself is a leaf in the scene graph.
    /// 
    /// As the base for all other nodes in the graph, spatial provides
    /// support for setting effects, render states, transforms, and bounding
    /// volumes at every node in the tree.
    /// </remarks>
    public abstract class RCSpatial : RCGameObject, ISpatial
    {
        /// <summary>
        /// The computed world bound for this object.
        /// </summary>
        protected IRCBoundingVolume _worldBound;

        /// <summary>
        /// List of contollers that control any time varying quantites in this object
        /// to facilitate animations.
        /// </summary>
        protected List<IController> _animateControllers = new List<IController>();

        /// <summary>
        /// The parent scene graph object of this object. Is null if this is the root.
        /// </summary>
        public RCSpatial ParentNode { get; set;}

        /// <summary>
        /// Flag indicating whether the node's transform is up to date.
        /// </summary>
        public bool WorldTransformIsCurrent { get; set; }
        
        /// <summary>
        /// Gets the current bounding volume in worldspace for theis object.
        /// </summary>
        public IRCBoundingVolume WorldBound
        {
            get { return _worldBound; }
        }

        /// <summary>
        /// Gets or sets the transformation from this node's parent.
        /// If there is no parent, the local is the same as the world transform.
        /// </summary>
        public Matrix LocalTrans { get; set; }

        /// <summary>
        /// Gets or sets the world transform for this node.
        /// </summary>
        /// <remarks>The world transform is the object's tranform in world space.</remarks>
        public Matrix WorldTrans { get; set; }

        /// <summary>
        /// Use this property to configure the active renderstates associated with this node.
        /// </summary>
        /// <remarks> There are no states in the collection by default.</remarks>
        public RCRenderStateCollection GlobalStates { get; set; }

        public RCSpatial()
        {
            LocalTrans = Matrix.Identity;
            WorldTrans = Matrix.Identity;
            ParentNode = null;

            WorldTransformIsCurrent = false;

            _worldBound = new RCBoundingSphere(Vector3.Zero, 0.0f);

            GlobalStates = new RCRenderStateCollection(true);
        }

        /// <summary>
        /// Starts an update pass from this node.
        /// </summary>
        /// <remarks>May be called from anywhere in the scene graph. Updates transforms, animations, and bounding volumes all the way to the root.</remarks>
        /// <param name="gameTime">Elasped time</param>
        /// <param name="fInitiator">Indicates whether this call is the start of an update pass. Is false in recursive calls.</param>
        public virtual void Update(GameTime gameTime, bool fInitiator)
        {
            UpdateWorldData(gameTime);
            UpdateWorldBound();
            if (fInitiator)
            {
                PropigateBVToRoot();
            }
        }

        /// <summary>
        /// Override for specific behavior on the draw pass.
        /// </summary>
        public abstract void Draw(IRCRenderManager render);

        /// <summary>
        /// Sets the animation controller to be updated on this object.
        /// </summary>
        /// <remarks>Please use AnimationController.AttachToObject instead.</remarks>
        /// <param name="controller">The conntroller to add.</param>
        /// <returns>If update succeeded</returns>
        public bool AddController(IController controller)
        {
            bool fAttachSucceeded = false;

            if (controller != null)
            {
                _animateControllers.Add(controller);
                fAttachSucceeded = true;
            }

            return fAttachSucceeded;
        }

        /// <summary>
        /// Gets the controller in the controller list based on its type.
        /// </summary>
        /// <typeparam name="ContrllerType"></typeparam>
        /// <returns></returns>
        public IController GetController<ContrllerType> ()
        {
            return _animateControllers.Last(x => x is ContrllerType);
        }   

        /// <summary>
        /// Removes a controller from the controller list.
        /// </summary>
        /// <param name="controller">The controller refrence to remove.</param>
        public void RemoveController(IController controller)
        {
            if (controller != null)
            {
                _animateControllers.Remove(controller);
            }
        }

        /// <summary>
        /// Updates animations of any controller attached to this object.
        /// </summary>
        /// <param name="gameTime"></param>
        protected void UpdateControllers(GameTime gameTime)
        {
            for (int iController = 0; iController < _animateControllers.Count; iController++ )
            {
                _animateControllers[iController].Update(gameTime);
            }
        }

        /// <summary>
        /// Override to update all object world space data.
        /// </summary>
        /// <remarks>By default, this function updates animations and transformations.</remarks>
        protected virtual void UpdateWorldData(GameTime gameTime)
        {
            // Update components
            Update(gameTime);

            // Update animations
            UpdateControllers(gameTime);

            if (!WorldTransformIsCurrent)
            {
                if (ParentNode != null)
                {
                    // Compute world transform from parent's and local transforms.
                    WorldTrans = LocalTrans* ParentNode.WorldTrans;
                }
                else
                {
                    // This is root, local and world trans are identical.
                    WorldTrans = LocalTrans;
                }
            }
        }
        /// <summary>
        /// Override to specify the world bounding volume for your objects
        /// </summary>
        protected abstract void UpdateWorldBound();

        /// <summary>
        /// If the SceneObject moves and its world BV changes, all its 
        /// parent node's BVs need to be updated.
        ///</summary>
        protected void PropigateBVToRoot()
        {
            if (ParentNode != null)
            {
                ParentNode.UpdateWorldBound();
                ParentNode.PropigateBVToRoot();
            }
        }
    }
}