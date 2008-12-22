using System;
using Microsoft.Xna.Framework;
using RC.Engine.ContentManagement;
using RC.Engine.StateManagement;
using RC.Engine.Cameras;
using RC.Engine.Rendering;

namespace RC.Engine.Base
{
    /// <summary>
    /// The base game that all games should be derived.
    /// </summary>
    public class RCXnaGame : Game
    {
        private GraphicsDeviceManager _deviceMgr = null;
        private IRCContentManager _contentMgr = null;
        private IRCCameraManager _cameraMgr = null;
        private IRCGameStateManager _stateMgr = null;
        private IRCGameStateStack _stateStk = null;
        private IRCRenderManager _renderMgr = null;

        /// <summary>
        /// I am a contructor that will setup the graphics device manager.
        /// </summary>
        public RCXnaGame()
        {
            _deviceMgr = new GraphicsDeviceManager(this);
            _contentMgr = new RCContentManager(this);
            _cameraMgr = new RCCameraManager(this);
            _stateMgr = new RCGameStateManager(this);
            _stateStk = (IRCGameStateStack) _stateMgr;
            _renderMgr = new RCRenderManager(this);
        }

        /// <summary>
        /// The Update Event.
        /// </summary>
        public event EventHandler<GameTimeEventArgs> UpdateEvent;

        /// <summary>
        /// The Draw Event.
        /// </summary>
        public event EventHandler<GameTimeEventArgs> DrawEvent;

        /// <summary>
        /// The graphics device manager for the game.
        /// </summary>
        protected GraphicsDeviceManager DeviceMgr
        {
            get { return _deviceMgr; }
        }

        /// <summary>
        /// The content manager for the game.
        /// </summary>
        protected IRCContentManager ContentMgr
        {
            get { return _contentMgr; }
        }

        /// <summary>
        /// The camera manager for the game.
        /// </summary>
        protected IRCCameraManager CameraMgr
        {
            get { return _cameraMgr; }
        }

        /// <summary>
        /// The state manager for the game.
        /// </summary>
        protected IRCGameStateManager StateMgr
        {
            get { return _stateMgr; }
        }

        /// <summary>
        /// The state stack for the game.
        /// </summary>
        protected IRCGameStateStack StateStk
        {
            get { return _stateStk; }
        }

        /// <summary>
        /// The render manager for the game.
        /// </summary>
        protected IRCRenderManager RenderMgr
        {
            get { return _renderMgr; }
        }

        /// <summary>
        /// I initialize stuff.
        /// </summary>
        protected override void Initialize()
        {
            Components.Add(_contentMgr);
            Components.Add(_stateMgr);
            base.Initialize();
        }

        /// <summary>
        /// Called on an update pass.
        /// </summary>
        /// <param name="gameTime">The current gametime.</param>
        protected override void Update(GameTime gameTime)
        {
            RaiseUpdateEvent(new GameTimeEventArgs(gameTime));
            base.Update(gameTime);
        }

        /// <summary>
        /// Called on an draw pass.
        /// </summary>
        /// <param name="gameTime">The current gametime.</param>
        protected override void Draw(GameTime gameTime)
        {
            RaiseDrawEvent(new GameTimeEventArgs(gameTime));
            base.Draw(gameTime);
        }

        private void RaiseUpdateEvent(GameTimeEventArgs args)
        {
            if(UpdateEvent != null) UpdateEvent(this, args);
        }

        private void RaiseDrawEvent(GameTimeEventArgs args)
        {
            if (DrawEvent != null) DrawEvent(this, args);
        }
    }
}
