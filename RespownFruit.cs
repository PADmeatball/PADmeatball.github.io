using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespownFruit : MonoBehaviour
{
    
    //出現させるフルーツを入れておく
    public List<GameObject> Fruit = new List<GameObject>();
    //次にフルーツが出現する時間
    [SerializeField] private float FruitRespownTime;

    // 射出する目標を設定
    public GameObject Target;

    //射出角度を決める
    [SerializeField] private float MaxAngle;
    [SerializeField] private float minAngle;

    //待ち時間計測
    private float WaitTime;

    //SE用の変数
    private AudioSource se_Taihou;
    private AudioSource se_Gold;

    private bool Goldflag;
    public static float angle;
    private int listend;



    // Use this for initialization
    void Start()
    {

        //初期化
        WaitTime = 0.0f;
        AudioSource[] audioSource = GetComponents<AudioSource>();

        //ファイルの名前で実装しとく。音源のなかが見れるから、バグわかりやすい。
        se_Taihou = audioSource[0];
        se_Gold   = audioSource[1];

        Goldflag = false;
        listend = Fruit.Count - 1;
    }

    // Update is called once per frame
    void Update()
    {

        //経過時間を足す
        //秒数足すだけの処理がいっぱい走るのでコルーチン
        WaitTime += Time.deltaTime;

        //経過時間がたったら
        if (WaitTime > FruitRespownTime)
        {
            WaitTime = 0;
            angle = Random.Range(MaxAngle, minAngle);
            throwFruit();
        }

    }
/*
    IEnumerator hoge()
    {
        do
        {
            yield return new WaitForSeconds(FruitRespownTime);
            throwFruit();
        } while ( true );
    }
    */

    public void throwFruit()
    {
         if (Goldflag)
        {
            Fruit.RemoveAt(listend);
            Goldflag = false;
        }
       
            //出現するフルーツをランダムに選ぶ
            var randomValue = Random.Range(0, Fruit.Count);

            if(randomValue == listend)
            {
                Goldflag = true;
            }

            //新しいゲームオブジェクトを作り、オブジェクトを拡大。
            GameObject InstFruit = GameObject.Instantiate(Fruit[randomValue], transform.position, Fruit[randomValue].gameObject.transform.localRotation);

        InstFruit.transform.parent = this.gameObject.transform;
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
            Vector3 velocity = CalculateVelocity(this.transform.position, TargetPosition, angle);

            //射出
            //Rididbody型でもってきているとInstantiateしたときにRididbody型で帰ってくるので楽ちん。
            Rigidbody rb = InstFruit.GetComponent<Rigidbody>();

            rb.AddForce(velocity * rb.mass, ForceMode.Impulse);
            
            if (Goldflag == false)
            {
                //発射音を再生
                se_Taihou.PlayOneShot(se_Taihou.clip);
            }
            else
            {
                //GoldAppleの発射音
                se_Gold.PlayOneShot(se_Gold.clip);
            }

            WaitTime = 0f;
        
       
        
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




