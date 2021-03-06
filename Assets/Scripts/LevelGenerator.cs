﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using HutongGames.PlayMaker;

public class LevelGenerator : MonoBehaviour
{
    // Nombre de blovks à générer d'avance
    public int ActiveMeshesCount;

    // Distance de détection pour le spawn des blocks
    public float SpawnCheckDistance;

    public GameObject[] SafeBlocks;
    public GameObject Coin;
    public float StartSpeed;
    public float SpeedIncreaseFactor;
    public float MaxSpeed;

    // Blocks
    public GameObject[] Blocks;

    public GameObject[] DecoBlocks;

    // Point de spawn actuel
    public Vector3 SpawnPoint;

    private Queue<GameObject> _activeBlocks;

    private Hashtable _meshData;

    #region Comportements Unity
    void Start()
    {
        FsmVariables.GlobalVariables.GetFsmFloat("SPEED_Forward").Value = StartSpeed;
        FsmVariables.GlobalVariables.GetFsmFloat("SPEED_Forward").Value = StartSpeed;

        FsmVariables.GlobalVariables.GetFsmFloat("SPEED_Forward").Value = StartSpeed;

        _meshData = new Hashtable();
        _meshData.Add("D_Sol_01(Clone)", "ooo");
        _meshData.Add("D_block_01(Clone)", "xxx");
        _meshData.Add("D_block_02(Clone)", "xxx");
        _meshData.Add("D_block_03(Clone)", "xxx");
        _meshData.Add("D_block_04(Clone)", "xxx");
        _meshData.Add("D_block_05(Clone)", "xoo");
        _meshData.Add("D_block_06(Clone)", "xxx");
        _meshData.Add("D_block_07(Clone)", "oxo");
        _meshData.Add("D_block_08(Clone)", "xxx");
        _meshData.Add("D_block_09(Clone)", "xxx");
        _meshData.Add("D_block_10(Clone)", "0xx");
        _meshData.Add("D_block_11(Clone)", "xxx");
        _meshData.Add("D_block_12(Clone)", "xxx");
        _meshData.Add("D_block_13(Clone)", "ooo");
        _meshData.Add("D_block_14(Clone)", "oox");
        _meshData.Add("D_block_15(Clone)", "xoo");
        _meshData.Add("D_block_16(Clone)", "xxx");
        _meshData.Add("D_block_17(Clone)", "xoo");
        _meshData.Add("D_block_18(Clone)", "oxo");
        _meshData.Add("D_block_19(Clone)", "oox");
        _meshData.Add("D_block_20(Clone)", "xoo");

        _meshData.Add("D_block_Vide_01(Clone)", "ooo");
        _meshData.Add("D_block_Vide_02(Clone)", "ooo");
        _meshData.Add("D_block_Vide_03(Clone)", "ooo");
        _meshData.Add("D_block_Vide_04(Clone)", "ooo");
        _meshData.Add("D_block_Vide_05(Clone)", "ooo");

        // Reset du score
        Score.GameScore = 0;

        _activeBlocks = new Queue<GameObject>();

        // Génération des blocs de base
        for (var i = 0; i <= ActiveMeshesCount; i++ )
        {
            AddLevelBlock(SafeBlocks[Random.Range(0, SafeBlocks.Length)]);
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

        // Speed
        var speed = FsmVariables.GlobalVariables.GetFsmFloat("SPEED_Forward").Value;
        speed += SpeedIncreaseFactor * Time.deltaTime;
        if (speed > MaxSpeed) speed = MaxSpeed; // Clamp
        //print(speed);
        FsmVariables.GlobalVariables.GetFsmFloat("SPEED_Forward").Value = speed;

        Time.timeScale = speed - StartSpeed + 1;
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

        // DecoBlock
        var deco = (GameObject)GameObject.Instantiate(DecoBlocks[Random.Range(0, DecoBlocks.Length)], SpawnPoint, Quaternion.identity);
        deco.transform.parent = block.transform;

        SpawnPoint += new Vector3(0F, 0F, 6F);

        // Suppression des blocks inutiles
        if (ActiveMeshesCount < _activeBlocks.Count)
        {
            // On vire la première
            var oldBlock = _activeBlocks.Dequeue();
            Destroy(oldBlock);
        }

        // Check for coin spawn
        if(specGO == null)
        {
            var data = (string) _meshData[block.name];
            try
            {
                data.Contains("o");
            }
            catch(System.Exception e)
            {
                print(block.name);
            }

            if(data.Contains("o"))
            {
                // On peut spawn des pièces ici
                var random = Random.Range(0, 2);
                //print(random);
                //random = 0;
                if(random == 0)
                {
                    // get the spawn lane
                    var a = data.Substring(0, 1) == "o";
                    var b = data.Substring(1, 1) == "o";
                    var c = data.Substring(2, 1) == "o";

                    var count = 0;
                    if(a) count++;
                    if(b) count++;
                    if(c) count++;

                    var lanes = new int[count];
                    var index = 0;
                    if(a)
                    {
                        lanes[index] = 1;
                        index++;
                    }
                    if(b)
                    {
                        lanes[index] = 0;
                        index++;
                    }
                    if(c)
                    {
                        lanes[index] = -1;
                    }

                    // Spawn the coins
                    var pos = block.transform.position;
                    pos.y += 0.5F;
                    pos.z -= 1.5F;
                    pos.x += lanes[Random.Range(0, lanes.Length - 1)];
                    for (var i = 0; i < 4; i++ )
                    {
                        var coin = (GameObject) GameObject.Instantiate(Coin);
                        coin.transform.parent = block.transform;
                        coin.transform.position = pos;
                        pos.z += 1;
                    }
                    //print("spawn !!!!");
                }
            }
        }
    }
    #endregion

    // A appeler par les colliders de pièges et les fosses
    public static void Loose()
    {
        Time.timeScale = 1;
        Score.GameScore = FsmVariables.GlobalVariables.GetFsmInt("Score_Total_Int").Value;
        Application.LoadLevel("GameOver");
    }
}
