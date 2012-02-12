using System;
using System.Collections.Generic;
using System.Text;
using RC.Engine.Rendering;
using RC.Engine.ContentManagement;
using RC.Engine.Rendering.EffectConstants;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;


namespace RC.Engine.SceneEffects
{
    public class RCTextureEffect : RCVisualEffect
    {
        private RCDefaultContent<Effect> _textureEffectContent;
        public RCTextureEffect(Texture2D Texture)
        {
            _textureEffectContent = new RCDefaultContent<Effect>(
                @"Content\ShaderEffects\Texture");

            Effect = _textureEffectContent.Content;

            SetTexture("xTexture", Texture);
            SetConstant("xWorldViewProjection", new RCPVWMatrixConstant());
        }
    }
}