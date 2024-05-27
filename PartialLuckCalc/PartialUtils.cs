using UnityEngine;

namespace PartialLuckPlugin
{
    public class PartialUtils
    {
		public static float GetChanceAfterLuck(float percent, float luck)
		{
			if (luck > 0)
				return 1f - Mathf.Pow(1f - percent, luck + 1);
			if (luck < 0)
				return Mathf.Pow(percent, Mathf.Abs(luck) + 1);

			return percent;
		}
	}
}
