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
    /// Use this for when you have a core that can be loaded safely, otherwise avoid this entirely.
    /// </summary>
    public abstract class Core
    {
        public Core() {
            CCUtilities.LogI($"Initializing: {Name}");
            OnLoaded();
        }

        /// <summary>
        /// The user-friendly name of the core. This will be used in the debug log for when the core is loaded.
        /// </summary>
        public abstract string Name{ get; }
        /// <summary>
        /// Does this core need to be loaded before other cores? If so, make sure this is true.
        /// </summary>
        public abstract bool Priority { get; }
        public virtual void OnLoaded() {
            CloudburstPlugin.instance.PluginStart += Start; }

        
        public virtual void Start() { }

    }
}
