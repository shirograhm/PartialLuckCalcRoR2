using RoR2;
using MonoMod.RuntimeDetour;
using UnityEngine;

namespace PartialLuckPlugin
{
    internal class PartialLuckHook
    {
        private static Hook hook;
    
        public PartialLuckHook()
        {
            Setup();
        }

        public void Setup()
        {
			var replaceable = typeof(Util).GetMethod(nameof(Util.CheckRoll), new[] { typeof(float), typeof(float), typeof(CharacterMaster) });
			var replacement = typeof(PartialLuckHook).GetMethod(nameof(CheckPartialRoll), System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
			hook = new Hook(replaceable, replacement);
        }

        internal static bool CheckPartialRoll(float percent, float luck, CharacterMaster master)
        {
			Log.Debug($"Using partial roll calculation for a proc with {percent:F2}% chance.");
			if (percent <= 0f) return false;

			float basePercent = percent / 100f;
			float percentWithLuck = PartialUtils.GetChanceAfterLuck(basePercent, luck);
			float randomRoll = Random.Range(0f, 1f);

			if (randomRoll <= percentWithLuck)
			{
				if (randomRoll > basePercent && master)
				{
					GameObject bodyObject = master.GetBodyObject();
					if (bodyObject)
					{
						CharacterBody component = bodyObject.GetComponent<CharacterBody>();
						if (component)
						{
							component.wasLucky = true;
						}
					}
				}
				return true;
			}
			return false;
		}
	}
}
