using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework.Graphics;

using RC.Engine.Rendering.EffectConstants;

namespace RC.Engine.Rendering
{
    public class RCVisualEffect
    {
        private RCEffectConstantCollection _constants;

        public RCEffectConstantCollection EffectConstants { get { return _constants; } }
        public Effect Effect { get; set; }

        public RCVisualEffect()
        {
            _constants = new RCEffectConstantCollection();
        }

        public void SetConstant(string paramName, RCEffectConstant constant)
        {
            EffectParameter param = Effect.Parameters[paramName];

            if (param == null)
            {
                throw new InvalidOperationException("Paramater on Effect not found.");
            }

            constant.Parameter = param;

            _constants.AddConstant(paramName, constant);
        }

        public void SetTexture(string paramName, Texture2D texture)
        {
            Effect.Parameters[paramName].SetValue(texture);
        }
    }
}
