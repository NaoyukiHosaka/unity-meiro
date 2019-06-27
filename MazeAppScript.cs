using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;

public class MazeAppScript : MonoBehaviour
{   //
    //GUI
    public Text powerText;
    public Text timeText;
    public Text messageText;
    public GameObject panel;
    public Slider sizeSlider;
    public Slider levelSlider;

    private System.Random rnd;//乱数
    private bool endFlg = false;//終了フラグ
    public int power = 100;
    public int gameTime = 300;
    private int playTime = 300;
    private int endTime = 300;
    public int mazeSize = 10;//迷路のサイズ
    public float mazeLevel = 1;
    private int hiScore = 0;
    private bool toolFlg = false;//ツールの表示用フラグ
    //迷路の初期化
    void Start()
    {
        rnd = new System.Random(System.Environment.TickCount);
        LoadPref();
        Reset();
    }
   //設定呼び出し、終了チェック、時間更新、終了時間確認
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            toolFlg = !toolFlg;
            panel.GetComponent<CanvasGroup>().alpha = toolFlg ? 1f : 0f;
            if (!toolFlg) { SavePref(); }
        }
        if (endFlg) { return; }
        playTime = endTime - (int)Time.time;
        timeText.text = "TIME:" + playTime;
        powerText.text = "POWER:" + power;
        CheckTime();
    }
    public void SetSizeSlider()
    {
        mazeSize = (int)sizeSlider.value;
    }
    public void SetLevelSlider()
    {
        mazeLevel = (int)levelSlider.value;
    }
    //リセットボタン
    public void DoReset()
    {
        Reset();
    }
    //設定をロードする
    void LoadPref()
    {
        mazeSize = PlayerPrefs.GetInt("mazeSize");
        mazeLevel = PlayerPrefs.GetFloat("mazeLevel");
        hiScore = PlayerPrefs.GetInt("hiScore");
        if (mazeSize < 4) { mazeSize = 4; }
        if (mazeLevel < 1) { mazeLevel = 1; }
        sizeSlider.value = mazeSize;
        levelSlider.value = mazeLevel;
    }
    //設定保存
    void SavePref()
    {
        PlayerPrefs.SetInt("mazeSize", mazeSize);
        PlayerPrefs.SetFloat("mazeLevel", mazeLevel);
    }
    //迷路の初期化、迷路を消去し作成する
    private void Reset()
    {
        SavePref();
        panel.GetComponent<CanvasGroup>().alpha = 0f;
        messageText.text = "";
        power = 100;
        endTime = gameTime + (int)Time.time;
        toolFlg = false;
        endFlg = false;
        //壁取得
        GameObject[] walls = GameObject.FindGameObjectsWithTag("ob_wall");
        //壁破壊
        foreach (GameObject obj in walls)
        {
            GameObject.Destroy(obj);
        }
        //sphereタグつき敵を取得
        GameObject[] sps = GameObject.FindGameObjectsWithTag("sphere");
        //敵を破壊
        foreach (GameObject obj in sps)
        {
            GameObject.Destroy(obj);
        }
        CreateMazeDate();
        CreateSphere();
        GameObject.Find("unitychan").GetComponent<MazeAvatorColliderScript>().collisionFlg = false;
        GameObject.Find("Plane").GetComponent<Renderer>().material.color = Color.white;
    }
    //迷路データ作成
    void CreateMazeDate()
    {
        int mazeW = mazeSize * 4 + 2;
        bool[,] fdate = new bool[mazeW, mazeW];
        for (int i = 0; i < mazeW; i++)
            for (int j = 0; j < mazeW; i++)
            {
                if (i == 0 || i == (mazeW - 1) ||
                        j == 0 || j == (mazeW - 1)) { fdate[i, j] = true; }
                else
                {
                    fdate[i, j] = false;
                }
            }

        int[,] arw = new int[,]
        {
        {0,-1},{0,1},{-1,0},{1,0}
        };
        for (int i = 0; i < (mazeSize / 2) * (mazeSize / 2); i++)
        {
            while (true)
            {
                int x = rnd.Next(1, mazeSize) * 4;
                int y = rnd.Next(1, mazeSize) * 4;
                if (fdate[x, y]) { continue; }
                int n = i % 4;
                fdate[x, y] = true;
                while (true)
                {
                    x += arw[n, 0];
                    y += arw[n, 1];
                    if (fdate[x, y])
                    {
                        break;
                    }
                    else { fdate[x, y] = true; }
                }
                break;
            }
        }
        int cp = mazeW / 2;
        fdate[cp, cp] = false;
        GameObject.Find("unitychan").transform.position = new Vector3(cp, 0, cp);
        CreateMaze(fdate);
        int[,] gdatas = new int[,]
        {
            {1,1 },{1,mazeW-2},{mazeW-2,1},{mazeW-2,mazeW-2}
        };
        int gn = rnd.Next(4);
        Vector3 goalpos = new Vector3(gdatas[gn, 0], 1.5f, gdatas[gn, 1]);
        GameObject.Find("goal").transform.position = goalpos;

    }
    //迷路オブジェクトの作成
    void CreateMaze(bool[,] date)
    {
        int mazeW = mazeSize * 4 + 2;
        Texture txtr = Resources.Load<Texture>("CliffAlbedoSpecular");
        for (int i = 0; i < mazeW; i++)
        {
            for (int j = 0; j < mazeW; j++)
            {
                if (date[i, j])
                {
                    GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    obj.tag = "ob_wall";
                    obj.transform.localScale = new Vector3(1, 2, 1);
                    obj.transform.position = new Vector3(i, 1, j);
                    obj.GetComponent<Collider>().isTrigger = false;
                    obj.GetComponent<Renderer>().material.mainTexture = txtr;
                }
            }
        }
    }
    //敵を生成
    void CreateSphere()
    {
        for (int i = 0; i < (mazeSize / 2); i++)
        {
            GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            obj.tag = "sphere";
            Renderer renderer = obj.GetComponent<Renderer>();
            renderer.material.color = new Color(1, 0, 0, 0.5f);
            renderer.material.SetFloat("_Mode", 3f);
            obj.AddComponent<Renderer>();
            obj.transform.position = new Vector3(rnd.Next(mazeSize) * 4 + 2, 0, rnd.Next(mazeSize) * 4 + 1);
            obj.AddComponent<MazeSphereScript>();
        }
    }
    //終了フラグを返す
    public bool IsEnd()
    {
        return endFlg;
    }
    public void LossPower(int n)
    {
        power -= n;
        if (power <= 0)
        {
            power = 0;
            BadEnd();
        }
    }
    public void CheckTime()
    {
        if (playTime <= 0)
        {
            BadEnd();
        }
    }
    //ゲームオーバー時の処理
    public void BadEnd()
    {
        endFlg = true;
        int Score = (int)(power * mazeLevel + playTime * mazeSize);
        messageText.color = Color.blue;
        messageText.text = "GAMEOVER";
        PlayerPrefs.SetInt("hiScore", 1);//delete
    }
    //ゲームクリア時の処理
    public void GoodEnd()
    {
        endFlg = true;
        int score = (int)(power * mazeLevel * 2 + playTime * mazeSize * 2);
        string msg = "CLEAR!";
        messageText.color = Color.yellow;
        if (score > hiScore)
        {
            hiScore = score;
            msg = "Hi-Score!";
            PlayerPrefs.SetInt("hiScore", hiScore);
            messageText.color = Color.red;
        }
        msg += "[" + score + "]";
        messageText.text = msg;
    }
}