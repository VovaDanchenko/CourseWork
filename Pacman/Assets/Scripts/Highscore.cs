using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


    class Highscore
    {
        public int Score { get; set; }
        public string Name{ get; set; }
        public int ID { get; set; }
        public Highscore(int id,string name,int score)
    {
        this.Score = score;
        this.Name = name;
        this.ID = id;
    }
    }

