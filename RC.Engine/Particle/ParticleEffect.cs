//using System;
//using System.Collections.Generic;
//using System.Text;

//using RC.Engine.ContentManagement;
//using RC.Engine.Rendering;

//using Microsoft.Xna.Framework.Graphics;
//using Microsoft.Xna.Framework;


//namespace RC.Engine.Particle
//{
//    public class ParticleEffect
//    {
//        public ParticleSettings Settings;
//        public EffectParameter effectTimeParameter;

//        public ParticleEffect()
//        {
//        }

//        public RCVisualEffectInstance CreateInstance()
//        {
//            RCDefaultContent<Effect> effect = new RCDefaultContent<Effect>(@"Content\ParticleEffect");

//            RCVisualEffectInstance instance = new RCVisualEffectInstance(effect);
//            EffectParameterCollection parameters = instance.Effect.Parameters;

//            // Look up shortcuts for parameters that change every frame.

//            effectTimeParameter = parameters["CurrentTime"];

//            // Set the values of parameters that do not change.
//            parameters["Duration"].SetValue((float)Settings.Duration.TotalSeconds);
//            parameters["DurationRandomness"].SetValue(Settings.DurationRandomness);
//            parameters["Gravity"].SetValue(Settings.Gravity);
//            parameters["EndVelocity"].SetValue(Settings.EndVelocity);
//            parameters["MinColor"].SetValue(Settings.MinColor.ToVector4());
//            parameters["MaxColor"].SetValue(Settings.MaxColor.ToVector4());

//            parameters["RotateSpeed"].SetValue(
//                new Vector2(Settings.MinRotateSpeed, Settings.MaxRotateSpeed));

//            parameters["StartSize"].SetValue(
//                new Vector2(Settings.MinStartSize, Settings.MaxStartSize));

//            parameters["EndSize"].SetValue(
//                new Vector2(Settings.MinEndSize, Settings.MaxEndSize));

//            // Load the particle texture, and set it onto the effect.
//            parameters["Texture"].SetValue(Settings.texture.Content);

//            // Choose the appropriate effect technique. If these particles will never
//            // rotate, we can use a simpler pixel shader that requires less GPU power.
//            string techniqueName;

//            if ((Settings.MinRotateSpeed == 0) && (Settings.MaxRotateSpeed == 0))
//                techniqueName = "NonRotatingParticles";
//            else
//                techniqueName = "RotatingParticles";

//            instance.Effect.CurrentTechnique = Effect.Techniques[techniqueName];

//            return instance;
//        }
//    }
//}
