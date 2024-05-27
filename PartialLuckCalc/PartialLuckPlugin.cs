using BepInEx;
using R2API;

namespace PartialLuckPlugin
{
    // Dependencies
    [BepInDependency(ItemAPI.PluginGUID)]
    [BepInDependency(LanguageAPI.PluginGUID)]

    // This attribute is required, and lists metadata for your plugin.
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]

    public class PartialLuckPlugin : BaseUnityPlugin
    {
        public const string PluginGUID = PluginAuthor + "." + PluginName;
        public const string PluginAuthor = "shirograhm";
        public const string PluginName = "PartialLuckPlugin";
        public const string PluginVersion = "1.0.0";

        public static PluginInfo PInfo { get; private set; }

        internal PartialLuckHook partial;

        public void Awake()
        {
            PInfo = Info;
            Log.Init(Logger);
            
            partial = new PartialLuckHook();

            Log.Message("Finished initializations.");
        }
    }
}
