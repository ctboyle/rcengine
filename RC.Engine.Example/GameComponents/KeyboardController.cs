using System;
using System.Collections.Generic;
using System.Text;

using RC.Engine.StateManagement;
using RC.Engine.Cameras;
using RC.Engine.GameObject;
using RC.Engine.SceneGraph;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RC.Engine.Example
{
    public class KeyboardController : RCGameComponent
    {
        RCSpatial spatial;   
        float speed;
        float rotSpeed;

        public override void Initialize()
        {
            spatial = GetComponent<RCSpatial>();
            speed = 10;
            rotSpeed = MathHelper.PiOver2;
        }

        public override void Update(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedRealTime.TotalSeconds;

            Vector3 dPos = new Vector3();
            Vector2 dRot = new Vector2();

            KeyboardState keys = Keyboard.GetState();
            foreach (Keys key in keys.GetPressedKeys())
            {
                switch (key)
                {
                    case Keys.Up:
                        dPos += spatial.LocalTrans.Forward;
                        break;
                    case Keys.Left:
                        dPos += spatial.LocalTrans.Left;
                        break;
                    case Keys.Down:
                        dPos += spatial.LocalTrans.Backward;
                        break;
                    case Keys.Right:
                        dPos += spatial.LocalTrans.Right;
                        break;
                    case Keys.W:
                        dRot -= Vector2.UnitX;
                        break;
                    case Keys.S:
                        dRot += Vector2.UnitX;
                        break;
                    case Keys.A:
                        dRot += Vector2.UnitY;
                        break;
                    case Keys.D:
                        dRot -= Vector2.UnitY;
                        break;
                }
            }


            if (dRot != Vector2.Zero || dPos != Vector3.Zero)
            {
                Vector3 scale;
                Vector3 pos;
                Quaternion rot;
                spatial.LocalTrans.Decompose(out scale, out rot, out pos);


                pos += dPos * speed * dt;

                dRot = dRot * rotSpeed * dt;

                spatial.LocalTrans =
                    Matrix.CreateFromQuaternion(rot) *
                    Matrix.CreateFromAxisAngle(Vector3.Up, dRot.Y) *
                    Matrix.CreateFromAxisAngle(spatial.LocalTrans.Right, dRot.X) *
                    Matrix.CreateTranslation(pos);
            }
                
        }
    }
}