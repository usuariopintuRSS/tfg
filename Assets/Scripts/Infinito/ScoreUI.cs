using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ScoreUI : MonoBehaviour
{
    public RowUI rowUI;
    public ScoreManager scoreManager;

    void Start()
    {
        scoreManager.AddScore(new Score("eran", 1));
        scoreManager.AddScore(new Score("el", 1));

        var scores = scoreManager.GetHighScores().ToArray();
        for (int i = 0; i < scores.Length; i++)
        {
            var row = Instantiate(rowUI, transform).GetComponent<RowUI>();
            row.rank.text = (i + 1).ToString();
            row.nombre.text = scores[i].name;
            row.score.text = scores[i].score.ToString(); // Corregido: Asignar el puntaje a row.score.text
        }
    }
}