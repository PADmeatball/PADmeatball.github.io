using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Rapire : MonoBehaviour {
   
    // 剣の場所参照用
    [SerializeField] GameObject player;
    //刺さったフルーツの場所を格納するリスト
    [SerializeField] private List<GameObject> fruitpos;
    //刺さったフルーツを格納するリスト
    public List<GameObject> fruitlist = new List<GameObject>();

    //爆発エフェクトを格納する
    [SerializeField] private GameObject exprosiveobj;

    //SE用の変数
    private AudioSource se_poke;
    private AudioSource se_exprosive;
    private AudioSource se_gold;

    //score表示用のGUI
    public TextMesh scoretext;
    [SerializeField] private TextMesh combotext;
    [SerializeField] private TextMesh bounstext;
    //刺さっているフルーツの数をカウントするテキスト
    [SerializeField] private TextMesh fruitcounttext;

    //score計算用
    public static int score;

    //Time表示用のテキスト表示
    //制限時間を表示するためのテキストを表示
    [SerializeField] private TextMesh timetext;

    //Time記録用の変数
    private static float timer;
    [SerializeField] private bool timerflag;


    // Use this for initialization
    void Start() {

        //AudioSourceを変数にいれる
        AudioSource[] AudioSoueces = GetComponents<AudioSource>();
        se_exprosive = AudioSoueces[0];
        se_poke = AudioSoueces[1];
        se_gold = AudioSoueces[2];

        //text初期化
        scoretext.text = "score : 0";

        //scoreを初期化
        score = 0;

        //Textのcolorのa値を０にして不透明に。
        //表示から透明にする処理をのちに追加するため。
        combotext.color = new Color(0,0,0, 0);
        bounstext.color = new Color(1, 0.92f, 0.016f, 0);

        //フルーツのカウントを更新
        fruitcounttext.text = fruitlist.Count.ToString();

        timer = 45;       
    }


    // Update is called once per frame
    void Update() {


        //フルーツのカウントを更新
        fruitcounttext.text = fruitlist.Count.ToString();

        //コントローラの右左を取る

        //トリガーが押されたらフルーツの削除処理に入る。
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger) || Input.GetButtonDown("TEST")) 
            {
            if(fruitlist.Count >= 1)
                    ExprosiveAddScore(fruitlist);

                    for (int listnum = fruitlist.Count - 1; listnum >= 0; listnum--)
                    {
                //爆弾を削除
                Destroy(fruitlist[listnum]);
                //爆発エフェクトを生成
                Destroy(Instantiate(exprosiveobj, fruitpos[1].gameObject.transform.position, Quaternion.identity), 2);
                //爆発音を再生
                se_exprosive.PlayOneShot(se_exprosive.clip);
            }
            fruitlist.Clear();

            //フルーツのカウントを更新
            fruitcounttext.text = fruitlist.Count.ToString();

        }
            
        //時間計算用関数を呼び出す。
        TimerCounter();

    }

    //フルーツと剣が衝突したときの判定
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Fruit")
        {

            //フルーツのカウントを更新
            fruitcounttext.text = fruitlist.Count.ToString();

            if (fruitlist.Count <= 2)
            {
                if (collision.gameObject.name.IndexOf("GoldApple") != -1)
                {
                    //GoldAppleが刺さった時の音
                    se_gold.PlayOneShot(se_gold.clip);
                }
                else
                {
                    //刺さった時用の音を再生
                    se_poke.PlayOneShot(se_poke.clip);
                }

                // フルーツリストの末尾に当たったフルーツを追加する
                fruitlist.Add(collision.gameObject);

                //フルーツを刺す処理
                if (fruitpos.Count >= fruitlist.Count)
                {

                    //Kinematicのチェックをオンにする
                    collision.rigidbody.isKinematic = true;

                    //collision.rigidbodyにかかっている力を０にする。
                    collision.rigidbody.velocity = Vector3.zero;

                    //回転は後々つけるかもしれないので一応・・・
                     collision.rigidbody.angularVelocity = Vector3.zero;

                    //FruitPosを親にする。
                    fruitlist[fruitlist.Count - 1].transform.parent = fruitpos[fruitlist.Count - 1].transform;

                    //FuruitList[FruitList.Count - 1]をローカルポジションのx,y,zをすべて０にする。
                    fruitlist[fruitlist.Count - 1].transform.localPosition = Vector3.zero;

                    //刺した時にフルーツを縮める
                    collision.gameObject.transform.localScale = new Vector3(5, 5, 5);
                }

                //テキストを表示
                AddScore(collision.gameObject);
            }
        }
        //爆弾が剣に当たった時
        //スコアを加算せずに刺さってるフルーツを消す。
        else if(collision.gameObject.tag == "Bomb")
        {
            ExprosiveFunction(collision.gameObject);          

            for (int listnum = fruitlist.Count - 1; listnum >= 0; listnum--)
            {
                ExprosiveFunction(fruitlist[listnum].gameObject);

            }

            fruitlist.Clear();
            //フルーツのカウントを更新
            fruitcounttext.text = fruitlist.Count.ToString();
        }
    }

    //フルーツが刺さった時にスコアを加算する関数
    //引数１　GameObject gameObject : 刺さったフルーツ
    private void AddScore(GameObject gameObject)
    {
        //刺さったフルーツの種類でスコアを変化
        //スコアを加算する

        if (gameObject.name.IndexOf("Gold") != -1)
        {
            score += 200;
        }
        else if (gameObject.name.IndexOf("Banana") != -1)
        {
            score += 20;
        }
        else if (gameObject.name.IndexOf("Kiwi") != -1)
        {
            score += 30;
        }
        else if (gameObject.name.IndexOf("Strawberry") != -1)
        {
            score += 50;
        }
        else if(gameObject.name.IndexOf("Apple") != -1)
        {
            score += 10;
        }

       ScoreAdder(score,scoretext);
    }


    //爆発時にスコアを加算させる関数
    //引数１　List<GameObject> FruitList : 刺さっているフルーツのリスト
    private void ExprosiveAddScore(List<GameObject> fruitlist)
    {
        //爆発による点数の増減処理
        int addscore = 0;
        //CalcScore にてaddscoreをscoreに加え、見やすく。
        addscore += CalcScore(fruitlist);
        TextColor(combotext, addscore, Fadeout(combotext), new Vector3(fruitpos[2].gameObject.transform.position.x, fruitpos[2].gameObject.transform.position.y, fruitpos[2].gameObject.transform.position.z + 1.5f));
        
        //刺さってるフルーツが３つの時に３つ同じかどうか確かめる処理に入る。
        if(fruitlist.Count - 1 == 2)
        {
            //FruitListの０番目、最初に刺さっているフルーツの名前をFruitnameに入れる
            string fruitname = fruitlist[0].gameObject.name;
            //０番目と１番目のオブジェクトネームがおなじなら
            if (fruitname == fruitlist[1].gameObject.name)
                if (fruitname == fruitlist[2].gameObject.name)
                {
                    int conbobonus = 100;
                    score += conbobonus;                    
                    TextColor(bounstext, conbobonus, Fadeout(bounstext), new Vector3(fruitpos[2].gameObject.transform.position.x, fruitpos[2].gameObject.transform.position.y + 1.5f, fruitpos[2].gameObject.transform.position.z + 1.5f));
                    
                }

        }
        score += addscore;

       ScoreAdder(score, scoretext);

    }

    //爆発処理の実行する関数
    private void ExprosiveFunction(GameObject gameObject)
    { 
        //爆弾を削除
        Destroy(gameObject);
        //爆発エフェクトを生成
        Destroy(Instantiate(exprosiveobj, fruitpos[1].gameObject.transform.position, Quaternion.identity), 2);
        //爆発音を再生
        se_exprosive.PlayOneShot(se_exprosive.clip);

    }

    //Comboテキストを表示、Colorを元に戻す。
    //引数１　TextMesh     otherText    　　  : 表示したいテキスト。
    //引数２　int          Addscorepoint      : 加算したいスコアポイント
    //引数３　IEnumerator　enumerable         : 透明化コルーチンを呼ぶ
    //引数４　Vector3      vector3            : テキストの位置を変える用
    private void TextColor(TextMesh otherText,int Addscorepoint,IEnumerator enumerable,Vector3 vector3)
    {
        otherText.text = string.Format( "+{0}" ,Addscorepoint);
        StartCoroutine(enumerable);
        otherText.gameObject.transform.position = new Vector3(vector3.x, vector3.y, vector3.z);                                                          
        
    }

    //テキストを透明化
    //引数１　TextMesh     otherText    　　  : 透明化したいテキスト。
    IEnumerator Fadeout(TextMesh Text)
    {
        for (float i = 0; i <= 60; i++)
        {
            Text.color = new Color(Text.color.r, Text.color.g, Text.color.b, 1.0f - i / 60.0f);
            yield return null;
        }
    }
    //Timer計算用の関数
    private void TimerCounter()
    {
        if (timerflag)
        {
            //Timeをカウントする
            timer -= Time.deltaTime;

            //timerが0になったらendingへ移行
            if (timer < 0)
            {
                timer = 0;
                SceneManager.LoadScene("Ending");
            }

            //TimetextにTimerの値を文字にして表示。(小数点第２位)
            timetext.text = timer.ToString("F2");
        }
    }

    //スコア加算用の関数
    //引数１　int        Addscorepoint       : 加算したいスコアポイント。
    //引数２　TextMesh   text                : 表示したいテキスト。
    private void ScoreAdder(int Addscorepoint, TextMesh text)
    {
        text.text  = string.Format("score :{0}" , Addscorepoint);
    }

    //加算スコア計算用の関数
    private int CalcScore(List<GameObject> fruitlist)
    {
        int addscore = 0;
        switch (fruitlist.Count - 1)
        {

            case 0:
                addscore = 10;
                break;

            case 1:
                addscore = 30;
                break;

            case 2:
                addscore = 50;
                break;


        }
        return addscore;
    }

    //スコア受け渡し用関数
    public static int Getscore()
    {
         return score;
    }

    //Time受け渡し用
    public static float GetTime()
    {
        return timer;
    }

}
