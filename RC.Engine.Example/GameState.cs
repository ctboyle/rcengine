using System;
using System.Collections.Generic;
using System.Text;

using RC.Engine.StateManagement;
using RC.Engine.Cameras;
using RC.Engine.GameObject;
using RC.Engine.Rendering;
using RC.Engine.SceneGraph;
using RC.Engine.SceneEffects;
using RC.Engine.ContentManagement.ContentTypes;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RC.Engine.Example
{
    public class ShipMover : RCGameComponent
    {
        Matrix initialPos;
        RCSpatial spatial;
        float angle;

        public override void Initialize()
        {
            spatial = GetComponent<RCSpatial>();
            initialPos = spatial.LocalTrans;
        }

        public override void Update(GameTime gameTime)
        {
            angle += (float)(MathHelper.PiOver2 * gameTime.ElapsedRealTime.TotalSeconds);

            spatial.LocalTrans = initialPos * Matrix.CreateRotationZ((float)Math.Sin(angle));
        }
    }

    class GameState : RCGameState
    {
        private IGraphicsDeviceService graphics = null;
        private IRCCameraManager cameraMgr = null;
        private IRCRenderManager renderMgr = null;
        private RCSceneNode sceneRoot = null;

        public GameState(IServiceProvider services)
            : base(services)
        {
            #region Get Required Services
            graphics = (IGraphicsDeviceService)Services.GetService(typeof(IGraphicsDeviceService));
            cameraMgr = (IRCCameraManager)Services.GetService(typeof(IRCCameraManager));
            renderMgr = (IRCRenderManager)Services.GetService(typeof(IRCRenderManager));
            #endregion
        }

        public override void Initialize()
        {
            ///////////////////////////////////////////////////////////////
            // Create a Camera
            ///////////////////////////////////////////////////////////////
            RCCamera camera = new RCPerspectiveCamera(graphics.GraphicsDevice.Viewport);
            Matrix cameraLookAt = Matrix.CreateLookAt(new Vector3(2.0f, 4.0f, 10.0f), Vector3.Zero, Vector3.Up);
            camera.LocalTrans = Matrix.Invert(cameraLookAt);
            cameraMgr.AddCamera("MainCamera", camera);

            camera.AddComponent<KeyboardController>();


            ///////////////////////////////////////////////////////////////
            // Create the light
            ///////////////////////////////////////////////////////////////
            RCLight light = new RCLight(RCLight.LightType.Ambient);
            light.Diffuse = new Vector4(1.0f);
            light.Specular = new Vector4(0.8f);
            light.SetDirection(new Vector3(-1.0f, -1.0f, 0.0f));
            
            ///////////////////////////////////////////////////////////////
            // Create the model
            ///////////////////////////////////////////////////////////////
            RCModel model = new RCModel(@"Content\Models\p1_wedge");
            RCSceneNode modelNode = model.ConvertToSceneGraph();
            modelNode.LocalTrans = 
                  Matrix.CreateRotationY(MathHelper.ToRadians(180.0f)) *
                  Matrix.CreateScale(0.005f);

            modelNode.AddComponent<ShipMover>();

            ///////////////////////////////////////////////////////////////
            // Construt the scene graph.
            ///////////////////////////////////////////////////////////////
            // Create the Root Node
            sceneRoot = new RCSceneNode();

            // ALWAYS INCLUDE CAMERA IN THE SCENE GRAPH SO IT CAN BE UPDATED.
            sceneRoot.AddChild(camera);
            sceneRoot.AddChild(modelNode);

            // Attach a light to the root; this is not a child node.
            //sceneRoot.AddLight(light);

            //model.Content.GlobalStates.Add(depthState);

            // Update the renderstates so that lighting will be applied
            //sceneRoot.UpdateRS();

            base.Initialize();
        }
        
        public override void Update(GameTime gameTime)
        {
            // Update the scene graph.
            sceneRoot.Update(gameTime, true);
            base.Update(gameTime);
        }
        
        public override void Draw(GameTime gameTime)
        {
            // Set the active camera
            cameraMgr.SetActiveCamera("MainCamera");

            // Use the render manager to draw the scene.
            renderMgr.Draw(sceneRoot);
            base.Draw(gameTime);
        }
    }
}
