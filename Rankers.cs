using System.Collections;
using UnityEngine;

namespace NCMB
{
    //GIT用test変更
    public class Rankers 
    {
        public int score { get; set; }

        public Rankers (int _score)
        {
            score = _score;
        }
     
        public int print()
        {
            return score;
        }
        
        
    }
   
}

