using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Relojito : MonoBehaviour
{
    public TMP_Text tiempoTexto; // Referencia al componente TextMeshPro para mostrar el tiempo
    public float duracionParpadeo = 2f; // Duración total del parpadeo
    public Canvas canvasFinal; // Referencia al Canvas de la escena final

    private float tiempoRestante = 300f; // Tiempo inicial en segundos (5 minutos)
    private float tiempoInicio; // Tiempo de inicio del parpadeo
    private bool parpadeoIniciado = false; // Bandera para controlar el inicio del parpadeo

    private void Start()
    {
        tiempoInicio = Time.time; // Guardar el tiempo de inicio
    }

    private void Update()
    {
        if (tiempoRestante > 0)
        {
            tiempoRestante -= Time.deltaTime;

            // Calcular minutos y segundos
            int minutos = Mathf.FloorToInt(tiempoRestante / 60);
            int segundos = Mathf.FloorToInt(tiempoRestante % 60);

            // Actualiza visualmente el temporizador en el componente TextMeshPro
            tiempoTexto.text = string.Format("{0:00}:{1:00}", minutos, segundos);

            // Verifica si el tiempo llegó a cero y no se ha iniciado el parpadeo
            if (tiempoRestante <= 0 && !parpadeoIniciado)
            {
                parpadeoIniciado = true;

                tiempoTexto.text = "Tiempo agotado!";

                // Inicia el parpadeo del texto
                tiempoInicio = Time.time;
                InvokeRepeating("ParpadearTexto", 0f, duracionParpadeo / 2f);

                // Inicia la activación del Canvas de la escena final después de 5 segundos
                Invoke("MostrarCanvasFinal", 5f);
            }
        }
    }

    private void ParpadearTexto()
    {
        // Alterna la visibilidad del texto
        tiempoTexto.gameObject.SetActive(!tiempoTexto.gameObject.activeSelf);
    }

    private void MostrarCanvasFinal()
    {
        // Activa el Canvas de la escena final
        canvasFinal.gameObject.SetActive(true);
    }
}
