using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using Random = UnityEngine.Random;

public class MapLoader : MonoBehaviour
{


    public static byte Mode = 1;
    public static bool gameStarted = false;
    public static int selectedLevel = 1;

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
    public static List<int> RandomLevelTokenList;

    [Serializable]
    public class SaveScorePayload
    {
        public string username;
        public int score;
    }
    
    void Awake()
    {
        time = TIMEPLAYER;
        PlayerPrefs.DeleteAll();
    }

    public void RestartGame()
    {
        Loading.Instance.Show();

        string payload = JsonUtility.ToJson(new SaveScorePayload()
        {
            username = Logins.UserName,
            score = Score.Value
        });

        //string payload = "{ \"username\": \"adil\", \"score\": 100 }";
        
        Debug.Log(payload);

        StartCoroutine(ApiHelper.Post("https://boss-fall.herokuapp.com/api/score", (response) =>
        {
            Debug.Log("Score updated (will only update on server if higher than previous score) (server side setting)");
            //MenuController.returningPlay = true;
            this.Invoke(SceneManagerX.LoadPreviousScene,4.0f);
        }, (error) =>
        {
            Debug.Log("Error updating score : " + error);
            //MenuController.returningPlay = true;
            this.Invoke(SceneManagerX.LoadPreviousScene,4.0f);
        }, payload));
    }


    public void OnClick_Leaderboard()
    {
        MenuController.ShowLeaderboard = true;
        RestartGame();
    }
    
    IEnumerator Start()
    {
        Time.timeScale = 1;
        Touch.hintTime = 7.5f;
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

        Menu.isRun = true;
        selectedLevel = Random.Range(1, 5); // 4 groups
        Debug.Log("Selected Level : " + selectedLevel);

        int randomLevel = selectedLevel;

       //    randomLevel = 1;
        
        RandomLevelTokenList = new List<int>();

        if (randomLevel == 1)
        {
            RandomLevelTokenList.Add(0);  // bitcoin
            RandomLevelTokenList.Add(1);  // boss
            RandomLevelTokenList.Add(2);  // cougar
            RandomLevelTokenList.Add(3);  // pink
            RandomLevelTokenList.Add(11); // usdc
            RandomLevelTokenList.Add(10); // solana
            
                /*RandomLevelTokenList.Add(5);  // harmonape
                RandomLevelTokenList.Add(6);  // harmony
                RandomLevelTokenList.Add(9);  // rvrs
                RandomLevelTokenList.Add(8);  // monster*/
        }
        else if (randomLevel == 2)
        {
            RandomLevelTokenList.Add(4);  // etherium
            RandomLevelTokenList.Add(1);  // boss
            RandomLevelTokenList.Add(5);  // harmonape
            RandomLevelTokenList.Add(6);  // harmony
            RandomLevelTokenList.Add(9);  // rvrs
            RandomLevelTokenList.Add(8);  // monster
        }
        else if (randomLevel == 3)
        {
            RandomLevelTokenList.Add(7);  // hydra
            RandomLevelTokenList.Add(1);  // boss
            RandomLevelTokenList.Add(8);  // monster
            RandomLevelTokenList.Add(9);  // rvrs
            RandomLevelTokenList.Add(5);  // harmonape
            RandomLevelTokenList.Add(3);  // pink
        }
        else if (randomLevel == 4)
        {
            RandomLevelTokenList.Add(10); // solana
            RandomLevelTokenList.Add(1);  // boss
            RandomLevelTokenList.Add(11); // usdc
            RandomLevelTokenList.Add(0);  // bitcoin
            RandomLevelTokenList.Add(7);  // hydra
            RandomLevelTokenList.Add(3);  // pink
        }
        
        var randomList = RandomLevelTokenList.GetRandomElements(3);
        if (!randomList.Contains(1)) { randomList[0] = 1; }
        
        Targets.Instance.PopulateTargets(randomList);
        
        ///- Level Wait
        gameStarted = false;
        Time.timeScale = 3.0f;
        yield return new WaitForSecondsRealtime(0.2f);
        CellScript.movedone = false;
        yield return new WaitForSecondsRealtime(0.2f);
        CellScript.movedone = true;
        JewelSpawn.isRespawn = true;
        GetComponent<Process>().enabled = true;
        yield return new WaitForSecondsRealtime(3.0f);
        Time.timeScale = 1.0f;
        Loading.Instance.Hide();
        gameStarted = true;
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
