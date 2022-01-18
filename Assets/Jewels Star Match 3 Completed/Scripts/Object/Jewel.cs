using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Jewel : MonoBehaviour
{
    public Vector2Int PosMap;
    public List<Vector2Int> listX;
    public List<Vector2Int> listY;
    public int indexinlist;
    public int type;
    public float X = -1;
    public float Y = -1;
    public float baseY = -1;
    public bool isDrop;
    public bool isDestroy;
    //public int PowerUp = 0;
    public Vector2Int PowerUpPosition;
    public bool isProcess;
    public bool isMove;
    public int dtype = -1;
    public bool iswaitdes;  //destroy flag
    int x;
    int y;
    float droptime = 0.45f;
    float spamtime = 0.4f;
    float BonusTime = 22f;
    public GameObject[] effect;
    public GameObject Number;
    public Sprite[] NumberSprite;
    public List<int> lowList = new List<int>();
    public List<int> lowpos = new List<int>();
    public List<GameObject> column = new List<GameObject>();
    public bool isSound = false;
    //public Turn turn;

    Transform mtransform;
    BoxCollider2D mBoxCollider2D;

    void Start()
    {
        mtransform = transform;
        mBoxCollider2D = GetComponent<BoxCollider2D>();
    }

    void Update()
    {

        if (baseY != -1)
            MoveToStart();
        if (X != -1 && X != mtransform.localPosition.x)
            MoveToX(X);
        if (Y != -1 && Y != mtransform.localPosition.y)
            MoveToY(Y);

        if (iswaitdes)
        {
            iswaitdes = false;
            Destroying();
        }

    }

    void MoveToX(float x)
    {
        isMove = true;
        if (Mathf.Abs(x - mtransform.localPosition.x) > 0.15)
        {
            mBoxCollider2D.enabled = false;
            if (mtransform.localPosition.x > x)
                mtransform.localPosition -= new Vector3(Time.smoothDeltaTime * 8f, 0, 0);
            else if (mtransform.localPosition.x < x)
                mtransform.localPosition += new Vector3(Time.smoothDeltaTime * 8f, 0, 0);
        }
        else
        {
            mtransform.localPosition = new Vector3(x, mtransform.localPosition.y, mtransform.localPosition.z);
            X = -1;
            isMove = false;
            mBoxCollider2D.enabled = true;
            if (type == 99)
                if (WinChecker())
                {
                    GameObject.Find("top").GetComponent<Menu>().Win();
                }
        }
    }

    void MoveToY(float y)
    {
        isMove = true;
        if (Mathf.Abs(y - mtransform.localPosition.y) > 0.15)
        {
            mBoxCollider2D.enabled = false;
            if (mtransform.localPosition.y > y)
                mtransform.localPosition -= new Vector3(0, Time.smoothDeltaTime * 8f, 0);
            else if (mtransform.localPosition.y < y)
                mtransform.localPosition += new Vector3(0, Time.smoothDeltaTime * 8f, 0);
        }
        else
        {
            mtransform.localPosition = new Vector3(mtransform.localPosition.x, y, mtransform.localPosition.z);
            Y = -1;
            mBoxCollider2D.enabled = true;
            isMove = false;
            if (type == 99)
                if (WinChecker())
                {
                    GameObject.Find("top").GetComponent<Menu>().Win();
                }
        }

    }

    public void MoveToStart()
    {
        isMove = true;
        Y = -1;
        Touch.hintTime = 3f;
        //Touch.supportTimeRp = 1.5f;
        if (Mathf.Abs(baseY - mtransform.localPosition.y) > 0.15 && baseY < mtransform.localPosition.y)
        {
            mBoxCollider2D.enabled = false;
            mtransform.localPosition -= new Vector3(0, Time.smoothDeltaTime * 10f, 0);
        }
        else
        {
            mtransform.localPosition = new Vector3(mtransform.localPosition.x, baseY, mtransform.localPosition.z);
            baseY = -1;
            isDrop = false;
            isMove = false;

            x = Mathf.RoundToInt(mtransform.localPosition.x);
            PowerUpPosition = new Vector2Int(-1, -1);

            if (CellScript.map[PosMap.x, PosMap.y] % 10 != 4)
                JewelProcessing();
            if (type == 99)
                if (WinChecker())
                {
                    GameObject.Find("top").GetComponent<Menu>().Win();
                }
            mtransform.Find("Render").GetComponent<Animator>().SetInteger("state", 103);
            mBoxCollider2D.enabled = true;
        }
    }

    public void JewelProcessing()
    {
        StartCoroutine(JewelProcess());
    }

    IEnumerator JewelProcess()
    {
        ///- Matching
        
        isProcess = true;
        yield return null;
        
        x = PosMap.x;
        y = PosMap.y;

        listX.Clear();
        listY.Clear();
        listX = JewelController.RowChecker(type, y, x);
        listY = JewelController.ColumnChecker(type, x, y);
        
        if (listX.Count + listY.Count == 3)
        {
            if (MapLoader.gameStarted)
            {
                Score.Increment(3);
                Targets.TargetMatch(type, 3);
            }
            CallDestroy(new Vector2Int(-1, -1), 0);
        }
        else if (listX.Count + listY.Count == 4)
        {
            if (MapLoader.gameStarted)
            {
                Score.Increment(4);
                
                Turn.OnIncrementScoreCallBack(1);
                Targets.TargetMatch(type, 4);
            }
            Vector2Int tmp = Editor.powerUp1(type,listX, listY);
            if (x != tmp.x || y != tmp.y)
            {
                CallDestroy(tmp, 0);
            }
            else
            {
                CallDestroy(tmp, 0);
                Destroying();
            }
            
        }
        else if (listX.Count + listY.Count >= 5)
        {
            if (MapLoader.gameStarted)
            {
                Score.Increment(5);

                Turn.OnIncrementScoreCallBack(2);
                Targets.TargetMatch(type, 5);
            }
            Vector2Int tmp = Editor.PowerUpType(listX, listY);
            if (x != tmp.x || y != tmp.y)
            {
                Destroying();
            }
            else
            {
                CallDestroy(tmp, 0);
                Destroying();
            }
            //turn.ShowPopupScore("+2");

        }
        isProcess = false;
    }

    public void JewelDrop()
    {
        indexinlist = PosChecker(PosMap.x, PosMap.y);
        if (indexinlist != -1 && indexinlist != PosMap.y)
        {
            isMove = true;
            JewelSpawn.JewelList[PosMap.x, PosMap.y] = null;
            JewelSpawn.JewelList[PosMap.x, indexinlist] = gameObject;
            PosMap = new Vector2Int(PosMap.x, indexinlist);
            baseY = PosMap.y;
        }
    }

    /// <summary>
    /// processing jewel destroy
    /// </summary>
    /// <returns></returns>
    IEnumerator destroy()
    {
        Touch.hintTime = 3f;
        //Touch.supportTimeRp = 1.5f;
        if (type < 99)
        {
            GetComponent<BoxCollider2D>().enabled = false;
            Process.DropTimer = droptime;
            if (Process.SpawnWaitTime > 0)
                Process.SpawnWaitTime = spamtime;

            JewelSpawn.JewelList[PosMap.x, PosMap.y] = null;
            isDestroy = true;

            /*try
            {
                CellScript.Cells[(int)PosMap.x, (int)PosMap.y].GetComponent<Cell>().playAnimation();
            }
            catch { }*/

            yield return new WaitForSeconds(0.2f);

            if (Targets.IsTargetType(type))
            {
                var endValue = Targets.Instance.GetTargetTransform(type).position;
                gameObject.transform.DOMove(endValue, 0.2f).OnComplete(() =>
                {
                    mtransform.Find("Render").GetComponent<Animator>().SetInteger("state", 6);
                });
            }
            else
            {
                mtransform.Find("Render").GetComponent<Animator>().SetInteger("state", 6);
            }
            Editor.cellprocess(PosMap.x, PosMap.y);
            //PowerProcess(PowerUp);
            //Effect.SpawnNumber(new Vector2Int(mtransform.position.x, mtransform.position.y), Number, NumberSprite, 0.5f);

            if (isSound)
                Sound.sound.jewelclr();

            Destroy(gameObject, 0.5f);
        }
        yield return null;

    }
    /// <summary>
    /// process if jewel power > 0
    /// </summary>
    /// <param name="power"></param>
    /// <returns></returns>
    /*bool PowerProcess(int power)
    {
        switch (power)
        {
            case 0:
                return false;
            case 1:
                //boom
                Effect.SpawnBoom(new Vector2Int(mtransform.position.x, mtransform.position.y), effect[2], 0.5f);
                Editor.DestroyAround(PosMap);
                return false;
            case 2:
                //row lighting
                Effect.RowLighting(mtransform.position.y, effect[3]);
                Editor.RowLighting(PosMap);
                return false;
            case 3:
                //column lighting
                Effect.ColumnLighting(mtransform.position.x, effect[4]);
                Editor.ColumnLighting(PosMap);

                return true;
            case 4://time 
                GameObject.Find("top").GetComponent<Menu>().timeinc(BonusTime);
                return false;
        }
        return false;

    }*/

    public void Destroying()
    {
        StartCoroutine(destroy());
    }
    /// <summary>
    /// destroy all jewel same jewel type around by row and column
    /// </summary>
    /// <param name="UnDestroyPosition"></param>
    /// <param name="pow"></param>
    public void CallDestroy(Vector2Int UnDestroyPosition, int pow)
    {
        List<Vector2Int> DestroyList = new List<Vector2Int>();
        foreach (Vector2Int v in listX)
        {
            DestroyList.Add(v);
        }

        DestroyList.Add(new Vector2Int(x, y));
        foreach (Vector2Int v in listY)
        {
            DestroyList.Add(v);
        }

        foreach (Vector2Int v in DestroyList)
        {
            if (v != UnDestroyPosition)
            {
                GameObject tmp = JewelSpawn.JewelList[v.x, v.y];

                if (v == DestroyList[0] && tmp != null /*&& tmp.GetComponent<Jewel>().PowerUp == 0*/)
                    tmp.GetComponent<Jewel>().isSound = true;

                if (tmp != null && !tmp.GetComponent<Jewel>().isDestroy && tmp.GetComponent<Jewel>().type == type)
                    if (tmp.GetComponent<Jewel>().listX.Count <= 0 || tmp.GetComponent<Jewel>().listY.Count <= 0)
                    {
                        tmp.GetComponent<Jewel>().Destroying();
                    }
            }
            else
            {
                GameObject tmp = JewelSpawn.JewelList[v.x, v.y];
                /*if (tmp != null && !tmp.GetComponent<Jewel>().isDestroy && tmp.GetComponent<Jewel>().type == type)
                {
                    /*tmp.GetComponent<Jewel>().DoPowerUp(1);#1#
                }*/
            }
        }
    }

    /*public void DoPowerUp(int pow)
    {
        if (PowerUp == 0 && pow == 1)
        {
            PowerUp = 1;
            Effect.SpawnEnchan(effect[0], gameObject);
            Editor.LightingRandomPoint();
            try
            {
                CellScript.Cells[(int)PosMap.x, (int)PosMap.y].GetComponent<Cell>().playAnimation();
            }
            catch { }
        }

    }*/

    int PosChecker(int x, int y)
    {
        int indx = -1;
        lowList.Clear();

        int newpos = y;
        if (CellScript.map[x, y] % 10 == 4)
            return indx;
        for (int i = y - 1; i >= 0; i--)
        {
            if (CellScript.map[x, i] % 10 == 4)
            {
                return newpos;
            }
            if (JewelSpawn.JewelList[x, i] == null && CellScript.map[x, i] > 0)
            {
                newpos = i;
            }

        }
        indx = newpos;
        return indx;

    }

    public void PlayMoveAnimation(Vector2Int posmap2)
    {
        StartCoroutine(MoveAnimation(posmap2));
    }
    IEnumerator MoveAnimation(Vector2Int posmap2)
    {
        int state = 100;
        if (PosMap.x < posmap2.x)
            state = 13;
        else if (PosMap.x > posmap2.x)
            state = 12;
        else if (PosMap.y < posmap2.y)
            state = 10;
        else if (PosMap.y > posmap2.y)
            state = 11;



        mtransform.Find("Render").GetComponent<Animator>().SetInteger("state", state);
        yield return new WaitForSeconds(0.2f);
        mtransform.Find("Render").GetComponent<Animator>().SetInteger("state", 100);
    }
    bool WinChecker()
    {
        if (Mathf.RoundToInt(mtransform.localPosition.y) == Editor.MinCell(PosMap.x))
        {
            return true;
        }
        return false;

    }

    

    /*void playSound()
    {
        if (PowerUp == 1)
            Sound.sound.boom();
        else if (PowerUp == 2 || PowerUp == 3)
            Sound.sound.elec();
    }*/

}
