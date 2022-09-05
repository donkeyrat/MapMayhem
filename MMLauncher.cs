using BepInEx;
using UnityEngine;

namespace MapMayhem
{
	[BepInPlugin("teamgrad.mapmayhem", "Map Mayhem", "2.0.1")]
	public class MMLauncher : BaseUnityPlugin
	{
		public MMLauncher()
		{
			MMBinder.UnitGlad();
		}
	}
}
