using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelGenerator : MonoBehaviour
{
    // Nombre de blovks à générer d'avance
    public int ActiveMeshesCount;


    private Queue<GameObject> _activeBlocks;

    #region Comportements Unity
    void Start()
    {
        _activeBlocks = new Queue<GameObject>();
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
            yield return new WaitForSeconds(0.1F);
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
        var block = (GameObject)GameObject.Instantiate(DebugSpawn, SpawnPoint, Quaternion.identity);
        block.transform.parent = transform;
        _activeBlocks.Enqueue(block);

        SpawnPoint += new Vector3(0F, 0F, 6F);

        // Suppression des blocks inutiles
        if (ActiveMeshesCount < _activeBlocks.Count)
        {
            // On vire la première
            var oldBlock = _activeBlocks.Dequeue();
            Destroy(oldBlock);
        }
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
