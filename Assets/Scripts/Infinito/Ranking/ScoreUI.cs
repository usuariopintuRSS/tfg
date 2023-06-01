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
        var scores = scoreManager.GetHighScores().ToArray();
        for (int i = 0; i < scores.Length; i++)
        {
            var row = Instantiate(rowUI, transform).GetComponent<RowUI>();
            row.rank.text = (i + 1).ToString();
            row.nombre.text = scores[i].name;
            row.score.text = scores[i].score.ToString(); // Corregido: Asignar el puntaje a row.score.text
        }
    }

    private void ActualizarNombreUltimaPuntuacion()
    {
        var scores = scoreManager.GetHighScores().ToArray();

        if (scores.Length > 0)
        {
            var ultimaPuntuacion = scores[0];
            var row = transform.GetChild(0).GetComponent<RowUI>(); // Obtener la primera fila
            row.nombre.text = ultimaPuntuacion.name;
        }
    }

    public void ActualizarTabla()
    {
        var scores = scoreManager.GetHighScores().ToArray();
        for (int i = 0; i < scores.Length; i++)
        {
            var row = Instantiate(rowUI, transform).GetComponent<RowUI>();
            row.rank.text = (i + 1).ToString();
            row.nombre.text = scores[i].name;
            row.score.text = scores[i].score.ToString(); // Corregido: Asignar el puntaje a row.score.text
        }
        ActualizarNombreUltimaPuntuacion();
    }
}
