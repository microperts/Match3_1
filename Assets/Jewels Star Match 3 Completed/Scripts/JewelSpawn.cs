using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

public class JewelSpawn : MonoBehaviour
{
    public static GameObject[,] JewelList;
    public Sprite[] JewelSprites;
    public GameObject JewelPrefab;
    public GameObject parent;
    public static int[] posX;
    public static int[] QuaX;
    public static bool spawnStart = true;
    public static bool isRespawn = false;
    public GameObject clock;
    public GameObject star;
    Supporter sp;

    void Start()
    {
        spawnStart = true;
        JewelList = new GameObject[CellScript.Instance.Size.x, CellScript.Instance.Size.y];
        posX = new int[CellScript.Instance.Size.x];
        QuaX = new int[CellScript.Instance.Size.x];
        sp = new Supporter();
    }

    // Update is called once per frame
    void Update()
    {
        if (isRespawn)
        {
            isRespawn = false;
            Respawn();
        }
    }

    void StartGameSpawn(int[,] map)
    {
        int sx = -1;
        int sy = -1;
        if (MapLoader.starwin != null)
        {
            sx = (int)MapLoader.starwin.GetComponent<Jewel>().PosMap.x;
            sy = (int)MapLoader.starwin.GetComponent<Jewel>().PosMap.y;
        }

        for (int x = 0; x < CellScript.Instance.Size.x; x++)
        {
            for (int y = CellScript.Instance.Size.y-1; y >= 0; y--)
            {
                if (map[x, y] > 0)
                {
                    int rd = RandomJewel();
                    if (sx != -1 && sx == x && sy == y)
                        JewelPrefab.transform.Find("Render").GetComponent<SpriteRenderer>().sprite = null;
                    else
                        JewelPrefab.transform.Find("Render").GetComponent<SpriteRenderer>().sprite = JewelSprites[rd];
                    GameObject tmp = Instantiate(JewelPrefab) as GameObject;
                    tmp.transform.parent = parent.transform;
                    tmp.transform.localPosition = new Vector3(x, y + 11, -1);
                    tmp.GetComponent<Jewel>().type = rd;
                    tmp.GetComponent<Jewel>().PosMap = new Vector2Int(x, y);
                    //tmp.name = (x * 9 + y).ToString();
                    tmp.GetComponent<Jewel>().baseY = y;

                    /*if (sx != -1 && sx == x && sy == y)
                    {
                        tmp.GetComponent<Jewel>().type = 99;
                        //tmp.GetComponent<Jewel>().PowerUp = 99;
                        Effect.SpawnStarWin(tmp, star, true);
                        GameObject.Find("StarWin(Clone)").transform.GetChild(0).GetComponent<Animator>().enabled = true;
                        MapLoader.starwin = tmp;
                    }*/
                    
                    JewelList[x, y] = tmp;

                    /*if (map[x, y] > 20 && map[x, y] % 10 == 4)
                        break;*/
                }
            }
        }

        //sp.SetVirtualJewel();
        ///- Regen if no matches (at start)
        GameObject[] objchecker = sp.GetHintSupportGameObjects();
        if (objchecker[0] == null) { Respawn(); }
    }

    int RandomJewel()
    {
        return MapLoader.RandomLevelTokenList.PickRandom();
        
        /*if (MapLoader.Mode == 1)
            return Random.Range(0, 6);
        else
            return Random.Range(0, 7);*/
    }

    public void Spawn(int x, int y)
    {
        int rd = RandomJewel();
        JewelPrefab.transform.Find("Render").GetComponent<SpriteRenderer>().sprite = JewelSprites[rd];
        GameObject tmp = Instantiate(JewelPrefab) as GameObject;
        tmp.GetComponent<Jewel>().isMove = true;
        JewelList[x, y] = tmp;
        tmp.transform.parent = parent.transform;
        tmp.transform.localPosition = new Vector3(x, posX[x] + 11, -1);
        tmp.GetComponent<Jewel>().type = rd;
        int r = Random.Range(0, 100);
        /*if (r == 83 && MapLoader.Mode == 1)
        {
            Effect.SpawnClock(clock, tmp, new Vector3(0, 0, 0));
            tmp.GetComponent<Jewel>().PowerUp = 4;
        }*/

        posX[x]++;
        tmp.GetComponent<Jewel>().PosMap = new Vector2Int(x, y);
        tmp.GetComponent<Jewel>().baseY = y;
    }

    /*IEnumerator waittodrop(GameObject obj)
    {
        yield return new WaitForSeconds(0.6f);
        obj.GetComponent<Jewel>().isDrop = true;
    }*/

    public void SpawnJewel()
    {
        if (spawnStart)
        {
            for (int i = 0; i < CellScript.Instance.Size.x; i++)
            {
                posX[i] = 0;
                int start = 0;
                for (int s = 0; s < CellScript.Instance.Size.y; s++)
                    if (CellScript.map[i, s] % 10 == 4)
                    {
                        start = s;
                    }

                if (start == 0 && CellScript.map[i, 0] % 10 != 4)
                    start = -1;


                for (int j = start + 1; j < CellScript.Instance.Size.y; j++)
                    if (CellScript.map[i, j] > 0)
                        if (JewelSpawn.JewelList[i, j] == null || JewelSpawn.JewelList[i, j].GetComponent<Jewel>().isDestroy)
                        {
                            Spawn(i, j);
                        }
            }
        }

    }
  
    void Respawn()
    {

        for (int i = 0; i < CellScript.Instance.Size.x; i++)
        {
            for (int j = 0; j < CellScript.Instance.Size.y; j++)
                if (JewelList[i, j] != null)
                    Destroy(JewelList[i, j]);
        }

        StartGameSpawn(CellScript.map);

    }
    
    
}

public static class EnumerableExtension
{
    public static T PickRandom<T>(this IEnumerable<T> source)
    {
        return source.PickRandom(1).Single();
    }

    public static IEnumerable<T> PickRandom<T>(this IEnumerable<T> source, int count)
    {
        return source.Shuffle().Take(count);
    }

    public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
    {
        return source.OrderBy(x => Guid.NewGuid());
    }
    
    public static List<T> GetRandomElements<T>(this IEnumerable<T> list, int elementsCount)
    {
        return list.OrderBy(arg => Guid.NewGuid()).Take(elementsCount).ToList();
    }
}
