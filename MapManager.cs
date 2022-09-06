﻿using UnityEngine;
using UnityEngine.SceneManagement;
using Pathfinding;
using Landfall.TABS;
using System.Collections;
using System.Linq;
using Landfall.TABS.UnitEditor;
using System.Collections.Generic;

namespace MapMayhem
{ 
    public class MapManager : MonoBehaviour
    {
        public MapManager()
        {
            SceneManager.sceneLoaded += SceneLoaded;
        }

        public void SceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
        {
            if (scene.path == "Assets/11 Scenes/MainMenu.unity" && !doneStealing)
            {
                StartCoroutine(LoadAsync());
            }
            if (scene.name.Contains("MM_"))
            {
                GameObject astar = null;
                GameObject map = null;
                foreach (var obj in scene.GetRootGameObjects())
                {
                    if (obj.name == "AStar_Lvl1_Grid")
                    {
                        astar = obj;
                    }
                    if (obj.GetComponent<MapSettings>())
                    {
                        map = obj;
                    }
                    if (obj.name == "Water")
                    {
                        obj.GetComponent<MeshRenderer>().material = MMMain.wet;
                    }
                    if (obj.name == "WaterManager")
                    {
                        obj.GetComponent<PirateWaterManager>().WaterMaterial = MMMain.wet;
                    }
                }
                if (astar != null && map != null)
                {
                    var path = astar.GetComponentInChildren<AstarPath>(true);
                    astar.SetActive(true);
                    if (path.data.graphs.Length > 0) { path.data.RemoveGraph(path.data.graphs[0]); }
                    path.data.AddGraph(typeof(RecastGraph));
                    path.data.recastGraph.minRegionSize = 0.1f;
                    path.data.recastGraph.characterRadius = 0.3f;
                    path.data.recastGraph.cellSize = 0.2f;
                    path.data.recastGraph.forcedBoundsSize = new Vector3(map.GetComponent<MapSettings>().m_mapRadius * 2f, map.GetComponent<MapSettings>().m_mapRadius * map.GetComponent<MapSettings>().mapRadiusYMultiplier * 2f, map.GetComponent<MapSettings>().m_mapRadius * 2f);
                    path.data.recastGraph.rasterizeMeshes = false;
                    path.data.recastGraph.rasterizeColliders = true;
                    path.Scan();
                    /*
                    path.data.GetNodes(delegate (GraphNode node)
                    {
                        GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        gameObject.transform.position = (Vector3)node.position;
                        gameObject.GetComponent<Renderer>().material.color = Color.green;
                        gameObject.GetComponent<Collider>().enabled = false;
                        gameObject.transform.localScale *= 0.5f;
                    });
                    */
                }
            }
        }
        public IEnumerator LoadAsync()
        {
            var async = SceneManager.LoadSceneAsync("08_Lvl1_Pirate_VC", LoadSceneMode.Additive);
            yield return new WaitUntil(() => async.isDone);
            if (SceneManager.GetSceneByName("08_Lvl1_Pirate_VC").isLoaded)
            {
                foreach (var obj in SceneManager.GetSceneByName("08_Lvl1_Pirate_VC").GetRootGameObjects())
                {
                    if (obj.name == "Map")
                    {
                        MMMain.wet = obj.transform.FindChildRecursive("Scene").FindChildRecursive("Stuff").FindChildRecursive("Terrain_Pirate_lvl1").FindChildRecursive("Water").GetComponent<MeshRenderer>().material;
                    }
                }
            }

            if (SceneManager.GetSceneByName("08_Lvl1_Pirate_VC").isLoaded)
            {
                async = SceneManager.UnloadSceneAsync("08_Lvl1_Pirate_VC");
                yield return new WaitUntil(() => async.isDone);
                SceneManager.LoadScene("Assets/11 Scenes/MainMenu.unity");
            }
            doneStealing = true;
            yield break;
        }

        public bool doneStealing;
    }
}
