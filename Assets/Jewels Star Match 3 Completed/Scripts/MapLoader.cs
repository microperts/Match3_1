using UnityEngine;
using System.Collections;

public class MapLoader : MonoBehaviour
{


    public static byte Mode = 1;

    public static Player MapPlayer;
    public SpriteRenderer BackGround;
    public Sprite[] BackGroundSprite;
    public TextMesh[] TextMenu;
    public static float TIMEPLAYER = 200f;
    public static float time = 200f;
    public static int CellNotEmpty;
    public static bool Starwin = false;
    public static GameObject starwin = null;
    public GameObject[] lv;
    public Sprite[] numbersprite;

    public static int dem = 0;

    void Awake()
    {
        time = TIMEPLAYER;
    }
    // Use this for initialization
    IEnumerator Start()
    {
        Time.timeScale = 1;
        Touch.supportTime = 7.5f;
        if (Mode == 1)
        {
            starwin = null;
            Starwin = false;
        }
        Editor.time = time;

        Menu.IsWin = false;
        Touch.isPause = false;
        Menu.IsLose = false;
        Effect.SetCount = 0;
        Effect.bonusLighting = 0;
        //setbackground();

        //setLvlabel();

        ///- Level Wait
        yield return new WaitForSeconds(0.2f);
        CellScript.movedone = false;
        yield return new WaitForSeconds(0.2f);
        CellScript.movedone = true;
        JewelSpawn.isRespawn = true;
        GetComponent<Process>().enabled = true;
        yield return new WaitForSeconds(3.0f);
        Loading.Instance.Hide();
    }

    void setLvlabel()
    {
        int chuc;
        int dv;
        string lvltext = "";
        if (MapPlayer.Level <= 99)
        {
            chuc = MapPlayer.Level / 10;
            dv = MapPlayer.Level % 10;
            lvltext = MapPlayer.Level.ToString();
        }
        else if (MapPlayer.Level <= 198)
        {
            chuc = (MapPlayer.Level - 99) / 10;
            dv = (MapPlayer.Level - 99) % 10;
            lvltext = (MapPlayer.Level - 99).ToString();
        }
        else
        {
            chuc = (MapPlayer.Level - 198) / 10;
            dv = (MapPlayer.Level - 198) % 10;
            lvltext = (MapPlayer.Level - 198).ToString();
        }
        TextMenu[1].text = MapPlayer.HightScore.ToString();
        TextMenu[2].text = lvltext;

        lv[1].transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = numbersprite[chuc];
        lv[1].transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = numbersprite[dv];

        Destroy(Instantiate(lv[0]), 2f);
        Destroy(Instantiate(lv[1]), 2f);

    }

    void setbackground()
    {
        int inx = 1;
        if (MapLoader.Mode == 1)
            inx = int.Parse(MapPlayer.Name.Substring(0, 1));
        else
            inx = Random.Range(1, 4);

        BackGround.sprite = BackGroundSprite[inx - 1];
    }

    public void Scoreupdate()
    {
        //TextMenu[0].text = score.ToString();
    }
}
