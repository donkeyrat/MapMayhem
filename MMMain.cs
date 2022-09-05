using Landfall.TABS;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using System.Reflection;

namespace MapMayhem
{
    public class MMMain
    {
        public MMMain()
        {
            AssetBundle.LoadFromMemory(Properties.Resources.badlands);
            AssetBundle.LoadFromMemory(Properties.Resources.beepic);
            AssetBundle.LoadFromMemory(Properties.Resources.cannonchaos);
            AssetBundle.LoadFromMemory(Properties.Resources.darksim);
            AssetBundle.LoadFromMemory(Properties.Resources.denmark);
            AssetBundle.LoadFromMemory(Properties.Resources.desert);
            AssetBundle.LoadFromMemory(Properties.Resources.galaxy);
            AssetBundle.LoadFromMemory(Properties.Resources.greensim);
            AssetBundle.LoadFromMemory(Properties.Resources.hills);
            AssetBundle.LoadFromMemory(Properties.Resources.japan1);
            AssetBundle.LoadFromMemory(Properties.Resources.japan2);
            AssetBundle.LoadFromMemory(Properties.Resources.japan3);
            AssetBundle.LoadFromMemory(Properties.Resources.neon);
            AssetBundle.LoadFromMemory(Properties.Resources.nightsim);
            AssetBundle.LoadFromMemory(Properties.Resources.pirate2);
            AssetBundle.LoadFromMemory(Properties.Resources.sahara);
            AssetBundle.LoadFromMemory(Properties.Resources.scotland1);
            AssetBundle.LoadFromMemory(Properties.Resources.scotland2);
            AssetBundle.LoadFromMemory(Properties.Resources.spaceisland);
            AssetBundle.LoadFromMemory(Properties.Resources.spikes);
            AssetBundle.LoadFromMemory(Properties.Resources.winter);
            var db = LandfallUnitDatabase.GetDatabase();
            foreach (var sb in mapMayhem.LoadAllAssets<SoundBank>()) {
                var vsb = ServiceLocator.GetService<MusicHandler>().bank;
                var cat = vsb.Categories.ToList();
                cat.AddRange(sb.Categories);
                foreach (var categ in sb.Categories) {
                    foreach (var sound in categ.soundEffects) {
                        var song = new SongInstance();
                        song.clip = sound.clipTypes[0].clips[0];
                        song.soundEffectInstance = sound;
                        song.songRef = categ.categoryName + "/" + sound.soundRef;
                        ServiceLocator.GetService<MusicHandler>().m_songs.Add(song.songRef, song);
                    }
                }
            }
            foreach (var map in mapMayhem.LoadAllAssets<MapAsset>())
            {
                db.MapList.AddItem(map);
                List<MapAsset> maps = (List<MapAsset>)typeof(LandfallUnitDatabase).GetField("Maps", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(db);
                maps.Add(map);
                typeof(LandfallUnitDatabase).GetField("Maps", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(db, maps);
            }
            foreach (var mat in mapMayhem.LoadAllAssets<Material>())
            {
                if (mat.shader.name == "Standard")
                {
                    mat.shader = Shader.Find("Standard");
                }
            }
            new GameObject()
            {
                name = "SuperBullshit",
                hideFlags = HideFlags.HideAndDontSave
            }.AddComponent<MapManager>();
        }

        public static AssetBundle mapMayhem = AssetBundle.LoadFromMemory(Properties.Resources.mapmayhem);

        public static Material wet;
    }
}
