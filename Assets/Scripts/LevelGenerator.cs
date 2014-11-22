using UnityEngine;
using System.Collections;

public class LevelGenerator : MonoBehaviour
{
    // Nombre de blovks à générer d'avance
    public int MeshesToGenerate;


    #region Comportements Unity
    void Start()
    {
        StartCoroutine(DebugSpawnLoop());
    }

    void Update()
    {

    }
    #endregion

#if UNITY_EDITOR
    #region DEBUG

    public GameObject DebugSpawn;
    public Vector3 SpawnPoint;
    private IEnumerator DebugSpawnLoop()
    {
        while(true)
        {
            TriggerBlockGeneration();
            SpawnPoint += new Vector3(1F, 0F, 0F);
            yield return new WaitForSeconds(0.5F);
        }
    }
    #endregion
#endif

    #region
    /// <summary>
    /// Ajoute un block à la suite du niveua
    /// </summary>
    private void AddLevelBlock()
    {
        // TODO
        var spawn = (GameObject)GameObject.Instantiate(DebugSpawn, SpawnPoint, Quaternion.identity);
        var r = 255F / (float)Random.Range(0, 255);
        var v = 255F / (float)Random.Range(0, 255);
        var b = 255F / (float)Random.Range(0, 255);
        spawn.GetComponent<MeshRenderer>().material.color = new Color(r, v, b);
    }
    #endregion

    #region Evenements
    /// <summary>
    /// Lance la génération d'un nouveau block à la suite du parcourt
    /// </summary>
    public void TriggerBlockGeneration()
    {
        // TODO
        AddLevelBlock();
    }


    #endregion
}
