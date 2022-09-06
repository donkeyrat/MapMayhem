using BepInEx;
using UnityEngine;

namespace MapMayhem
{
	[BepInPlugin("teamgrad.mapmayhem", "Map Mayhem", "2.1.0")]
	public class MMLauncher : BaseUnityPlugin
	{
		public MMLauncher()
		{
			MMBinder.UnitGlad();
		}
	}
}
