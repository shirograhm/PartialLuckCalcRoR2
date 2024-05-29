using BepInEx;
using R2API;
using R2API.Networking;
using R2API.Utils;
using RoR2;

namespace PartialLuckPlugin
{
    // Dependencies
    [BepInDependency(ItemAPI.PluginGUID)]
    [BepInDependency(RecalculateStatsAPI.PluginGUID)]
    // Soft Dependencies
    [BepInDependency("com.droppod.lookingglass", BepInDependency.DependencyFlags.SoftDependency)]
    // Compatibility
    [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.EveryoneNeedSameModVersion)]
    
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

            ItemCatalog.availability.CallWhenAvailable(() => {
                partial = new PartialLuckHook();
                LookingGlassIntegration.Init();
            });

            NetworkingAPI.RegisterMessageType<PartialLuckTracker.Sync>();

            CharacterMaster.onStartGlobal += (obj) =>
            {
                obj?.gameObject.AddComponent<PartialLuckTracker>();
            };

            RecalculateStatsAPI.GetStatCoefficients += (sender, args) =>
            {
                if (sender.master && sender.master.gameObject)
                {
                    PartialLuckTracker tracker = sender.master.gameObject.GetComponent<PartialLuckTracker>();
                    if (tracker && tracker.PartialLuck != 0)
                    {
                        tracker.PartialLuck = 0;
                    }
                }
            };

            Log.Message("Finished initializations.");
        }
    }
}
