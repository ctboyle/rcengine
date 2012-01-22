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
        /// I push registered states onto the stack by name.
        /// </summary>
        /// <param name="stateLabel">The state name.</param>
        void PushState(string stateLabel);

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

        /// <summary>
        /// I return named states that have been registered with <see cref=" AddNamedState"/>.
        /// </summary>
        /// <param name="label">The name of the state</param>
        /// <returns>The named state instance assigned to given label. </returns>
        RCGameState GetNamedState(string label);

        /// <summary>
        /// I register state instances to be perserved and tracked by the manager. You may
        /// refer to the state by name,
        /// </summary>
        /// <param name="label">Same of the state</param>
        /// <param name="state">The state</param>
        void AddNamedState(string label, RCGameState state);

        /// <summary>
        /// Removes a state instance from the state manager by name
        /// </summary>
        /// <param name="label">Name of state</param>
        void RemoveNamedState(string label);
    }

    internal class RCGameStateManager : DrawableGameComponent, IRCGameStateManager
    {
        private Dictionary<string, RCGameState> _states = new Dictionary<string, RCGameState>();
        private LinkedList<RCGameState> _stateStack = new LinkedList<RCGameState>();

        public RCGameStateManager(Game game)
            : base(game)
        {
            game.Services.AddService(typeof(IRCGameStateManager), this);

            this.DrawOrder = 1;
            this.UpdateOrder = 1;
        }

        public event StateChangeHandler StateChanged;

        public void AddNamedState(string label, RCGameState state)
        {
            _states.Add(label, state);
            state.Initialize();
        }
		
		public void RemoveNamedState(string label)
        {
            RCGameState removeState = _states[label];
            removeState.Dispose();
            _states.Remove(label);
        }
		
		public RCGameState GetNamedState(string label)
		{
			return _states[label];
		}
		
        public void PushState(string label)
        {
            RCGameState nextState = GetNamedState(label);
            RCGameState previousState = null;

            if(_stateStack.Count != 0)
            {
                previousState = _stateStack.First.Value;
                previousState.Unactivate();
            }

            _stateStack.AddFirst(nextState);
            nextState.Activate();

            FireStateChangedEvent(nextState, previousState); 
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
		
		protected override void UnloadContent()
        {
            _states.Clear();
            base.UnloadContent();
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
