
using RC.Engine.Rendering.EffectConstants;
using RC.Engine.SceneGraph;
using Microsoft.Xna.Framework;
using RC.Engine.Cameras;

namespace RC.Engine.Rendering.EffectConstants
{
    public class RCLightConstant : RCEffectConstant
    {
        protected RCLight Light { get; set; }

        public RCLightConstant(RCLight light)
        {
            Light = light;
        }
    }

    public class RCLightWorldPositionConstant : RCLightConstant
    {
        public RCLightWorldPositionConstant(RCLight light)
            : base(light) { }

        public override void Update(RCVisual visual, RCCamera camera)
        {
            Parameter.SetValue(Light.Transform.Translation);
        }
    }

    public class RCLightWorldDirectionConstant : RCLightConstant
    {
        public RCLightWorldDirectionConstant(RCLight light)
            : base(light) { }

        public override void Update(RCVisual visual, RCCamera camera)
        {
            Parameter.SetValue(Light.Transform.Forward);
        }
    }

    public class RCLightAmbientColorConstant : RCLightConstant
    {
        public RCLightAmbientColorConstant(RCLight light)
            : base(light) { }

        public override void Update(RCVisual visual, RCCamera camera)
        {
            Parameter.SetValue(Light.Ambient);
        }
    }

    public class RCLightDiffuseColorConstant : RCLightConstant
    {
        public RCLightDiffuseColorConstant(RCLight light)
            : base(light) { }

        public override void Update(RCVisual visual, RCCamera camera)
        {
            Parameter.SetValue(Light.Diffuse);
        }
    }

    public class RCLightSpecularColorConstant : RCLightConstant
    {
        public RCLightSpecularColorConstant(RCLight light)
            : base(light) { }

        public override void Update(RCVisual visual, RCCamera camera)
        {
            Parameter.SetValue(Light.Specular);
        }
    }

    public class RCLightIntensityConstant : RCLightConstant
    {
        public RCLightIntensityConstant(RCLight light)
            : base(light) { }

        public override void Update(RCVisual visual, RCCamera camera)
        {
            Parameter.SetValue(Light.Intensity);
        }
    }
}