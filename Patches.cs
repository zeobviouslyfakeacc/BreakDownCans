using MelonLoader;
using System;
using Harmony;
using UnityEngine;

namespace BreakDownCans {
	internal class Patches : MelonMod {

		private const string RECYCLED_CAN_NAME = "GAMEPLAY_RecycledCan";
		private const string SCRAP_METAL_NAME = "GEAR_ScrapMetal";
		private const string HARVEST_AUDIO = "Play_HarvestingGeneric";

		public static void OnLoad() {
			Debug.Log("[BreakDownCans] Loaded!");
		}

		[HarmonyPatch(typeof(GearItem), "Awake", new Type[0])]
		private static class Main {
			private static void Prefix(GearItem __instance) {
				GameObject gameObject = __instance.gameObject;

				if (__instance.m_LocalizedDisplayName?.m_LocalizationID == RECYCLED_CAN_NAME && !gameObject.GetComponent<Harvest>()) {
					GameObject scrapMetalObject = Resources.Load<GameObject>(SCRAP_METAL_NAME);
					GearItem scrapMetalItem = scrapMetalObject?.GetComponent<GearItem>();
					if (!scrapMetalItem)
						return;

					Harvest harvest = gameObject.AddComponent<Harvest>();
					harvest.m_YieldGear = new GearItem[] { scrapMetalItem };
					harvest.m_YieldGearUnits = new int[] { 1 };
					harvest.m_DurationMinutes = 10;
					harvest.m_Audio = HARVEST_AUDIO;
				}
			}
		}
	}
}
