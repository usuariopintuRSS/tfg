using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine.SceneManagement;

public class PopUpVolverAlTitulo : MonoBehaviour
{
    public void CargarEscenita(string escenita)
    {
        //DataPersistenceManager.instance.NewGame();
        SceneManager.LoadSceneAsync(escenita);
    }
}
