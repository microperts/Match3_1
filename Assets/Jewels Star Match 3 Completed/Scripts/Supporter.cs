using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Supporter : MonoBehaviour
{
    public Vector2Int[] result = new Vector2Int[2];
    
    private int[,] _virtualJewelTypeMap;
    private Vector2Int _mapSize;

    private void Awake()
    {
        _mapSize = CellScript.Instance.Size;
        _virtualJewelTypeMap = new int[_mapSize.x, _mapSize.y];
    }

    private void UpdateVirtualJewelMap()
    {
        for (int x = 0; x < _mapSize.x; x++)
            for (int y = 0; y < _mapSize.y; y++)
            {
                GameObject tmp = JewelSpawn.JewelList[x, y];
                if (tmp != null && CellScript.map[x, y] < 20)
                {
                    _virtualJewelTypeMap[x, y] = tmp.GetComponent<Jewel>().type;
                }
                else
                {
                    _virtualJewelTypeMap[x, y] = -1;
                }
            }
    }

    public GameObject[] MoveSupportGameObject()
    {
        var jewelPositions = new Vector2Int[2];
        var jewelObjects = new GameObject[2];
        
        UpdateVirtualJewelMap();
        
        for (int i = 0; i < _mapSize.x; i++)
            for (int j = 0; j < _mapSize.y; j++)
            {
                if (_virtualJewelTypeMap[i, j] >= 0)
                {
                    jewelPositions = positionChecker(i, j);
                    if (jewelPositions[0].x != -1)
                    {
                        result = jewelPositions;
                        goto mgoto;
                    }
                }
            }
        result = jewelPositions;
    mgoto:
        if (result[0].x != -1)
        {
            jewelObjects = ObjFinder(result);
            if (jewelObjects != null && jewelObjects[0] != null && jewelObjects[1] != null && MapLoader.gameStarted)
            {
                var j1 = jewelObjects[0].GetComponent<Jewel>();
                var j2 = jewelObjects[1].GetComponent<Jewel>();
                Debug.Log($"Found Virtual Match of {j1.type} at {j1.PosMap} and {j2.type} at {j2.PosMap}");
            }
        }
        else
        {
            Debug.Log("No Virtual Matches Found, Regenerating Board");
        }

        return jewelObjects;

    }
    GameObject[] ObjFinder(Vector2Int[] v)
    {
        GameObject[] tmp = new GameObject[2];
        tmp[0] = JewelSpawn.JewelList[(int)v[0].x, (int)v[0].y];
        tmp[1] = JewelSpawn.JewelList[(int)v[1].x, (int)v[1].y];
        return tmp;

    }

    void setDefaut()
    {
        for (int i = 0; i < _mapSize.x; i++)
            for (int j = 0; j < _mapSize.y; j++)
                _virtualJewelTypeMap[i, j] = -1;
    }

    Vector2Int[] positionChecker(int x, int y)
    {
        List<Vector2Int> sameType = new List<Vector2Int>();
        Vector2Int[] tmp = new Vector2Int[2];
        tmp[0] = new Vector2Int(-1, -1);
        tmp[1] = new Vector2Int(-1, -1);
        sameType = same(x, y);

        if (sameType.Count < 0)
        {
            return tmp;
        }
        else
        {
            for (int i = 0; i < sameType.Count; i++)
            {
                tmp = YChecker(sameType[i], x, y);
                if (tmp[0].x != -1)
                    return tmp;
                else
                {
                    tmp = XChecker(sameType[i], x, y);
                    if (tmp[0].x != -1)
                        return tmp;
                }
            }

            tmp = JumpChecker(x, y);
            if (tmp[0].x != -1)
            {
                return tmp;
            }
        }

        return tmp;
    }

    Vector2Int[] XChecker(Vector2Int v, int x, int y)
    {

        Vector2Int[] tmp = new Vector2Int[2];
        tmp[0] = new Vector2Int(-1, -1);
        tmp[1] = new Vector2Int(-1, -1);

        if (_virtualJewelTypeMap[x, y] == _mapSize.y)
        {
            if (y + 1 < _mapSize.y && _virtualJewelTypeMap[x, y + 1] >= 0)
            {
                tmp[0] = new Vector2Int(x, y);
                tmp[1] = new Vector2Int(x, y + 1);
                return tmp;
            }
            else if (y - 1 >= 0 && _virtualJewelTypeMap[x, y - 1] >= 0)
            {
                tmp[0] = new Vector2Int(x, y);
                tmp[1] = new Vector2Int(x, y - 1);
                return tmp;
            }
        }


        if ((int)v.y > y && y + 2 < _mapSize.y && _virtualJewelTypeMap[x, y + 2] >= 0)
        {
            if (x - 1 >= 0 && _virtualJewelTypeMap[x - 1, y + 2] == _virtualJewelTypeMap[x, y])
            {
                tmp[0] = new Vector2Int(x, y + 2);
                tmp[1] = new Vector2Int(x - 1, y + 2);
                return tmp;
            }
            else if (x + 1 <= _mapSize.x-1 && _virtualJewelTypeMap[x + 1, y + 2] == _virtualJewelTypeMap[x, y])
            {
                tmp[0] = new Vector2Int(x, y + 2);
                tmp[1] = new Vector2Int(x + 1, y + 2);
                return tmp;
            }
            else if (y + 3 <= _mapSize.y-1 && _virtualJewelTypeMap[x, y + 3] == _virtualJewelTypeMap[x, y])
            {
                tmp[0] = new Vector2Int(x, y + 2);
                tmp[1] = new Vector2Int(x, y + 3);
                return tmp;
            }
        }

        else if ((int)v.y < y && y - 2 >= 0 && _virtualJewelTypeMap[x, y - 2] >= 0)
        {
            if (x - 1 >= 0 && _virtualJewelTypeMap[x - 1, y - 2] == _virtualJewelTypeMap[x, y])
            {
                tmp[0] = new Vector2Int(x, y - 2);
                tmp[1] = new Vector2Int(x - 1, y - 2);
                return tmp;
            }
            else if (x + 1 <= _mapSize.x-1 && _virtualJewelTypeMap[x + 1, y - 2] == _virtualJewelTypeMap[x, y])
            {
                tmp[0] = new Vector2Int(x, y - 2);
                tmp[1] = new Vector2Int(x + 1, y - 2);
                return tmp;
            }
            else if (y - 3 >= 0 && _virtualJewelTypeMap[x, y - 3] == _virtualJewelTypeMap[x, y])
            {
                tmp[0] = new Vector2Int(x, y - 2);
                tmp[1] = new Vector2Int(x, y - 3);
                return tmp;
            }
        }

        return tmp;

    }

    Vector2Int[] YChecker(Vector2 v, int x, int y)
    {
        Vector2Int[] tmp = new Vector2Int[2];
        tmp[0] = new Vector2Int(-1, -1);
        tmp[1] = new Vector2Int(-1, -1);

        if (_virtualJewelTypeMap[x, y] == _mapSize.y)
        {
            if (x + 1 < _mapSize.x && _virtualJewelTypeMap[x + 1, y] >= 0)
            {
                tmp[0] = new Vector2Int(x, y);
                tmp[1] = new Vector2Int(x + 1, y);
                return tmp;
            }
            else if (x - 1 >= 0 && _virtualJewelTypeMap[x - 1, y] >= 0)
            {
                tmp[0] = new Vector2Int(x, y);
                tmp[1] = new Vector2Int(x - 1, y);
                return tmp;
            }
        }

        if ((int)v.x > x && x + 2 < _mapSize.x && _virtualJewelTypeMap[x + 2, y] >= 0)
        {
            if (y - 1 >= 0 && _virtualJewelTypeMap[x + 2, y - 1] == _virtualJewelTypeMap[x, y])
            {
                tmp[0] = new Vector2Int(x + 2, y);
                tmp[1] = new Vector2Int(x + 2, y - 1);
                return tmp;
            }
            else if (y + 1 <= _mapSize.y-1 && _virtualJewelTypeMap[x + 2, y + 1] == _virtualJewelTypeMap[x, y])
            {
                tmp[0] = new Vector2Int(x + 2, y);
                tmp[1] = new Vector2Int(x + 2, y + 1);
                return tmp;
            }
            else if (x + 3 <= _mapSize.x-1 && _virtualJewelTypeMap[x + 3, y] == _virtualJewelTypeMap[x, y])
            {
                tmp[0] = new Vector2Int(x + 2, y);
                tmp[1] = new Vector2Int(x + 3, y);
                return tmp;
            }
        }
        else if ((int)v.x < x && x - 2 >= 0 && _virtualJewelTypeMap[x - 2, y] >= 0)
        {
            if (y - 1 >= 0 && _virtualJewelTypeMap[x - 2, y - 1] == _virtualJewelTypeMap[x, y])
            {
                tmp[0] = new Vector2Int(x - 2, y);
                tmp[1] = new Vector2Int(x - 2, y - 1);
                return tmp;
            }
            else if (y + 1 <= _mapSize.y-1 && _virtualJewelTypeMap[x - 2, y + 1] == _virtualJewelTypeMap[x, y])
            {
                tmp[0] = new Vector2Int(x - 2, y);
                tmp[1] = new Vector2Int(x - 2, y + 1);
                return tmp;
            }
            else if (x - 3 >= 0 && _virtualJewelTypeMap[x - 3, y] == _virtualJewelTypeMap[x, y])
            {
                tmp[0] = new Vector2Int(x - 2, y);
                tmp[1] = new Vector2Int(x - 3, y);
                return tmp;
            }
        }
        return tmp;
    }

    List<Vector2Int> same(int x, int y)
    {
        List<Vector2Int> lsttmp = new List<Vector2Int>();
        Vector2Int[] tmp = new Vector2Int[4];
        tmp[0] = new Vector2Int(x - 1, y);
        tmp[1] = new Vector2Int(x + 1, y);
        tmp[2] = new Vector2Int(x, y - 1);
        tmp[3] = new Vector2Int(x, y + 1);

        if (_virtualJewelTypeMap[x, y] == _mapSize.y)
        {
            for (int i = 0; i < 4; i++)
                if ((int)tmp[i].x >= 0 && (int)tmp[i].y >= 0)
                    if ((int)tmp[i].x < _mapSize.x && (int)tmp[i].y < _mapSize.y)
                        lsttmp.Add(tmp[i]);
            return lsttmp;
        }


        for (int i = 0; i < 4; i++)
            if ((int)tmp[i].x >= 0 && (int)tmp[i].y >= 0)
                if ((int)tmp[i].x < _mapSize.x && (int)tmp[i].y < _mapSize.y)
                    if (_virtualJewelTypeMap[(int)tmp[i].x, (int)tmp[i].y] == _virtualJewelTypeMap[x, y])
                        lsttmp.Add(tmp[i]);
        return lsttmp;

    }

    List<Vector2Int> sameJump(int x, int y)
    {
        List<Vector2Int> lsttmp = new List<Vector2Int>();
        Vector2Int[] tmp = new Vector2Int[4];
        tmp[0] = new Vector2Int(x - 1, y - 1);
        tmp[1] = new Vector2Int(x + 1, y + 1);
        tmp[2] = new Vector2Int(x + 1, y - 1);
        tmp[3] = new Vector2Int(x - 1, y + 1);

        for (int i = 0; i < 4; i++)
            if ((int)tmp[i].x >= 0 && (int)tmp[i].y >= 0)
                if ((int)tmp[i].x < _mapSize.x && (int)tmp[i].y < _mapSize.y)
                    if (_virtualJewelTypeMap[(int)tmp[i].x, (int)tmp[i].y] == _virtualJewelTypeMap[x, y])
                        lsttmp.Add(tmp[i]);

        return lsttmp;

    }

    Vector2Int[] JumpChecker(int x, int y)
    {
        List<Vector2Int> sameType = new List<Vector2Int>();
        Vector2Int[] tmp = new Vector2Int[2];
        tmp[0] = new Vector2Int(-1, -1);
        tmp[1] = new Vector2Int(-1, -1);
        sameType = sameJump(x, y);


        if (sameType.Count < 2)
        {
            return tmp;
        }
        else
        {
            for (int i = 0; i < sameType.Count; i++)
                for (int j = i + 1; j < sameType.Count; j++)
                    if ((sameType[i].x + sameType[j].x) / 2 != x || (sameType[i].y + sameType[j].y) / 2 != y)
                    {
                        int tmpx = (int)(sameType[i].x + sameType[j].x) / 2;
                        int tmpy = (int)(sameType[i].y + sameType[j].y) / 2;
                        if (_virtualJewelTypeMap[tmpx, tmpy] >= 0)
                        {
                            tmp[0] = new Vector2Int(x, y);
                            tmp[1] = new Vector2Int(tmpx, tmpy);
                            return tmp;
                        }
                    }
        }
        return tmp;
    }
}
