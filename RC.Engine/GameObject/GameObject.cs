using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using RC.Engine.Base;
using System.Reflection;
using System.Collections.ObjectModel;
using System.Collections;

namespace RC.Engine.GameObject
{
    public class RCGameComponent : GameComponent
    {
        public RCGameObject GameObject 
        {
            get 
            { 
                return _gameObject; 
            } 
        }

        public virtual T GetComponent<T>() where T : RCGameComponent
        {
            return _gameObject.GetComponent<T>();
        }

        public static T CreateComponent<T>(RCGameObject gameObject) where T : RCGameComponent, new()
        {
            T newInstance = new T();
            newInstance._gameObject = gameObject;

            return newInstance;
        }

        protected RCGameComponent()
            : base(RCGame.Instance){}
        
        protected RCGameObject _gameObject;
    }

    public class RCGameObject : RCGameComponent
    {
        public RCGameObject()
        {
            _gameObject = this;
        }

        public sealed override T GetComponent<T>()
        {
            if (this is T)
            {
                T refer = this as T;
                return refer;
            }

            return (T)_components[typeof(T)];
        }

        public void AddComponent<T>() where T : RCGameComponent, new()
        {
            T newInstance = RCGameComponent.CreateComponent<T>(this);
 
            _components.AddComponent(newInstance);

            newInstance.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            foreach (RCGameComponent component in _components)
            {
                component.Update(gameTime);
            }

            base.Update(gameTime);
        }

        private RCGameComponentCollection _components = new RCGameComponentCollection();


  
    }
}
