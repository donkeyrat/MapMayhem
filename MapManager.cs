using UnityEngine;
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
                        var shadersToReplace = new List<MeshRenderer>(obj.GetComponentsInChildren<MeshRenderer>(true)
                            .ToList().FindAll(x => x.name.Contains("_ReplaceMe")));
                        foreach (var rend in shadersToReplace)
                        {
                            rend.material.shader = Shader.Find(rend.material.shader.name);
                            if (rend.GetComponent<PiratePlacementTransparency>())
                            {
                                rend.GetComponent<PiratePlacementTransparency>().Materials[0].m_oldMaterial.shader =
                                    Shader.Find(rend.GetComponent<PiratePlacementTransparency>().Materials[0]
                                        .m_oldMaterial.shader.name);
                            }
                        }
                    }
                    if (obj.name.Contains("_ReplaceMe"))
                    {
                        obj.GetComponent<MeshRenderer>().material.shader =
                            Shader.Find(obj.GetComponent<MeshRenderer>().material.shader.name);
                    }
                    if (obj.name == "WaterManager")
                    {
                        obj.GetComponent<PirateWaterManager>().WaterMaterial =
                            obj.transform.parent.GetComponent<MeshRenderer>().material;
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

                    //path.data.GetNodes(delegate (GraphNode node)
                    //{
                    //    GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    //    gameObject.transform.position = (Vector3)node.position;
                    //    gameObject.GetComponent<Renderer>().material.color = Color.green;
                    //    gameObject.GetComponent<Collider>().enabled = false;
                    //    gameObject.transform.localScale *= 0.5f;
                    //});

                }
            }
        }
    }
}
