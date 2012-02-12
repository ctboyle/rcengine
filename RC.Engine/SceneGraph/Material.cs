using Microsoft.Xna.Framework;

public class Material
{
    public Vector4 Emissive = new Vector4(0,0,0,1);  // default: (0,0,0,1)
    public Vector4 Ambient  = new Vector4(0,0,0,1);  // default: (0,0,0,1)

    // The material alpha is the alpha channel of mDiffuse.
    public Vector4 Diffuse  = new Vector4(0,0,0,1);  // default: (0,0,0,1)

    // The material specular exponent is in the alpha channel of mSpecular.
    public Vector4 Specular = new Vector4(0,0,0,0);  // default: (0,0,0,0)
}