using System.Collections;
using System.Collections.Generic;
using NCMB;
using UnityEngine;


public class HighScore
{

    public List<NCMB.Rankers> topRanker = null;
    public bool endflag = false;
    
    public void fetchTopRankers()
    {
       
        NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>("Score");
        query.OrderByDescending("score");
        query.Limit = 3;
        query.FindAsync((List<NCMBObject> objList, NCMBException e) =>
        {
            
            if (e != null)
            {
                
            }
            else
            {
                

                //検索成功時
                List<NCMB.Rankers> list = new List<NCMB.Rankers>();
                foreach (NCMBObject obj in objList)
                {
                    int i = System.Convert.ToInt32(obj["score"]);
                    list.Add(new Rankers(i));
                }
                topRanker = list;
                
                endflag = true;
            }
        });

    }
}
