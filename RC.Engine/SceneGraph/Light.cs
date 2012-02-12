using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace RC.Engine.SceneGraph
{
    /// <summary>
    /// A light object that can be used as a light source in the scene.
    /// </summary>
    public class RCLight
    {
        public enum LightType
        {
            Ambient,
            Directional,
            Point,
            Spot
        };

        public LightType Type = LightType.Ambient;

        /// <summary>
        /// Location in world space.
        /// </summary>
        public Matrix Transform = Matrix.Identity;


        public RCLight()
        {
            Type = LightType.Ambient;
        }

        public RCLight (LightType eType)
        {
            Type = eType;
        }

        // The colors of the light.
        public Vector4 Ambient  = new Vector4(0,0,0,1);
        public Vector4 Diffuse = new Vector4(0, 0, 0, 1);
        public Vector4 Specular = new Vector4(0, 0, 0, 1);  

        // Attenuation is typically specified as a modulator
        //     m = 1/(C + L*d + Q*d*d)
        // where C is the constant coefficient, L is the linear coefficient,
        // Q is the quadratic coefficient, and d is the distance from the light
        // position to the vertex position.  To allow for a linear adjustment of
        // intensity, the choice is to use instead
        //     m = I/(C + L*d + Q*d*d)
        // where I is an intensity factor.
        public float Constant = 1;
        public float Linear = 0;
        public float Quadratic = 0;
        public float Intensity = 1; 

        // Parameters for spot lights.  The cone angle must be in radians and
        // should satisfy 0 < Angle <= pi.
        public float Angle = MathHelper.Pi;
        public float CosAngle = -1;
        public float SinAngle = 0;
        public float Exponent = 1; 

        // A helper function that lets you set mAngle and have mCosAngle and
        // mSinAngle computed for you.
        public void SetAngle (float angle)
        {
            if (!(0.0f < angle && angle <= MathHelper.Pi))
            {
                throw new InvalidOperationException("Angle out of range in SetAngle.\n");
            }

            Angle = angle;
            CosAngle = (float)Math.Cos(angle);
            SinAngle = (float)Math.Sin(angle);
        }

        // A helper function that lets you set the direction vector and computes
        // the up and right vectors automatically.
        public void SetDirection(Vector3 direction)
        {
            Matrix lightLookAt = Matrix.CreateLookAt(new Vector3(0f, 25.0f, 25.0f), Vector3.Zero, Vector3.Up);
            this.Transform = Matrix.Invert(lightLookAt);
        }
    }
}
