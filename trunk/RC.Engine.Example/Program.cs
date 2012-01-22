using System;

namespace RC.Engine.Example
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (ExampleGame game = new ExampleGame())
            {
                game.Run();
            }
        }
    }
}

