using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RC.Engine.SceneGraph;
using RC.Engine.Cameras;
using Microsoft.Xna.Framework.Graphics;

namespace RC.Engine.Rendering.EffectConstants
{
    class RCWMatrixConstant : RCEffectConstant
    {
        public override void Update(RCVisual visual, RCCamera camera)
        {
            Parameter.SetValue(visual.WorldTrans);
        }
    }

    class RCVWMatrixConstant : RCEffectConstant
    {
        public override void Update(RCVisual visual, RCCamera camera)
        {
            Parameter.SetValue(camera.View * visual.WorldTrans);
        }
    }

    class RCVMatrixConstant : RCEffectConstant
    {
        public override void Update(RCVisual visual, RCCamera camera)
        {
            Parameter.SetValue(camera.View);
        }
    }

    class RCPVWMatrixConstant : RCEffectConstant
    {
        public override void Update(RCVisual visual, RCCamera camera)
        {
            Parameter.SetValue(camera.Projection * camera.View * visual.WorldTrans);
        }
    }

    class RCPVMatrixConstant : RCEffectConstant
    {
        public override void Update(RCVisual visual, RCCamera camera)
        {
            Parameter.SetValue(camera.Projection * camera.View);
        }
    }

    class RCPMatrixConstant : RCEffectConstant
    {
        public override void Update(RCVisual visual, RCCamera camera)
        {
            Parameter.SetValue(camera.Projection);
        }
    }

}
