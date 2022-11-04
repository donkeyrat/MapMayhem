using Landfall.TABS;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using System.Reflection;
using DM;

namespace MapMayhem
{
    public class MMMain
    {
        public MMMain()
        {
            var db = ContentDatabase.Instance();
            
            
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
            AssetBundle.LoadFromMemory(Properties.Resources.lasertag);
            AssetBundle.LoadFromMemory(Properties.Resources.whirlwind);
            AssetBundle.LoadFromMemory(Properties.Resources.balancing);
            AssetBundle.LoadFromMemory(Properties.Resources.quadbridges);
            AssetBundle.LoadFromMemory(Properties.Resources.towerdefense);
            AssetBundle.LoadFromMemory(Properties.Resources.blackhole);
            AssetBundle.LoadFromMemory(Properties.Resources.slide);
            AssetBundle.LoadFromMemory(Properties.Resources.laststand);
            AssetBundle.LoadFromMemory(Properties.Resources.bouncycastle);
            var newMapList = ((MapAsset[])typeof(LandfallContentDatabase).GetField("m_orderedMapAssets", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(db.LandfallContentDatabase)).ToList();
            var newMapDict = (Dictionary<DatabaseID, int>)typeof(LandfallContentDatabase).GetField("m_mapAssetIndexLookup", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(db.LandfallContentDatabase);

            foreach (var map in mapMayhem.LoadAllAssets<MapAsset>()) 
            {
                if (!newMapList.Contains(map) && !newMapDict.ContainsKey(map.Entity.GUID))
                {
                    newMapList.Add(map);
                    newMapDict.Add(map.Entity.GUID, newMapList.IndexOf(map));
                }
            }

            typeof(LandfallContentDatabase).GetField("m_orderedMapAssets", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(db.LandfallContentDatabase, newMapList.ToArray());
            typeof(LandfallContentDatabase).GetField("m_mapAssetIndexLookup", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(db.LandfallContentDatabase, newMapDict);
            
            foreach (var sb in mapMayhem.LoadAllAssets<SoundBank>()) 
            {
                if (sb.name.Contains("Sound")) {
                    var vsb = ServiceLocator.GetService<SoundPlayer>().soundBank;
                    foreach (var sound in sb.Categories) { sound.categoryMixerGroup = vsb.Categories[0].categoryMixerGroup; }
                    var cat = vsb.Categories.ToList();
                    cat.AddRange(sb.Categories);
                    vsb.Categories = cat.ToArray();
                }
                if (sb.name.Contains("Music")) {
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
                    vsb.Categories = cat.ToArray();
                }
            }

            foreach (var mat in mapMayhem.LoadAllAssets<Material>())
            {
                if (mat.shader.name == "Standard")
                {
                    mat.shader = Shader.Find("Standard");
                }
            }
            new GameObject
            {
                name = "SuperBullshit",
                hideFlags = HideFlags.HideAndDontSave
            }.AddComponent<MapManager>();
            
            new Harmony("UC+Coming2023").PatchAll();
        }

        public static AssetBundle mapMayhem = AssetBundle.LoadFromMemory(Properties.Resources.mapmayhem);
    }
}
