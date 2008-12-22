using System;
using Microsoft.Xna.Framework;

namespace RC.Engine.Base
{
    /// <summary>
    /// Events arguments for GameTimeEventArgs.
    /// </summary>
    public class GameTimeEventArgs : EventArgs
    {
        public GameTimeEventArgs(GameTime gt) { GameTime = gt; }
        public GameTime GameTime;
    }
}
