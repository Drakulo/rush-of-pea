using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using HutongGames.PlayMaker;

public class LevelGenerator : MonoBehaviour
{
    // Nombre de blovks à générer d'avance
    public int ActiveMeshesCount;

    // Distance de détection pour le spawn des blocks
    public float SpawnCheckDistance;

    public GameObject SafeBlock;
    public GameObject Coin;

    // Blocks
    public GameObject[] Blocks;

    // Point de spawn actuel
    public Vector3 SpawnPoint;

    private Queue<GameObject> _activeBlocks;

    private Hashtable _meshData;

    #region Comportements Unity
    void Start()
    {
        _meshData = new Hashtable();
        _meshData.Add("BlockA(Clone)", "ooo");
        _meshData.Add("BlockA_01(Clone)", "oxx");
        _meshData.Add("BlockA_02(Clone)", "xxx");
        _meshData.Add("BlockA_03(Clone)", "xoo");

        // Reset du score
        Score.GameScore = 0;

        _activeBlocks = new Queue<GameObject>();

        // Génération des blocs de base
        for (var i = 0; i < ActiveMeshesCount; i++ )
        {
            AddLevelBlock(SafeBlock);
        }

        //StartCoroutine(DebugSpawnLoop());
    }

    void Update()
    {
        // Envoi d'un raycast pour savoit s'il faut générer un nouveau block
        var rayhit = new RaycastHit();
     
        //  
        var origin = Camera.main.transform.position;
        origin.z -= SpawnCheckDistance;
        
        //
        var direction = new Vector3(0, -10, 0);

        var ray = new Ray(origin, direction);

        Debug.DrawRay(origin, direction, Color.red);

        Physics.Raycast(ray, out rayhit);
        if(rayhit.collider == null)
        {
            AddLevelBlock();
        }
    }
    #endregion

#if UNITY_EDITOR
    #region DEBUG

    private IEnumerator DebugSpawnLoop()
    {
        while(true)
        {
            AddLevelBlock();
            yield return new WaitForSeconds(0.1F);
        }
    }
    #endregion
#endif

    #region
    /// <summary>
    /// Ajoute un block à la suite du niveua
    /// </summary>
    private void AddLevelBlock(GameObject specGO = null)
    {
        // Block aleatoire
        GameObject block = null;
        if (specGO)
        {
            block = (GameObject)GameObject.Instantiate(specGO, SpawnPoint, Quaternion.identity);
        }
        else
        {
            var index = Random.Range(0, Blocks.Length);
            block = (GameObject)GameObject.Instantiate(Blocks[index], SpawnPoint, Quaternion.identity);
        }
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

        // Check for coin spawn
        if(specGO != null)
        {
            var data = (string) _meshData[block.name];
            if(data.Contains("o"))
            {
                // On peut spawn des pièces ici
                var random = Random.Range(0, 3);
                print(random);
                if(random == 0)
                {
                    // Spawn the coins
                    print("spawn !!!!");
                }
            }
        }
    }
    #endregion

    // A appeler par les colliders de pièges et les fosses
    public static void Loose()
    {
        // TODO
        print("game over");
        Score.GameScore = FsmVariables.GlobalVariables.GetFsmInt("").Value;
        //Application.LoadLevel("GameOver");
    }
}
