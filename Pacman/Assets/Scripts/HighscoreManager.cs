using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Data;
using Mono.Data.Sqlite;
public class HighscoreManager : MonoBehaviour   
{
    // Start is called before the first frame update
    private string connectionString;
    private List<Highscore> highScores = new List<Highscore>();
    public GameObject scorePrefab;
    void Start()
    {
        connectionString = "Data Source=" + Application.dataPath + "/HighscoreTable/HighscoreDB.db";
        //insertScore("Oleg",228);
        getScores();
        //deleteScore(1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

   private void insertScore(string name, int newScore)
    {
        using (IDbConnection dbConnection = new SqliteConnection(connectionString))
        {
            dbConnection.Open();
            using (IDbCommand dbCmd = dbConnection.CreateCommand())
            {
                string sqlQuery =String.Format("INSERT INTO Highscore(Name,Score) VALUES(\"{0}\",\"{1}\");",name,newScore);
                dbCmd.CommandText = sqlQuery;
                dbCmd.ExecuteScalar();
                dbConnection.Close();
                

            }
        }
    }


    private void getScores()
    {
        highScores.Clear();
        using (IDbConnection dbConnection = new SqliteConnection(connectionString))
        {
            dbConnection.Open();
            using (IDbCommand dbCmd = dbConnection.CreateCommand()) {
                string sqlQuery = "SELECT * FROM Highscore";
                dbCmd.CommandText = sqlQuery;
                using (IDataReader reader = dbCmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        highScores.Add(new Highscore(reader.GetInt32(0), reader.GetString(1), reader.GetInt32(3)));
                    }
                    dbConnection.Close();
                    reader.Close();

                }

            }
        }

    }

    private void deleteScore(int playerID)
    {
        using (IDbConnection dbConnection = new SqliteConnection(connectionString))
        {
            dbConnection.Open();
            using (IDbCommand dbCmd = dbConnection.CreateCommand())
            {
                string sqlQuery = String.Format("DELETE FROM Highscore WHERE PlayerID='{0}';",playerID );
                dbCmd.CommandText = sqlQuery;
                dbCmd.ExecuteScalar();
                dbConnection.Close();


            }
        }
    }

    private void showScores()
    {
        for(int i = 0; i < highScores.Count; i++)
        {
            GameObject tmpObject = Instantiate(scorePrefab);
            Highscore tmpScore = highScores[i];
            tmpObject.GetComponent<HighscoreScript>;
        }
    }
}
