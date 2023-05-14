using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ScoreManager : MonoBehaviour
{
    private ScoreData sd;

    void Awake()
    {
        sd = new ScoreData();
    }

    public IEnumerable<Score> GetHighScores()
    {
        return sd.scores.OrderByDescending(keySelector: x => x.score);
    }

    public void AddScore(Score score)
    {
        sd.scores.Add(score);
    }
}