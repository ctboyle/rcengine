using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using RC.Engine.SceneGraph;
using RC.Engine.Cameras;


namespace RC.Engine.Rendering.EffectConstants
{
    public class RCEffectConstantCollection : IEnumerable<RCEffectConstant>
    {
        Dictionary<string, RCEffectConstant> _dictConstants = new Dictionary<string,RCEffectConstant>();

        public void AddConstant(string paramter, RCEffectConstant constant) 
        {
            _dictConstants[paramter] = constant;
        }

        public void Update(RCVisual visual, RCCamera camera)
        {
            foreach (RCEffectConstant constant in this)
            {
                constant.Update(visual, camera);
            }
        }

        public IEnumerator<RCEffectConstant> GetEnumerator()
        {
            return _dictConstants.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _dictConstants.Values.GetEnumerator();
        }
    }

    public abstract class RCEffectConstant
    {
        public EffectParameter Parameter {get; set;}

        virtual public void Update(RCVisual visual, RCCamera camera)
        {
            // Stub for derrived classes 
        }
    }
}
