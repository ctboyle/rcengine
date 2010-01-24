using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RC.Engine.Rendering;
using RC.Engine.ContentManagement;
using RC.Engine.Base;

namespace RC.Engine.StateManagement
{
    public delegate void StateChangeHandler(RCGameState newState, RCGameState oldState);
    
    /// <summary>
    /// I am the game state manager and maintain the current stack.  I have 
    /// the ability to push, pop, and peek states from the stack.
    /// </summary>
    public interface IRCGameStateManager : IGameComponent
    {
        /// <summary>
        /// I inform that a state has changed.
        /// </summary>
        event StateChangeHandler StateChanged;

        /// <summary>
        /// I push states onto the stack by name.
        /// </summary>
        /// <param name="state">The state instance.</param>
        void PushState(RCGameState state);

        /// <summary>
        /// I pop the top state off the stack.
        /// </summary>
        /// <returns>The state.</returns>
        RCGameState PopState();

        /// <summary>
        /// I peek the top state on the stack.  I do not alter the stack.
        /// </summary>
        /// <returns>The state.</returns>
        RCGameState PeekState();
    }

    internal class RCGameStateManager : DrawableGameComponent, IRCGameStateManager
    {
        private LinkedList<RCGameState> _stateStack = new LinkedList<RCGameState>();

        public RCGameStateManager(Game game)
            : base(game)
        {
            game.Services.AddService(typeof(IRCGameStateManager), this);

            this.DrawOrder = 1;
            this.UpdateOrder = 1;
        }

        public event StateChangeHandler StateChanged;

        public void PushState(RCGameState newState)
        {
            RCGameState previousState = null;

            if(_stateStack.Count != 0)
            {
                previousState = _stateStack.First.Value;
                previousState.Unactivate();
            }

            newState.Initialize();
            _stateStack.AddFirst(newState);
            newState.Activate();

            FireStateChangedEvent(newState, previousState); 
        }

        public RCGameState PopState()
        {
            if (_stateStack.Count == 0) return null;

            RCGameState previousState = _stateStack.First.Value;
            previousState.Unactivate();
            _stateStack.RemoveFirst();

            RCGameState newState = null;
            if (_stateStack.Count >= 1)
            {
                newState = _stateStack.First.Value;
                newState.Activate();
            }

            FireStateChangedEvent(newState, previousState);

            return previousState;
        }

        public RCGameState PeekState()
        {
            if (_stateStack.Count == 0) return null;
            return _stateStack.First.Value;
        }

        public override void Draw(GameTime gameTime)
        {
            foreach (RCGameState state in _stateStack)
            {
                if (!state.IsVisible) continue;

                state.Draw(gameTime);
            }

            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            foreach (RCGameState state in _stateStack)
            {
                if (!state.IsUpdateable) continue;

                state.Update(gameTime);
            }

            base.Update(gameTime);
        }

        private void FireStateChangedEvent(RCGameState newState, RCGameState oldState)
        {
            if (StateChanged != null)
            {
                StateChanged(newState, oldState);
            }
        }
    }
}
