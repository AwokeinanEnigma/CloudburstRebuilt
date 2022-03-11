using System;

namespace Cloudburst.Builders
{
    public abstract class Core<T> : Core where T : Core<T>
    {
        public static T instance { get; private set; }

        public Core()
        {
            if (instance != null) throw new InvalidOperationException("Singleton class \"" + typeof(T).Name + "\" was instantiated twice!");
            instance = this as T;
        }
    }
    /// <summary>
    /// Generic core class to inherit from. 
    /// Use this for when you have a core that can be loaded genericly, otherwise avoid this entirely.
    /// </summary>
    public abstract class Core
    {
        public Core() {
            CCUtilities.LogI($"Initializing: {Name}");
            CloudburstPlugin.instance.PluginStart += Start;
            OnLoaded();
        }

        /// <summary>
        /// The user-friendly name of the core. This will be used in the debug message for when the core is loaded.
        /// </summary>
        public abstract string Name{ get; }
        /// <summary>
        /// Does this core need to be loaded before other cores? If so, make sure this is true.
        /// </summary>
        public abstract bool Priority { get; }
        /// <summary>
        /// This void is invoked when the constructor is initalized. Aka, when you do a new MyCore(), this will be invoked.
        /// </summary>
        public abstract void OnLoaded();
        /// <summary>
        /// This is ran on the first frame of the game.
        /// </summary>
        public virtual void Start() { }

    }
}
