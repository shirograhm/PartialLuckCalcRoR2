using LookingGlass.ItemStatsNameSpace;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace PartialLuckPlugin
{
    class LookingGlassIntegration
    {
        internal static bool lookingGlassEnabled = false;

        internal static void Init()
        {
            var pluginInfos = BepInEx.Bootstrap.Chainloader.PluginInfos;
            if (pluginInfos.ContainsKey("droppod.lookingglass"))
            {
                try
                {
                    Log.Debug("Running LookingGlass integration for PartialLuckPlugin.");
                    IntegrationHook();
                    lookingGlassEnabled = true;
                }
                catch (Exception e)
                {
                    Log.Error(e);
                }
            }
        }

        internal static void IntegrationHook()
        {
            RoR2Application.onLoad += () =>
            {
                ItemStatsDef stats;

                // Sticky Bomb
                if (ItemDefinitions.allItemDefinitions.Remove((int)RoR2Content.Items.StickyBomb.itemIndex))
                {
                    stats = new ItemStatsDef();
                    stats.descriptions.Add("Sticky Bomb Chance: ");
                    stats.valueTypes.Add(ItemStatsDef.ValueType.Damage);
                    stats.measurementUnits.Add(ItemStatsDef.MeasurementUnits.Percentage);
                    stats.calculateValues = (master, itemCount) =>
                    {
                        return new List<float> { PartialUtils.GetChanceAfterLuck(0.05f * itemCount, master.luck) };
                    };
                    ItemDefinitions.allItemDefinitions.Add((int)RoR2Content.Items.StickyBomb.itemIndex, stats);
                }

                // Stun Grenade
                if (ItemDefinitions.allItemDefinitions.Remove((int)RoR2Content.Items.StunChanceOnHit.itemIndex))
                {
                    stats = new ItemStatsDef();
                    stats.descriptions.Add("Stun Chance: ");
                    stats.valueTypes.Add(ItemStatsDef.ValueType.Utility);
                    stats.measurementUnits.Add(ItemStatsDef.MeasurementUnits.Percentage);
                    stats.calculateValues = (master, itemCount) =>
                    {
                        return new List<float> { PartialUtils.GetChanceAfterLuck(PartialUtils.GetHyperbolicStacking(0.05f, itemCount), master.luck) };
                    };
                    ItemDefinitions.allItemDefinitions.Add((int)RoR2Content.Items.StunChanceOnHit.itemIndex, stats);
                }

                // Tri-Tip Dagger
                if (ItemDefinitions.allItemDefinitions.Remove((int)RoR2Content.Items.BleedOnHit.itemIndex))
                {
                    stats = new ItemStatsDef();
                    stats.descriptions.Add("Bleed Chance: ");
                    stats.valueTypes.Add(ItemStatsDef.ValueType.Damage);
                    stats.measurementUnits.Add(ItemStatsDef.MeasurementUnits.Percentage);
                    stats.calculateValues = (master, itemCount) =>
                    {
                        return new List<float> { PartialUtils.GetChanceAfterLuck(.1f * itemCount, master.luck) };
                    };
                    ItemDefinitions.allItemDefinitions.Add((int)RoR2Content.Items.BleedOnHit.itemIndex, stats);
                }

                // AtG Missile Mk. 1
                if (ItemDefinitions.allItemDefinitions.Remove((int)RoR2Content.Items.Missile.itemIndex))
                {
                    stats = new ItemStatsDef();
                    stats.descriptions.Add("Fire Chance: ");
                    stats.valueTypes.Add(ItemStatsDef.ValueType.Utility);
                    stats.measurementUnits.Add(ItemStatsDef.MeasurementUnits.Percentage);
                    stats.descriptions.Add("Damage: ");
                    stats.valueTypes.Add(ItemStatsDef.ValueType.Damage);
                    stats.measurementUnits.Add(ItemStatsDef.MeasurementUnits.Percentage);
                    stats.calculateValues = (master, itemCount) =>
                    {
                        return new List<float> { PartialUtils.GetChanceAfterLuck(.1f, master.luck), 3 * itemCount };
                    };
                    ItemDefinitions.allItemDefinitions.Add((int)RoR2Content.Items.Missile.itemIndex, stats);
                }

                // Bandolier
                if (ItemDefinitions.allItemDefinitions.Remove((int)RoR2Content.Items.Bandolier.itemIndex))
                {
                    stats = new ItemStatsDef();
                    stats.descriptions.Add("Drop Chance: ");
                    stats.valueTypes.Add(ItemStatsDef.ValueType.Utility);
                    stats.measurementUnits.Add(ItemStatsDef.MeasurementUnits.Percentage);
                    stats.calculateValues = (master, itemCount) =>
                    {
                        return new List<float> { PartialUtils.GetChanceAfterLuck(1 - (1 / Mathf.Pow(1 + itemCount, 0.33f)), master.luck) };
                    };
                    ItemDefinitions.allItemDefinitions.Add((int)RoR2Content.Items.Bandolier.itemIndex, stats);
                }

                // Ghor's Tome
                if (ItemDefinitions.allItemDefinitions.Remove((int)RoR2Content.Items.BonusGoldPackOnKill.itemIndex))
                {
                    stats = new ItemStatsDef();
                    stats.descriptions.Add("Drop Chance: ");
                    stats.valueTypes.Add(ItemStatsDef.ValueType.Utility);
                    stats.measurementUnits.Add(ItemStatsDef.MeasurementUnits.Percentage);
                    stats.calculateValues = (master, itemCount) =>
                    {
                        return new List<float> { PartialUtils.GetChanceAfterLuck(.04f * itemCount, master.luck) };
                    };
                    ItemDefinitions.allItemDefinitions.Add((int)RoR2Content.Items.BonusGoldPackOnKill.itemIndex, stats);
                }

                // Happiest Mask
                if (ItemDefinitions.allItemDefinitions.Remove((int)RoR2Content.Items.GhostOnKill.itemIndex))
                {
                    stats = new ItemStatsDef();
                    stats.descriptions.Add("Spawn Chance: ");
                    stats.valueTypes.Add(ItemStatsDef.ValueType.Utility);
                    stats.measurementUnits.Add(ItemStatsDef.MeasurementUnits.Percentage);
                    stats.descriptions.Add("Ghost Duration: ");
                    stats.valueTypes.Add(ItemStatsDef.ValueType.Damage);
                    stats.measurementUnits.Add(ItemStatsDef.MeasurementUnits.Seconds);
                    stats.calculateValues = (master, itemCount) =>
                    {
                        return new List<float> { PartialUtils.GetChanceAfterLuck(.07f, master.luck), itemCount * 30 };
                    };
                    ItemDefinitions.allItemDefinitions.Add((int)RoR2Content.Items.GhostOnKill.itemIndex, stats);
                }

                // Sentient Meat Hook
                if (ItemDefinitions.allItemDefinitions.Remove((int)RoR2Content.Items.BounceNearby.itemIndex))
                {
                    stats = new ItemStatsDef();
                    stats.descriptions.Add("Hook Chance: ");
                    stats.valueTypes.Add(ItemStatsDef.ValueType.Utility);
                    stats.measurementUnits.Add(ItemStatsDef.MeasurementUnits.Percentage);
                    stats.descriptions.Add("Hook Damage: ");
                    stats.valueTypes.Add(ItemStatsDef.ValueType.Damage);
                    stats.measurementUnits.Add(ItemStatsDef.MeasurementUnits.Percentage);
                    stats.calculateValues = (master, itemCount) =>
                    {
                        return new List<float> { PartialUtils.GetChanceAfterLuck(PartialUtils.GetHyperbolicStacking(0.2f, itemCount), master.luck), itemCount };
                    };
                    ItemDefinitions.allItemDefinitions.Add((int)RoR2Content.Items.BounceNearby.itemIndex, stats);
                }

                // Molten Perforator
                if (ItemDefinitions.allItemDefinitions.Remove((int)RoR2Content.Items.FireballsOnHit.itemIndex))
                {
                    stats = new ItemStatsDef();
                    stats.descriptions.Add("Trigger Chance: ");
                    stats.valueTypes.Add(ItemStatsDef.ValueType.Utility);
                    stats.measurementUnits.Add(ItemStatsDef.MeasurementUnits.Percentage);
                    stats.descriptions.Add("Damage: ");
                    stats.valueTypes.Add(ItemStatsDef.ValueType.Damage);
                    stats.measurementUnits.Add(ItemStatsDef.MeasurementUnits.Percentage);
                    stats.calculateValues = (master, itemCount) =>
                    {
                        return new List<float> { PartialUtils.GetChanceAfterLuck(0.1f, master.luck), itemCount * 3 };
                    };
                    ItemDefinitions.allItemDefinitions.Add((int)RoR2Content.Items.FireballsOnHit.itemIndex, stats);
                }

                // Charged Perforator
                if (ItemDefinitions.allItemDefinitions.Remove((int)RoR2Content.Items.LightningStrikeOnHit.itemIndex))
                {
                    stats = new ItemStatsDef();
                    stats.descriptions.Add("Trigger Chance: ");
                    stats.valueTypes.Add(ItemStatsDef.ValueType.Utility);
                    stats.measurementUnits.Add(ItemStatsDef.MeasurementUnits.Percentage);
                    stats.descriptions.Add("Damage: ");
                    stats.valueTypes.Add(ItemStatsDef.ValueType.Damage);
                    stats.measurementUnits.Add(ItemStatsDef.MeasurementUnits.Percentage);
                    stats.calculateValues = (master, itemCount) =>
                    {
                        return new List<float> { PartialUtils.GetChanceAfterLuck(0.1f, master.luck), itemCount * 5 };
                    };
                    ItemDefinitions.allItemDefinitions.Add((int)RoR2Content.Items.LightningStrikeOnHit.itemIndex, stats);
                }
            };
        }
    }
}