using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Para que al hacer click en "Opciones", te cambie a la escena que toca. :v

public class CambioDeEscena : MonoBehaviour
{
    public void CambiarAEscenaDeOpciones()
    {
        SceneManager.LoadScene("SettingsMenu");
    }
}