using System;
using Microsoft.Xna.Framework;

namespace RC.Engine.Base
{
    /// <summary>
    /// Events arguments for GameTimeEventArgs.
    /// </summary>
    public class RCGameTimeEventArgs : EventArgs
    {
        public RCGameTimeEventArgs(GameTime gt) { GameTime = gt; }
        public GameTime GameTime;
    }
}
