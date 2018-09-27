using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialThrow : MonoBehaviour {

    private AudioSource se_taihou;
    private AudioSource se_goldfruit;
    [SerializeField] private TextMesh tutorialtext;
    private RespownFruit respownFruit;
    private Rapire fruitlist;
    private Rapire rapire;
    public List<GameObject> Fruit = new List<GameObject>();
    [SerializeField] private GameObject Target;
    public static float angle;
   
    // Use this for initialization

    void Start () {
        
        AudioSource[] audioSource = GetComponents<AudioSource>();

        //ファイルの名前で実装しとく。音源のなかが見れるから、バグわかりやすい。
        se_taihou = audioSource[0];
        se_goldfruit = audioSource[1];
        angle = 40f;
   
        StartCoroutine(TutorialCorutin(tutorialtext));
        
    }		
    public IEnumerator TutorialCorutin(TextMesh text)
    {

        text.text = "チュートリアルを始めます。";                
        
        yield return new WaitForSeconds(3);

        text.text = "目の前に見える大砲からフルーツが\n飛んできます。\n飛んでくるフルーツを刺しましょう！";
        yield return new WaitForSeconds(3);
        for (int i = 0; i <= 3; i++)
        {
            //飛んでくるフルーツは何があるのかをじっくり見せる。
            TutrialthrowFruit(Fruit[i], Fruit[i].transform.rotation, false);
            yield return new WaitForSeconds(2);
        }

        text.text = "このように4種類のフルーツがあります。\n大きさに応じて点数が違います。";
        yield return new WaitForSeconds(3);

        text.text = "フルーツは３つまでしか刺さりません。\n刺さったフルーツは人差し指のボタンで\n爆発させれます。";
        yield return new WaitForSeconds(3);
        text.text = "刺さっているフルーツが多いほど、\n爆発させたときの点数が高いです。";
        yield return new WaitForSeconds(3);
        text.text = "制限時間内にスコアをどれほど\n稼げるかを競います！";

        yield return new WaitForSeconds(4);

        text.text = "次は爆弾が飛んできます。\nうまくよけてください！";
        yield return new WaitForSeconds(3);
        TutrialthrowFruit(Fruit[4], Fruit[4].transform.localRotation, false);
        yield return new WaitForSeconds(3);
        text.text = "爆弾は当たると、刺さっているフルーツが\n全部消えてしまいます。\nスコアは増えません・・・。";

        yield return new WaitForSeconds(4);

        text.text = "次はレアなフルーツが飛んできます！\nしっかり刺してください！";
        TutrialthrowFruit(Fruit[5], Fruit[5].transform.rotation, true);
        yield return new WaitForSeconds(2);
        text.text = "このフルーツは１ゲーム中に２つしか出てきません。\n頑張って刺しましょう！";

        yield return new WaitForSeconds(4);

        text.text = "最後にコンボの説明をします。";       
        yield return new WaitForSeconds(3);
        text.text = "同じフルーツが３つそろった時に爆発させると\nスコアがより多く獲得できます！";
        yield return new WaitForSeconds(3);

        text.text = "これでチュートリアルは終了です。\nでは頑張ってください！";

        
        yield return new WaitForSeconds(5);
        SceneManager.LoadSceneAsync("Main");

    }
    public void TutrialthrowFruit(GameObject Fruit, Quaternion angle, bool seflag)
    {


        //新しいゲームオブジェクトを作り、オブジェクトを拡大。
        GameObject InstFruit = Instantiate(Fruit, transform.position, angle);


        if (InstFruit.gameObject.tag == "Fruit")
        {
            InstFruit.transform.localScale = new Vector3(10, 10, 10);
        }
        else if (InstFruit.gameObject.tag == "Bomb")
        {
            InstFruit.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        }


        //目標オブジェクトの座標
        Vector3 TargetPosition = Target.transform.position;

        //射出速度を算出する
        Vector3 velocity = CalculateVelocity(this.transform.position, TargetPosition, 45.0f);

        //射出
        //Rididbody型でもってきているとInstantiateしたときにRididbody型で帰ってくるので楽ちん。
        Rigidbody rb = InstFruit.GetComponent<Rigidbody>();

        rb.AddForce(velocity * rb.mass, ForceMode.Impulse);

        if (seflag)
        {
            se_goldfruit.PlayOneShot(se_goldfruit.clip);
        }
        else
        {
             //発射音を再生
            se_taihou.PlayOneShot(se_taihou.clip);
        }


    }

    //標的に命中するまでの速度算出用関数
    //引数１   Vector3 pointA  :   射出開始座標
    //引数２   Vector pointB   :   標的の目標
    //引数３   float angle     :   射出時の角度
    public Vector3 CalculateVelocity(Vector3 pointA, Vector3 pointB, float angle)
    {
        //射出角をラジアンに変換
        // float rad = angle * Mathf.PI / 180;
        float rad = Mathf.Deg2Rad * angle;

        //水平方向の距離x
        float x = Vector2.Distance(new Vector2(pointA.x, pointA.z), new Vector2(pointB.x, pointB.z));
        //垂直方向の距離y
        float y = pointA.y - pointB.y;

        // 斜方投射の公式を初速度について解く
        float speed = Mathf.Sqrt(-Physics.gravity.y * Mathf.Pow(x, 2) / (2 * Mathf.Pow(Mathf.Cos(rad), 2) * (x * Mathf.Tan(rad) + y)));

        if (float.IsNaN(speed))
        {
            // 条件を満たす初速を算出できなければVector3.zeroを返す
            return Vector3.zero;
        }
        else
        {
            return (new Vector3(pointB.x - pointA.x, x * Mathf.Tan(rad), pointB.z - pointA.z).normalized * speed);
        }
    }

}
