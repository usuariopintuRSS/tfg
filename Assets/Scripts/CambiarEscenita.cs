using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine.SceneManagement;

public class CambiarEscenita : MonoBehaviour
{
    public void CargarEscenita(string escenita)
    {
        if (
            escenita == "JueguesitoDia1"
            || escenita == "JueguesitoContrarreloj"
            || escenita == "JueguesitoInfinito"
        )
            StartCoroutine(CambiarEscenaDespuesDeEspera(escenita));
        else
            SceneManager.LoadScene(escenita);
    }

    private IEnumerator CambiarEscenaDespuesDeEspera(string escenita)
    {
        yield return new WaitForSeconds(2f); // Espera 2 segundos

        SceneManager.LoadScene(escenita); // Cambia a la escena indicada
    }
}
