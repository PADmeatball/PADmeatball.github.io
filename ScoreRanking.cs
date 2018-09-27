using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NCMB;
using UnityEngine.UI;

public class ScoreRanking : MonoBehaviour
{

    //スコアランキング用テキスト
    [SerializeField] private TextMesh scorerakingtext;
    [SerializeField] private TextMesh mytext;
    [SerializeField] private Text rankingbuttontext;

    //スコア表示用変数
    private List<int> scoreList = new List<int>() { 0, 0, 0 };
    private string key = "key";

    //スコアを格納する変数
    private int score;

    private bool rankingflag;
    private bool onlineflag;
    private bool save;
    private HighScore highScore;



    private void OnEnable()
    {

        //スコアをRapireから持ってくる。
        score = Rapire.Getscore();

        NCMBObject scoreClass = new NCMBObject("Score");
        scoreClass["score"] = score;
        scoreClass.Save();
    }
    void Start()
    {
        
        mytext.text = string.Format("今回のスコア : {0}", score);

        for (int i = 0; i <= 2; i++)
        {
            //key + i番目のkeyでスコアをとってくる。値がない場合は0を返す。
            //０を返すことでスコアがなくてもランキングに０と表示される。
            scoreList[i] = PlayerPrefs.GetInt(key + string.Format("{0}", i), 0);
        }

        //４つのスコア（ランキングの１～３位と今回のスコア）を用意し、ソートをかけて
        //スコアの一番低いスコアをリストから削除する。
        scoreList.Add(score);
        scoreList.Sort((a, b) => b - a);
        scoreList.RemoveAt(3);

        //スコアを保存 key + 0～2で保存
        for (int i = 0; i <= 2; i++)
        {
            PlayerPrefs.SetInt(key + string.Format("{0}", i), scoreList[i]);
        }

        highScore = new HighScore();

        

        onlineflag = GameObject.Find("OnlineRankingButton").GetComponent<UIController>().onlineranking;
        rankingflag = GameObject.Find("OnlineRankingButton").GetComponent<UIController>().rankingflag;

        scorerakingtext.text = string.Format("1位 : {0}\n2位 : {1}\n3位 : {2}\n", scoreList[0], scoreList[1], scoreList[2]);

        highScore.fetchTopRankers();
    }

    private void Update()
    {
        if (highScore.endflag)
        {
            //１度だけ処理したいのでflagで管理
            if (GameObject.Find("OnlineRankingButton").GetComponent<UIController>().rankingflag)
            {

                //onlineボタンが押されたら、オンラインとローカルランキングを切り替える。
                if (GameObject.Find("OnlineRankingButton").GetComponent<UIController>().onlineranking)
                {



                    rankingbuttontext.text = "ローカルランキング\nLocalRanking";
                    //オンラインにあるスコアを表示する。
                    scorerakingtext.text = string.Format("1位 : {0}\n2位 : {1}\n3位 : {2}\n",
                                                         highScore.topRanker[0].print(), highScore.topRanker[1].print(), highScore.topRanker[2].print());
                   
                    GameObject.Find("OnlineRankingButton").GetComponent<UIController>().rankingflag = false;

                }
                else
                {
                    rankingbuttontext.text = "オンラインランキング\nOnlineRanking";
                    Debug.Log(rankingflag);
                    //ローカルランキングのテキストを表示

                    scorerakingtext.text = string.Format("1位 : {0}\n2位 : {1}\n3位 : {2}\n", scoreList[0], scoreList[1], scoreList[2]);

                    GameObject.Find("OnlineRankingButton").GetComponent<UIController>().rankingflag = false;

                }
            }
        }
      
    }
    

}
   

   
