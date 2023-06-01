using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    private string escenita;
    private ScoreData sd;
    private ScoreUI scoreUI;

    void Awake()
    {
        escenita = SceneManager.GetActiveScene().name;
        var json = PlayerPrefs.GetString(NombresitoMarcador(), "{}");
        sd = JsonUtility.FromJson<ScoreData>(json);
        if (sd == null)
            sd = new ScoreData();
    }

    public IEnumerable<Score> GetHighScores()
    {
        return sd.scores.OrderByDescending(x => x.score).Take(10);
    }

    public void AddScore(Score score)
    {
        sd.scores.Add(score);
        SaveScore();
    }
    
    public void SaveScore()
    {
        var json = JsonUtility.ToJson(sd);
        PlayerPrefs.SetString(NombresitoMarcador(), json);
    }

    private string NombresitoMarcador()
    {
        return "marcador_" + escenita;
    }
}