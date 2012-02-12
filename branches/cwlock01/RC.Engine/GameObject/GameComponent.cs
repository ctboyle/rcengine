using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using RC.Engine.Base;
using System.Reflection;
using RC.Engine.SceneGraph;
using System.Collections;

namespace RC.Engine.GameObject
{
    class RCGameComponentCollection : IEnumerable<RCGameComponent>
    {
        public void AddComponent<T>(T component) where T : RCGameComponent
        {
            if (_components.ContainsKey(typeof(T)))
            {
                throw new InvalidOperationException("Cant do that");
            }

            _components.Add(typeof(T), component);
        }

        public RCGameComponent this[Type t]
        {
            get
            {
                return _components[t];
            }
        }

        public T GetComponent<T>() where T : RCGameComponent

        {
            return (T)_components[typeof(T)];
        }

        public IEnumerator<RCGameComponent> GetEnumerator()
        {
            return _components.Values.GetEnumerator();
        }

        IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _components.Values.GetEnumerator();
        }

        private Dictionary<Type, RCGameComponent> _components = new Dictionary<Type, RCGameComponent>();
    }
}
