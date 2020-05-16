using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Data;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Mono.Data.Sqlite;
public class HighscoreManager : MonoBehaviour   
{
   
    private string connectionString;
    private List<Highscore> highScores = new List<Highscore>();
    public GameObject scorePrefab;
    public Transform scoreParent;
    public int topRanks;
    public int saveScores;
    public InputField enterName;
    public GameObject nameDialog;
    
    void Start()
    {
        connectionString = "Data Source=" + Application.dataPath + "/HighscoreDB.db";
        CreateTable();
        DeleteExtraScores();
        showScores();
        insertScore("test", 0);
        insertScore("test", 0);
        insertScore("test", 0);
        insertScore("test", 0);
        insertScore("test", 0);
        insertScore("test", 0);
        insertScore("test", 0);
        insertScore("test", 0);
        insertScore("test", 0);
        insertScore("test", 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)||Score.scoreValue>highScores[10].Score)
        {
         nameDialog.SetActive(nameDialog);
        }
    }

   private void insertScore(string name, int newScore)
    {
        getScores();
        int hsCount = highScores.Count;
        if (highScores.Count>0)
        {
            Highscore lowestScore = highScores[highScores.Count - 1];
            if (lowestScore != null && saveScores > 0 && highScores.Count > saveScores&&newScore >lowestScore.Score)
            {
                deleteScore(lowestScore.ID);
                hsCount--; 
            }
        }
       
            using (IDbConnection dbConnection = new SqliteConnection(connectionString))
            {
                dbConnection.Open();
                using (IDbCommand dbCmd = dbConnection.CreateCommand())
                {
                    string sqlQuery = String.Format("INSERT INTO Highscore(Name,Score) VALUES(\"{0}\",\"{1}\");", name, newScore);
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
                        highScores.Add(new Highscore(reader.GetInt32(0), reader.GetString(1), reader.GetInt32(2)));
                    }
                    dbConnection.Close();
                    reader.Close();

                }

            }
        }
        highScores.Sort();

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
        getScores();
        foreach (GameObject score in GameObject.FindGameObjectsWithTag("Score"))
        {
            Destroy(score);
        }
        for (int i = 0; i < topRanks; i++)
        {
            if (i <= highScores.Count - 1)
            {
                GameObject tmpObject = Instantiate(scorePrefab);
                Highscore tmpScore = highScores[i];
                tmpObject.GetComponent<HighscoreScript>().setScore(tmpScore.Name, tmpScore.Score.ToString(), "#" + (i + 1).ToString());
                tmpObject.transform.SetParent(scoreParent);
                tmpObject.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            }
            
        }
    }

    private void DeleteExtraScores()
    {
        getScores();
        if (saveScores <= highScores.Count)
        {
            int deleteCount = highScores.Count - saveScores;
            highScores.Reverse();
            using (IDbConnection dbConnection = new SqliteConnection(connectionString))
            {
                dbConnection.Open();
                using (IDbCommand dbCmd = dbConnection.CreateCommand())
                {
                    for (int i = 0; i < deleteCount; i++)
                    {

                        string sqlQuery = String.Format("DELETE FROM Highscore WHERE PlayerID='{0}';", highScores[i].ID);
                        dbCmd.CommandText = sqlQuery;
                        dbCmd.ExecuteScalar();
                       
                    }
                    dbConnection.Close();
                }
            }
        }
    }
    public void EnterName()
    {
        if (enterName.text != string.Empty)
        {
            int score = Score.scoreValue;
            insertScore(enterName.text, score);
            enterName.text = string.Empty;
            Score.scoreValue = 0;
            showScores();
            nameDialog.SetActive(!nameDialog.activeSelf);

        }
    }

    private void CreateTable()
    {

        using (IDbConnection dbConnection = new SqliteConnection(connectionString))
        {
            dbConnection.Open();
            using (IDbCommand dbCmd = dbConnection.CreateCommand())
            {
                string sqlQuery = String.Format("CREATE TABLE IF NOT EXISTS Highscore ( PlayerID  INTEGER PRIMARY KEY AUTOINCREMENT UNIQUE,Name  TEXT,Score INTEGER)");
                dbCmd.CommandText = sqlQuery;
                dbCmd.ExecuteScalar();
                dbConnection.Close();


            }
        }
    }
    


    public void BackButton()
    {
        SceneManager.LoadScene(0);
    }
    public void PlayAgain()
    {
        SceneManager.LoadScene(1);
    }
}
