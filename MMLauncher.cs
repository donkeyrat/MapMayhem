using BepInEx;
using UnityEngine;

namespace MapMayhem
{
	[BepInPlugin("teamgrad.mapmayhem", "Map Mayhem", "3.0.2")]
	public class MMLauncher : BaseUnityPlugin
	{
		public MMLauncher()
		{
			MMBinder.UnitGlad();
		}
	}
}
