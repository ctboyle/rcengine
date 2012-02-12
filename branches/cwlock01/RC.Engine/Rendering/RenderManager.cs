using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RC.Engine.Cameras;
using RC.Engine.SceneGraph;
using RC.Engine.ContentManagement;
using RC.Engine.Base;
using RC.Engine.Rendering.EffectConstants;

namespace RC.Engine.Rendering
{
    public delegate void RenderFunc(IRCRenderManager render);

    public enum DirectionalLightIndex
    {
        Light0 = 0,
        Light1,
        Light2,
        Count
    }

    /// <summary>
    /// I am the render manager and allow rendering of geometry and scenes.
    /// </summary>
    public interface IRCRenderManager
    {
        IGraphicsDeviceService GraphicsService { get; }

        /// <summary>
        /// I set the render state.
        /// </summary>
        /// <param name="renderState">The render state.</param>
        void SetRenderState(RCRenderState renderState);

        /// <summary>
        /// I get the render state for a render state type.
        /// </summary>
        /// <param name="type">The render state type.</param>
        RCRenderState GetRenderState(RCRenderState.StateType type);

        /// <summary>
        /// I draw a geometric object to the screen.
        /// </summary>
        /// <param name="geometry">The geometric object.</param>
        void Draw(RCVisual visual);

        /// <summary>
        /// I draw a scene to the screen.
        /// </summary>
        /// <param name="sceneRoot">The scene root.</param>
        void Draw(RCSpatial sceneRoot);

        /// <summary>
        /// I clear the screen.
        /// </summary>
        void ClearScreen();
    }

    /// <summary>
    /// Central functionality for Rendering the Scene.
    /// </summary>
    internal class RCRenderManager : IRCRenderManager
    {
        public IGraphicsDeviceService GraphicsService { get; set; }
        private IRCCameraManager CameraManager { get; set; }
        private IRCContentRequester ContentRequester { get; set; }
        private RCRenderStateCollection RenderStates {get; set; } 

        public RCRenderManager(RCGame game)
        {
            game.Services.AddService(typeof(IRCRenderManager), this);

            CameraManager = (IRCCameraManager)game.Services.GetService(typeof(IRCCameraManager));
            GraphicsService = (IGraphicsDeviceService)game.Services.GetService(typeof(IGraphicsDeviceService));
            ContentRequester = (IRCContentRequester)game.Services.GetService(typeof(IRCContentRequester));

            RenderStates = new RCRenderStateCollection(true);
        }

        public GraphicsDevice Device
        {
            get { return GraphicsService.GraphicsDevice; }
        }

        public void SetRenderState(RCRenderStateCollection renderStates)
        {
            foreach (RCRenderState state in renderStates)
            {
                SetRenderState(state);
            }
        }

        public void RestoreRenderState(RCRenderStateCollection renderStates)
        {
            foreach (RCRenderState state in renderStates)
            {
                SetRenderState(RCRenderState.Default[state.GetStateType()]);
            }
        }

        public void SetRenderState(RCRenderState renderState)
        {
            if (renderState != null)
            {
                // Cache the new state
                RenderStates[renderState.GetStateType()] = renderState;

                // Enable the state
                renderState.ConfigureDevice(Device);
            }
        }

        public RCRenderState GetRenderState(RCRenderState.StateType type)
        {
            return RenderStates[type];
        }

        /// <summary>
        /// Use to render a scene.
        /// 
        /// Will ensure that the Correct camera and viewport are used!
        /// </summary>
        public void Draw(RCSpatial sceneRoot)
        {
            if (CameraManager.ActiveCamera == null)
            {
                throw new InvalidOperationException("Active camera must be set before drawing scene");
            }

            UpdateSceneCameraParameters();
            
            // Clear screen using current clear color.
            if (CameraManager.ActiveCamera.ClearScreen)
            {
                ClearScreen();
            }
             
            sceneRoot.Draw(this);
        }

        public void ClearScreen()
        {
            Device.Clear(
                CameraManager.ActiveCamera.ClearOptions,
                CameraManager.ActiveCamera.ClearColor,
                1.0f, 0);
        }

        public void Draw(RCVisual visual)
        {
            RCVisualEffect visualEffect = visual.VisualEffect;
            if (visualEffect != null)
            {
                Effect xnaEffect = visualEffect.Effect;

                xnaEffect.Begin();

                Enable(visual.VertexBuffer, visual.StreamOffset, visual.VertexFormat.GetVertexStrideSize(0));
                Enable(visual.VertexFormat);

                if (visual.IndexBuffer != null)
                {
                    Enable(visual.IndexBuffer);
                }

                foreach (EffectPass pass in xnaEffect.CurrentTechnique.Passes)
                {
                    visualEffect.EffectConstants.Update(visual, CameraManager.ActiveCamera);

                    pass.Begin();
                    DrawPrimitive(visual);
                    pass.End();
                }

                xnaEffect.End();
            }
        }

        protected bool UpdateSceneCameraParameters()
        {
            // Ensure that the correct viewport is drawn to.
            Device.Viewport = CameraManager.ActiveCamera.Viewport;
            return true;
        }

        private void DrawPrimitive(RCVisual visual)
        {
            switch(visual.PrimitiveType)
            {
                case PrimitiveType.TriangleList:
                    {
                        RCTriangles triangles = (RCTriangles)visual;
                        Device.DrawIndexedPrimitives(
                            PrimitiveType.TriangleList,
                            triangles.BaseVertex,
                            triangles.MinVertexIndex,
                            triangles.NumVertices,
                            triangles.StartIndex,
                            triangles.NumTriangles
                            );
                    }
                    break;
                case PrimitiveType.PointList:
                    {
                        RCPolyPoint polyPoint = (RCPolyPoint)visual;
                        if (polyPoint.StartIndex < polyPoint.EndIndex)
                        {
                            // If the points are all in one consecutive range,
                            // we can draw them all in a single call.
                            Device.DrawPrimitives(PrimitiveType.PointList,
                                                  polyPoint.StartIndex,
                                                  polyPoint.EndIndex - polyPoint.StartIndex);
                        }
                        else
                        {
                            // If the points range wraps past the end, we must split them
                            // over two draw calls.
                            Device.DrawPrimitives(PrimitiveType.PointList,
                                                  polyPoint.StartIndex,
                                                  polyPoint.NumPoints - polyPoint.StartIndex);

                            if (polyPoint.EndIndex > 0)
                            {
                                Device.DrawPrimitives(PrimitiveType.PointList,
                                                      0,
                                                      polyPoint.EndIndex);
                            }
                        }
                    }
                    break;
            }
        }

        private void Enable(IndexBuffer ibuffer)
        {
            Device.Indices = ibuffer;
        }

        private void Enable(VertexBuffer vbuffer, int streamOffset, int stride)
        {
            Device.Vertices[0].SetSource(
                        vbuffer,
                        streamOffset,
                        stride);
        }


        private void Enable(VertexDeclaration vformat)
        {
            Device.VertexDeclaration = vformat;
        }
    }
}
