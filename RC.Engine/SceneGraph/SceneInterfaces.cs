using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using RC.Engine.SceneGraph.BoundingVolumes;

namespace RC.Engine.SceneGraph
{
    public interface INode
    {
        List<ISpatial> GetChildren();
    }

    public interface ISpatial
    {
        Matrix LocalTrans { get; set; }
        Matrix WorldTrans { get; }
        IRCBoundingVolume WorldBound { get; }
    }
}