using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Editor : MonoBehaviour
{
    public static bool Checkneighbor = false;
    public static int backCount;
    public static bool down = false;
    public static bool isboom = false;

    public static float time;
    public static bool[] isclickX = new bool[7];
    public static int[] isclickY = new int[8];
    // Use this for initialization
    public static Vector2Int powerUp1(int type, List<Vector2Int> r, List<Vector2Int> c)
    {
        if (MapLoader.gameStarted)
        {
            /*if (r.Count == 5 && NeiChecker(r))
            {
                Debug.Log("5 match Row");
                Targets.TargetMatch(type, 5);
                Turn.Increment(2);
            }
            else if (c.Count == 5 && NeiChecker(c))
            {
                Debug.Log("5 match Column");
                Targets.TargetMatch(type, 5);
                Turn.Increment(2);
            }*/
            
            /*if (r.Count == 4 && NeiChecker(r))
            {
                Debug.Log("4 match Row");
                Targets.TargetMatch(type, 4);
                Turn.Increment(1);
            }
            else if (c.Count == 4 && NeiChecker(c))
            {
                Debug.Log("4 match Column");
                Targets.TargetMatch(type, 4);
                Turn.Increment(1);
            }*/
        }

        if (r.Count == 4 && NeiChecker(r))
        {
            return r[1];
        }
        else if (c.Count == 4 && NeiChecker(c))
        {
            return c[1];
        }

        return new Vector2Int(-1, -1);
    }

    public static Vector2Int PowerUpType(List<Vector2Int> r, List<Vector2Int> c)
    {
        if (r.Count > 0 && c.Count > 0)
            return new Vector2Int(c[0].x, r[0].y);
        else if (r.Count > 0 && NeiChecker(r))
            return r[2];
        else if (c.Count > 0 && NeiChecker(c))
            return c[2];

        return new Vector2Int(-1, -1);
    }

    public static bool NeiChecker(List<Vector2Int> l)
    {
        foreach (Vector2Int v in l)
        {
            GameObject tmp = JewelSpawn.JewelList[(int)v.x, (int)v.y];
            if (tmp != null && tmp.GetComponent<Jewel>().listX.Count > 0 && tmp.GetComponent<Jewel>().listY.Count > 0)
                return false;
        }
        return true;
    }

    public static int PosChecker(int x, int y)
    {
        int indx = -1;
        List<int> LowerMapPositon = new List<int>();
        List<int> LowerJewelPosition = new List<int>();

        for (int i = 0; i < y; i++)
        {
            if (CellScript.map[x, i] > 0)
            {
                if (CellScript.map[x, i] % 10 != 4)
                {
                    LowerMapPositon.Add(i);
                }
                else
                    LowerMapPositon.Clear();

            }

            if (JewelSpawn.JewelList[x, i] != null && !JewelSpawn.JewelList[x, i].GetComponent<Jewel>().isDestroy)
                LowerJewelPosition.Add(i);
        }

        if (LowerMapPositon.Count > LowerJewelPosition.Count)
            indx = LowerMapPositon[LowerJewelPosition.Count];

        return indx;

    }

    public static void DropAll()
    {
        for (int j = 0; j < CellScript.Instance.Size.y; j++)
            for (int i = 0; i < CellScript.Instance.Size.x; i++)
                if (JewelSpawn.JewelList[i, j] != null && !JewelSpawn.JewelList[i, j].GetComponent<Jewel>().isDestroy)
                    JewelSpawn.JewelList[i, j].GetComponent<Jewel>().JewelDrop();
    }

    /*public static void destroylist(List<GameObject> lst)
    {
        foreach (GameObject o in lst)
            if (o != null)
                o.GetComponent<Jewel>().iswaitdes = true;
    }*/
    /// <summary>
    /// destroy jewels around position
    /// </summary>
    /// <param name="PosMap"></param>
    /*public static void DestroyAround(Vector2Int PosMap)
    {
        int x = (int)PosMap.x;
        int y = (int)PosMap.y;

        for (int i = x - 1; i <= x + 1; i++)
            if (i >= 0 && i <= CellScript.Instance.Size.x-1)
                for (int j = y - 1; j <= y + 1; j++)
                    if (j >= 0 && j <= CellScript.Instance.Size.y-1)
                    {
                        if (JewelSpawn.JewelList[i, j] != null && JewelSpawn.JewelList[i, j].GetComponent<Jewel>().type != 99 && CellScript.map[i, j] < 10)
                            JewelSpawn.JewelList[i, j].GetComponent<Jewel>().Destroying();
                        else if (CellScript.Cells[i, j] != null)
                            CellScript.Cells[i, j].GetComponent<Cell>().RemoveCellEffect();
                    }


    }*/
    /// <summary>
    /// destroy all jewel same type of jewel to be choose
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    /*public static IEnumerator DestroyAllType(GameObject obj)
    {
        int type = obj.GetComponent<Jewel>().type;
        Editor.down = true;
        if (MapLoader.Mode == 1)
            Menu.isRun = false;
        for (int x = 0; x < CellScript.Instance.Size.x; x++)
            for (int y = 0; y < CellScript.Instance.Size.y; y++)
                if (JewelSpawn.JewelList[x, y] != null &&
                        JewelSpawn.JewelList[x, y].GetComponent<Jewel>().type == type &&
                        !JewelSpawn.JewelList[x, y].GetComponent<Jewel>().isMove)
                {
                    GameObject tmp = JewelSpawn.JewelList[x, y];
                    if (CellScript.map[x, y] > 10)
                        CellScript.Cells[x, y].GetComponent<Cell>().RemoveCellEffect();
                    else
                        JewelSpawn.JewelList[x, y].GetComponent<Jewel>().Destroying();

                    Effect.LightingPoint(tmp.transform.position,
                      tmp.GetComponent<Jewel>().effect[5]);
                    yield return new WaitForSeconds(0.3f);

                }
        Editor.down = false;
        Menu.isRun = true;
    }*/
    /// <summary>
    /// remove cell effect around position
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public static void cellprocess(int x, int y)
    {
        if (y + 1 < CellScript.Instance.Size.y && CellScript.map[x, y + 1] > 10)
        {
            CellScript.Cells[x, y + 1].GetComponent<Cell>().RemoveCellEffect();
        }
        if (x + 1 < CellScript.Instance.Size.x && CellScript.map[x + 1, y] > 10)
        {
            CellScript.Cells[x + 1, y].GetComponent<Cell>().RemoveCellEffect();
        }
        if (y - 1 >= 0 && CellScript.map[x, y - 1] > 10)
        {
            CellScript.Cells[x, y - 1].GetComponent<Cell>().RemoveCellEffect();
        }
        if (x - 1 >= 0 && CellScript.map[x - 1, y] > 10)
        {
            CellScript.Cells[x - 1, y].GetComponent<Cell>().RemoveCellEffect();
        }
    }
    /// <summary>
    /// get random jewel and destroy
    /// </summary>
    public static void LightingRandomPoint()
    {
        Vector2Int postmp;
        if (MapLoader.CellNotEmpty > 0)
        {
            postmp = RandomPoint();
            if (postmp.x != -1)
            {
                GameObject tmp = JewelSpawn.JewelList[(int)postmp.x, (int)postmp.y];
                Cell cell = CellScript.Cells[(int)postmp.x, (int)postmp.y].GetComponent<Cell>();
                if (cell.Type > 10)
                    cell.RemoveCellEffect();
                else
                    tmp.GetComponent<Jewel>().Destroying();
                Effect.LightingPoint(tmp.transform.position, tmp.GetComponent<Jewel>().effect[5]);
            }
        }
        else
        {
            postmp = RandomStarWinPosLower();
            if (postmp.x != -1)
            {
                GameObject tmp = JewelSpawn.JewelList[(int)postmp.x, (int)postmp.y];
                if (tmp != null)
                {
                    tmp.GetComponent<Jewel>().Destroying();
                    Effect.LightingPoint(tmp.transform.position, tmp.GetComponent<Jewel>().effect[5]);
                }
            }
        }
    }
    /// <summary>
    /// random jewel under jewel star position
    /// </summary>
    /// <returns></returns>
    public static Vector2Int RandomStarWinPosLower()
    {
        List<Vector2Int> tmp = new List<Vector2Int>();
        Vector2Int posmap = new Vector2Int(-1, -1);
        Vector2Int posstar;
        int r;
        try
        {
            posstar = MapLoader.starwin.GetComponent<Jewel>().PosMap;
        }
        catch { return posmap; }

        for (int i = 0; i < posstar.y; i++)
            if (CellScript.map[(int)posstar.x, i] > 0)
                tmp.Add(new Vector2Int((int)posstar.x, i));
        if (tmp.Count > 0)
        {
            r = Random.Range(0, tmp.Count);
            posmap = tmp[r];
        }
        return posmap;

    }

    public static Vector2Int RandomPoint()
    {
        List<Vector2Int> tmp = new List<Vector2Int>();
        List<Vector2Int> tmpPri = new List<Vector2Int>();
        Vector2Int posmap = new Vector2Int(-1, -1);
        int dem = 0;

        for (int i = 0; i < CellScript.Instance.Size.x; i++)
            for (int j = 0; j < CellScript.Instance.Size.y; j++)
                if (CellScript.map[i, j] > 0 && CellScript.Cells[i, j].GetComponent<Cell>().Type > 0 && CellScript.map[i, j] < 10)
                    tmp.Add(new Vector2Int(i, j));
                else if (CellScript.map[i, j] > 0 && CellScript.Cells[i, j].GetComponent<Cell>().Type > 10)
                    tmpPri.Add(new Vector2Int(i, j));

        while (posmap.x != -1 || dem < 62)
        {

            Vector2Int vt;
            int r;
            if (tmpPri.Count > 0)
            {
                r = Random.Range(0, tmpPri.Count);
                vt = tmpPri[r];
                tmpPri.Remove(tmpPri[r]);
            }
            else
            {
                r = Random.Range(0, tmp.Count);
                vt = tmp[r];
            }
            try
            {
                if (JewelSpawn.JewelList[(int)vt.x, (int)vt.y] != null
                        && !JewelSpawn.JewelList[(int)vt.x, (int)vt.y].GetComponent<Jewel>().isMove)
                {
                    posmap = vt;
                    return posmap;
                }
            }
            catch { return posmap; }
            dem++;
        }

        return posmap;

    }

    public static int CountEmptyCell(int posx, int posy)
    {
        int tmp = 0;
        for (int i = 0; i < posy; i++)
            if (CellScript.map[posx, i] > 0 && CellScript.map[posx, i] % 10 != 4)
                tmp++;
            else if (CellScript.map[posx, i] % 10 == 4)
                tmp = 0;

        return tmp;
    }
    /// <summary>
    /// destroy a row jewel
    /// </summary>
    /// <param name="PosMap"></param>
    public static void RowLighting(Vector2Int PosMap)
    {
        int y = (int)PosMap.y;
        for (int i = 0; i <= CellScript.Instance.Size.x-1; i++)
        {
            if (JewelSpawn.JewelList[i, y] != null && JewelSpawn.JewelList[i, y].GetComponent<Jewel>().type != 99 && CellScript.map[i, y] < 10)
                JewelSpawn.JewelList[i, y].GetComponent<Jewel>().Destroying();
            else if (CellScript.Cells[i, y] != null)
                CellScript.Cells[i, y].GetComponent<Cell>().RemoveCellEffect();

            cellprocess(i, y);
        }
    }
    /// <summary>
    /// destroy a column jewel
    /// </summary>
    /// <param name="PosMap"></param>
    public static void ColumnLighting(Vector2Int PosMap)
    {
        int x = (int)PosMap.x;
        for (int i = 0; i <= CellScript.Instance.Size.y-1; i++)
        {

            if (JewelSpawn.JewelList[x, i] != null && JewelSpawn.JewelList[x, i].GetComponent<Jewel>().type != 99 && CellScript.map[x, i] < 10)
                JewelSpawn.JewelList[x, i].GetComponent<Jewel>().Destroying();
            else if (CellScript.Cells[x, i] != null)
            {
                CellScript.Cells[x, i].GetComponent<Cell>().RemoveCellEffect();
            }

            cellprocess(x, i);
        }
    }

    public static int MinCell(int x)
    {
        int min = -1;
        for (int i = 0; i < CellScript.Instance.Size.y; i++)
            if (CellScript.map[x, i] > 0)
                return i;
        return min;

    }
    /// <summary>
    /// add lighting power to random jewel
    /// </summary>
    /*public static void addLighting()
    {
        int x = -1;
        int y = -1;
        bool done = false;
        int dem = 0;
        while (!done && dem < 63)
        {
            x = Random.Range(0, 7);
            y = Random.Range(0, 9);
            if (CellScript.map[x, y] > 0 && JewelSpawn.JewelList[x, y] != null && JewelSpawn.JewelList[x, y].GetComponent<Jewel>().type < 9 &&
                                JewelSpawn.JewelList[x, y].GetComponent<Jewel>().PowerUp == 0)
            {
                int p = Random.Range(2, 4);
                JewelSpawn.JewelList[x, y].GetComponent<Jewel>().PowerUp = p;
                Effect.SpawnSet(JewelSpawn.JewelList[x, y], JewelSpawn.JewelList[x, y].GetComponent<Jewel>().effect[6], p);
                done = true;
            }
            dem++;
        }


    }*/
}
/// <summary>
/// get list of jewels same jewel type by position
/// </summary>
public class JewelController : MonoBehaviour
{
    public static List<Vector2Int> RowChecker(int type, int y, int x)
    {
        List<Vector2Int> SameType = new List<Vector2Int>();
        List<Vector2Int> l = LeftChecker(type, y, x);
        List<Vector2Int> r = RightChecker(type, y, x);

        if (l.Count + r.Count >= 2)
        {
            while (l.Count > 0)
            {
                SameType.Add(l[0]);
                l.Remove(l[0]);
            }

            SameType.Add(new Vector2Int(x, y));

            foreach (Vector2Int v in r)
                SameType.Add(v);
        }
        return SameType;
    }

    public static List<Vector2Int> LeftChecker(int type, int y, int x)
    {
        List<Vector2Int> dem = new List<Vector2Int>();
        for (int i = x - 1; i >= 0; i--)
        {
            if (i >= 0)
            {
                GameObject tmp = JewelSpawn.JewelList[i, y];
                if (tmp != null && tmp.GetComponent<Jewel>().type == type &&
                        !tmp.GetComponent<Jewel>().isDestroy &&
    !tmp.GetComponent<Jewel>().isMove && CellScript.map[i, y] < 10)
                    dem.Add(new Vector2Int(i, y));
                else
                {
                    dem = dem.OrderBy(v => v.x).ToList();
                    return dem;
                }
            }
        }

        return dem;
    }

    public static List<Vector2Int> RightChecker(int type, int y, int x)
    {
        List<Vector2Int> dem = new List<Vector2Int>();
        for (int i = x + 1; i < CellScript.Instance.Size.x; i++)
        {
            if (i <= CellScript.Instance.Size.x-1)
            {
                GameObject tmp = JewelSpawn.JewelList[i, y];
                if (tmp != null && tmp.GetComponent<Jewel>().type == type &&
                        !tmp.GetComponent<Jewel>().isDestroy &&
    !tmp.GetComponent<Jewel>().isMove && CellScript.map[i, y] < 10)
                    dem.Add(new Vector2Int(i, y));
                else
                    return dem;
            }
        }
        return dem;
    }

    public static List<Vector2Int> ColumnChecker(int type, int x, int y)
    {

        List<Vector2Int> SameType = new List<Vector2Int>();
        List<Vector2Int> u = UpChecker(type, x, y);
        List<Vector2Int> d = DownChecker(type, x, y);

        if (u.Count + d.Count >= 2)
        {
            foreach (Vector2Int v in d)
                SameType.Add(v);

            SameType.Add(new Vector2Int(x, y));

            foreach (Vector2Int v in u)
                SameType.Add(v);
        }
        return SameType;
    }

    public static List<Vector2Int> DownChecker(int type, int x, int y)
    {
        List<Vector2Int> dem = new List<Vector2Int>();
        for (int i = y - 1; i >= 0; i--)
        {
            if (i >= 0)
            {
                GameObject tmp = JewelSpawn.JewelList[x, i];
                if (tmp != null && tmp.GetComponent<Jewel>().type == type && !tmp.GetComponent<Jewel>().isDestroy && !tmp.GetComponent<Jewel>().isMove && CellScript.map[x, i] < 10)
                    dem.Add(new Vector2Int(x, i));
                else
                {
                    dem = dem.OrderBy(v => v.y).ToList();
                    return dem;
                }
            }
        }

        return dem;
    }

    public static List<Vector2Int> UpChecker(int type, int x, int y)
    {
        List<Vector2Int> dem = new List<Vector2Int>();
        for (int i = y + 1; i < CellScript.Instance.Size.y; i++)
        {
            if (i <= CellScript.Instance.Size.y-1)
            {
                GameObject tmp = JewelSpawn.JewelList[x, i];

                if (tmp != null && tmp.GetComponent<Jewel>().type == type && !tmp.GetComponent<Jewel>().isDestroy && !tmp.GetComponent<Jewel>().isMove && CellScript.map[x, i] < 10)
                    dem.Add(new Vector2Int(x, i));
                else
                    return dem;
            }
        }
        return dem;
    }

}
