using RC.Engine.Rendering;
using Microsoft.Xna.Framework.Graphics;
using RC.Engine.Rendering.EffectConstants;
using RC.Engine.SceneGraph;

public class RCBasicEffect : RCVisualEffect
{
    public enum LightId 
    {
        One,
        Two,
        Three
    }

    public BasicEffect BasicEffect {get {return (BasicEffect)Effect;} }

    public RCBasicEffect(BasicEffect effect)
    {
        Effect = effect;
        effect.EnableDefaultLighting();

        SetConstant("World",      new RCWMatrixConstant());
        SetConstant("View",       new RCVMatrixConstant());
        SetConstant("Projection", new RCPMatrixConstant());
    }

    public void SetLight(LightId id, RCLight light) {
        switch (id)
        {
        case LightId.One:
            SetConstant("DirLight0Direction", new RCLightWorldDirectionConstant(light));
            SetConstant("DirLight0DiffuseColor", new RCLightDiffuseColorConstant(light));
            SetConstant("DirLight0SpecularColor", new RCLightSpecularColorConstant(light));
            break;
        case LightId.Two:
            SetConstant("DirLight1Direction", new RCLightWorldDirectionConstant(light));
            SetConstant("DirLight1DiffuseColor", new RCLightDiffuseColorConstant(light));
            SetConstant("DirLight1SpecularColor", new RCLightSpecularColorConstant(light));
            break;
        case LightId.Three:
            SetConstant("DirLight2Direction", new RCLightWorldDirectionConstant(light));
            SetConstant("DirLight2DiffuseColor", new RCLightDiffuseColorConstant(light));
            SetConstant("DirLight2SpecularColor", new RCLightSpecularColorConstant(light));
            break;
        }
    }
}