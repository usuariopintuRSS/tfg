using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using System;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // VARIABLES Y DEMAS DEL SISTEMA ANIQUILA ENEMIGOS
    private int a = 0;
    private int b = 0;
    private int correctito = 0;
    private int incorrectito = 0;
    private int contadorPersonas;

    public Button b1;
    public Button b2;

    // Lineas agregadas

    public AudioSource audioSource;

    public TMP_Text tiempoTexto; // Referencia al componente TextMeshPro para mostrar el tiempo
    public float duracionParpadeo = 2f; // Duracion total del parpadeo
    public Canvas canvasFinal; // Referencia al Canvas de la escena final
    public Canvas canvasJuego;
    public Canvas canvasTabla;

    private float tiempoRestante = 0f; // Tiempo inicial en segundos (5 minutos)
    private float tiempoInicio; // Tiempo de inicio del parpadeo
    private bool parpadeoIniciado = false; // Bandera para controlar el inicio del parpadeo

    public TMP_Text puntuacionTexto;

    private int puntuacion = 0;

    // Fin de lineas agregadas

    // VARIABLES Y DEMAS MIERDAS DEL SPAWN
    public GameObject enemyPrefab;
    public GameObject document;
    public GameObject document2;
    public GameObject document3;

    private float timeBetweenEnemySpawn;
    private float timeBetweenWaves;

    public Transform spawnPoint;

    public Transform spawnDocumentPoint;

    bool spawningWave;

    List<string> n;

    private GameObject h;
    private GameObject i;
    private GameObject j;

    public static bool isOpen;

    private int doks;

    public bool fin;

    public ScoreManager scoreManager;
    public TMP_InputField user_inputField;

    int ra;

    public Sprite[] sprites;

    List<string[]> datosgatos;

    string escenita;

    bool finalHistoria;

    void Start()
    {
        contadorPersonas = 0;
        finalHistoria = false;
        escenita = SceneManager.GetActiveScene().name;
        StartCoroutine(SpawnWave());
        b1.onClick.AddListener(TaBien);
        b2.onClick.AddListener(TaMal);
        fin = false;
        Lista();

        tiempoInicio = Time.time; // Guardar el tiempo de inicio

        ActualizarPuntuacion();

        if (escenita == "JueguesitoContrarreloj")
        {
            tiempoRestante = 300f;
        }
        if (escenita == "JueguesitoInfinito")
        {
            tiempoRestante = 0f;
        }
    }

    void Update()
    {
        // SPAWN
        int cuentaDoks = 0;

        if (GameObject.FindGameObjectsWithTag("Personaje").Length == 0 && !spawningWave && !fin)
            StartCoroutine(SpawnWave());
        if (GameObject.FindGameObjectsWithTag("Documento").Length != 0)
        {
            for (int x = 0; x < GameObject.FindGameObjectsWithTag("Documento").Length; x++)
            {
                if (GameObject.FindGameObjectsWithTag("Documento")[0].transform.position.x <= -0.75)
                {
                    cuentaDoks++;
                }
                if (cuentaDoks == 0)
                    isOpen = true;
                else
                    isOpen = false;
            }
            /* if (
                GameObject.FindGameObjectsWithTag("Documento")[0].transform.position.x > -0.75
                && GameObject.FindGameObjectsWithTag("Documento")[1].transform.position.x > -0.75
            )
                isOpen = true;
            else
                isOpen = false; */
        }

        // Lineas agregadas
        //string SceneName = SceneManager.GetActiveScene().name;
        //if(SceneName=="JueguesitoContrarreloj) se hace lo de abajo
        //if(SceneName==) ...

        if (escenita == "JueguesitoContrarreloj")
        {
            if (tiempoRestante > 0)
            {
                tiempoRestante -= Time.deltaTime;

                RelojitoVisual();

                // Verifica si el tiempo llegó a cero y no se ha iniciado el parpadeo
                if (tiempoRestante <= 0 && !parpadeoIniciado)
                {
                    parpadeoIniciado = true;

                    tiempoTexto.text = "¡Tiempo agotado!";

                    // Inicia el parpadeo del texto
                    tiempoInicio = Time.time;
                    InvokeRepeating("ParpadearTexto", 0f, duracionParpadeo / 2f);

                    // Inicia la activación del Canvas de la escena final después de 5 segundos
                    Invoke("MostrarCanvasFinal", 5f);
                }
            }
        }

        if (escenita == "JueguesitoInfinito")
        {
            tiempoRestante += Time.deltaTime;
            RelojitoVisual();
            if (incorrectito > 0)
            {
                Invoke("MostrarCanvasFinal", 5f);
                incorrectito = 0;
            }
        }

        if (
            escenita == "JueguesitoDia1"
            || escenita == "JueguesitoDia2"
            || escenita == "JueguesitoDia3"
            || escenita == "JueguesitoDia4"
            || escenita == "JueguesitoDia5"
        )
        {
            if (finalHistoria == true || incorrectito > 2)
            {
                Invoke("MostrarCanvasFinal", 5f);
                incorrectito = 0;
            }
        }
    }

    void RelojitoVisual()
    {
        // Calcular minutos y segundos
        int minutos = Mathf.FloorToInt(tiempoRestante / 60);
        int segundos = Mathf.FloorToInt(tiempoRestante % 60);

        // Actualiza visualmente el temporizador en el componente TextMeshPro
        tiempoTexto.text = string.Format("{0:00}:{1:00}", minutos, segundos);
    }

    IEnumerator SpawnWave()
    {
        spawningWave = true;
        yield return new WaitForSeconds(timeBetweenWaves);
        if (escenita == "JueguesitoContrarreloj" || escenita == "JueguesitoInfinito")
        {
            GameObject o = Instantiate(
                enemyPrefab,
                spawnPoint.position,
                enemyPrefab.transform.rotation
            );
            ChangeSprite(o);
            StartCoroutine(SpawnDocument());
        }
        if (
            escenita == "JueguesitoDia1"
            || escenita == "JueguesitoDia2"
            || escenita == "JueguesitoDia3"
            || escenita == "JueguesitoDia4"
            || escenita == "JueguesitoDia5"
        )
        {
            if (finalHistoria == false)
            {
                GameObject o = Instantiate(
                    enemyPrefab,
                    spawnPoint.position,
                    enemyPrefab.transform.rotation
                );
                ChangeSprite(o);
                StartCoroutine(Dialogo());
            }
        }
        /* StartCoroutine(SpawnDocument());
        StartCoroutine(Dialogo()); */
        yield return new WaitForSeconds(timeBetweenEnemySpawn);
        spawningWave = false;

        /* yield return new WaitForSeconds(timeBetweenEnemySpawn);
        spawningWave = false;

        spawningWave = true;
        yield return new WaitForSeconds(timeBetweenWaves);
        GameObject o = Instantiate(
            enemyPrefab,
            spawnPoint.position,
            enemyPrefab.transform.rotation
        );
        ChangeSprite(o);
        if (escenita == "JueguesitoContrarreloj" || escenita == "JueguesitoInfinito")
        {
            StartCoroutine(SpawnDocument());
        }
        else
        {
            StartCoroutine(Dialogo());
        }
        /* StartCoroutine(SpawnDocument());
        StartCoroutine(Dialogo());
        yield return new WaitForSeconds(timeBetweenEnemySpawn);
        spawningWave = false; */
    }

    IEnumerator SpawnDocument()
    {
        yield return new WaitForSeconds(timeBetweenWaves);
        int n = UnityEngine.Random.Range(0, 2);
        if (escenita == "JueguesitoContrarreloj" || escenita == "JueguesitoInfinito")
        {
            if (n == 0)
            {
                h = Instantiate(document, spawnDocumentPoint.position, document.transform.rotation);
                i = Instantiate(
                    document2,
                    spawnDocumentPoint.position,
                    document2.transform.rotation
                );
                doks = 0;
            }
            else
            {
                h = Instantiate(document, spawnDocumentPoint.position, document.transform.rotation);
                i = Instantiate(
                    document2,
                    spawnDocumentPoint.position,
                    document2.transform.rotation
                );
                j = Instantiate(
                    document3,
                    spawnDocumentPoint.position,
                    document.transform.rotation
                );
                doks = 1;
            }
        }
        if (escenita == "JueguesitoDia1")
        {
            h = Instantiate(document, spawnDocumentPoint.position, document.transform.rotation);
        }
        if (escenita == "JueguesitoDia2" || escenita == "JueguesitoDia3")
        {
            h = Instantiate(document, spawnDocumentPoint.position, document.transform.rotation);
            i = Instantiate(document2, spawnDocumentPoint.position, document2.transform.rotation);
        }
        if (escenita == "JueguesitoDia4" || escenita == "JueguesitoDia5")
        {
            h = Instantiate(document, spawnDocumentPoint.position, document.transform.rotation);
            i = Instantiate(document2, spawnDocumentPoint.position, document2.transform.rotation);
            j = Instantiate(document3, spawnDocumentPoint.position, document.transform.rotation);
        }
        Nombresitos();
        yield return new WaitForSeconds(timeBetweenEnemySpawn);
    }

    public void TaBien()
    {
        if (
            GameObject.FindGameObjectWithTag("Personaje") != null
            && isOpen
            && GameObject.FindGameObjectsWithTag("Documento").Length != 0
        )
        {
            //if(h.transform.GetComponentInChildren<TMP_Text>().text == i.transform.GetComponentInChildren<TMP_Text>().text) correctito++;
            if (ra > 0 && Comprobasion() == true)
                correctito++;
            else
                incorrectito++;
            GameObject[] docs = GameObject.FindGameObjectsWithTag("Documento");
            foreach (GameObject doc in docs)
            {
                if (doc != null)
                    GameObject.Destroy(doc);
            }
            a++;
            //Debug.Log("le has dado TaBien a: " + a);
            //
            Comprobasion();
            ActualizarPuntuacion();
        }
    }

    public bool Comprobasion()
    {
        int fallos = 0;
        if (escenita == "JueguesitoContrarreloj" || escenita == "JueguesitoInfinito")
        {
            DateTime fechitaDni = DateTime.Parse(
                h.transform.GetComponentsInChildren<TMP_Text>()[4].text
            );
            if (doks == 1)
            {
                DateTime fechitaCovid = DateTime.Parse(
                    j.transform.GetComponentsInChildren<TMP_Text>()[2].text
                );
                TimeSpan dif = DateTime.Now.Subtract(fechitaCovid);
                int diferencia = (int)dif.TotalDays;
                if (diferencia > 1825)
                    fallos++;
            }
            if (DateTime.Now > fechitaDni)
                fallos++;
            if (fallos == 0)
                return true;
            else
                return false;
        }
        else
        {
            if (
                escenita == "JueguesitoDia1"
                || escenita == "JueguesitoDia2"
                || escenita == "JueguesitoDia3"
                || escenita == "JueguesitoDia4"
                || escenita == "JueguesitoDia5"
            )
            {
                if (contadorPersonas == 6)
                {
                    finalHistoria = true;
                }
            }

            return true;
        }
    }

    public void SubirPuntuacion()
    {
        scoreManager.AddScore(new Score(user_inputField.text.ToString(), puntuacion));
    }

    public void TaMal()
    {
        if (
            GameObject.FindGameObjectWithTag("Personaje") != null
            && isOpen
            && GameObject.FindGameObjectsWithTag("Documento").Length != 0
        )
        {
            //if(h.transform.GetComponentInChildren<TMP_Text>().text != i.transform.GetComponentInChildren<TMP_Text>().text) correctito++;
            if (ra == 0 || Comprobasion() == false)
                correctito++;
            else
                incorrectito++;
            GameObject[] docs = GameObject.FindGameObjectsWithTag("Documento");
            foreach (GameObject doc in docs)
                GameObject.Destroy(doc);
            b++;
            //Debug.Log("le has dado TaMal a: " + b);
            Comprobasion();
            ActualizarPuntuacion();
        }
    }

    public void ChangeSprite(GameObject o)
    {
        SpriteRenderer sr = o.transform.GetComponentInChildren<SpriteRenderer>();
        if (escenita == "JueguesitoContrarreloj" || escenita == "JueguesitoInfinito")
        {
            //SpriteRenderer sr = o.transform.GetComponentInChildren<SpriteRenderer>();
            int r = UnityEngine.Random.Range(1, sprites.Length + 1);
            sr.sprite = sprites[r - 1];
        }
        else if (escenita == "JueguesitoDia1" || escenita == "JueguesitoDia2")
        {
            switch (contadorPersonas)
            {
                case 0:
                    //SpriteRenderer sr = o.transform.GetComponentInChildren<SpriteRenderer>();
                    sr.sprite = sprites[5];
                    break;
                case 1:
                    sr.sprite = sprites[0];
                    break;
                case 2:
                    sr.sprite = sprites[4];
                    break;
                case 3:
                    sr.sprite = sprites[2];
                    break;
                case 4:
                    sr.sprite = sprites[3];
                    break;
                case 5:
                    sr.sprite = sprites[1];
                    break;
                default:
                    //SpriteRenderer sr = o.transform.GetComponentInChildren<SpriteRenderer>();
                    int r = UnityEngine.Random.Range(1, sprites.Length + 1);
                    sr.sprite = sprites[r - 1];
                    break;
            }
            /* SpriteRenderer sr = o.transform.GetComponentInChildren<SpriteRenderer>();
            int r = UnityEngine.Random.Range(1, sprites.Length + 1);
            sr.sprite = sprites[r - 1]; */
        }
        else if (escenita == "JueguesitoDia3")
        {
            switch (contadorPersonas)
            {
                case 0:
                    //SpriteRenderer sr = o.transform.GetComponentInChildren<SpriteRenderer>();
                    sr.sprite = sprites[5];
                    break;
                case 1:
                    sr.sprite = sprites[0];
                    break;
                case 2:
                    sr.sprite = sprites[4];
                    break;
                case 3:
                    sr.sprite = sprites[2];
                    break;
                case 4:
                    sr.sprite = sprites[0];
                    break;
                case 5:
                    sr.sprite = sprites[1];
                    break;
                default:
                    //SpriteRenderer sr = o.transform.GetComponentInChildren<SpriteRenderer>();
                    int r = UnityEngine.Random.Range(1, sprites.Length + 1);
                    sr.sprite = sprites[r - 1];
                    break;
            }
        }
        else if (escenita == "JueguesitoDia4")
        {
            switch (contadorPersonas)
            {
                case 0:
                    //SpriteRenderer sr = o.transform.GetComponentInChildren<SpriteRenderer>();
                    sr.sprite = sprites[5];
                    break;
                case 1:
                    sr.sprite = sprites[0];
                    break;
                case 2:
                    sr.sprite = sprites[4];
                    break;
                case 3:
                    sr.sprite = sprites[0];
                    break;
                case 4:
                    sr.sprite = sprites[0];
                    break;
                case 5:
                    sr.sprite = sprites[0];
                    break;
                default:
                    //SpriteRenderer sr = o.transform.GetComponentInChildren<SpriteRenderer>();
                    int r = UnityEngine.Random.Range(1, sprites.Length + 1);
                    sr.sprite = sprites[r - 1];
                    break;
            }
        }
        else if (escenita == "JueguesitoDia5")
        {
            switch (contadorPersonas)
            {
                case 0:
                    //SpriteRenderer sr = o.transform.GetComponentInChildren<SpriteRenderer>();
                    sr.sprite = sprites[0];
                    break;
                case 1:
                    sr.sprite = sprites[0];
                    break;
                case 2:
                    sr.sprite = sprites[0];
                    break;
                case 3:
                    sr.sprite = sprites[0];
                    break;
                case 4:
                    sr.sprite = sprites[0];
                    break;
                case 5:
                    sr.sprite = sprites[0];
                    break;
                default:
                    //SpriteRenderer sr = o.transform.GetComponentInChildren<SpriteRenderer>();
                    int r = UnityEngine.Random.Range(1, sprites.Length + 1);
                    sr.sprite = sprites[r - 1];
                    break;
            }
        }
    }

    public void Nombresitos()
    {
        ra = 0;
        ra = UnityEngine.Random.Range(0, 3);
        int cualEstaMal = UnityEngine.Random.Range(0, 2);
        int n1 = UnityEngine.Random.Range(0, 100000);
        int n2;
        if (ra == 0)
            n2 = UnityEngine.Random.Range(0, 100000);
        else
            n2 = n1;

        // DNI

        if (escenita == "JueguesitoContrarreloj" || escenita == "JueguesitoInfinito")
        {
            h.transform.GetComponentsInChildren<TMP_Text>()[0].text = datosgatos[n1][0];
            h.transform.GetComponentsInChildren<TMP_Text>()[1].text = datosgatos[n1][1];
            h.transform.GetComponentsInChildren<TMP_Text>()[2].text = datosgatos[n1][6];
            h.transform.GetComponentsInChildren<TMP_Text>()[3].text = datosgatos[n1][2];
            h.transform.GetComponentsInChildren<TMP_Text>()[4].text = datosgatos[n1][7];
            h.transform.GetComponentsInChildren<TMP_Text>()[5].text = datosgatos[n1][3];
            h.transform.GetComponentsInChildren<TMP_Text>()[6].text = datosgatos[n1][4];

            // carnet escolar
            i.transform.GetComponentsInChildren<TMP_Text>()[0].text = datosgatos[n2][5];
            i.transform.GetComponentsInChildren<TMP_Text>()[1].text = datosgatos[n2][6];
            i.transform.GetComponentsInChildren<TMP_Text>()[2].text =
                datosgatos[n2][0] + " " + datosgatos[n1][1];
            i.transform.GetComponentsInChildren<TMP_Text>()[3].text = datosgatos[n2][14];

            if (doks == 1)
            {
                // certificado vacunacion
                j.transform.GetComponentsInChildren<TMP_Text>()[0].text =
                    datosgatos[n2][0] + " " + datosgatos[n1][1];
                j.transform.GetComponentsInChildren<TMP_Text>()[1].text = datosgatos[n1][4];
                j.transform.GetComponentsInChildren<TMP_Text>()[2].text = datosgatos[n1][11]; //fechas
                j.transform.GetComponentsInChildren<TMP_Text>()[3].text = datosgatos[n1][12];
                j.transform.GetComponentsInChildren<TMP_Text>()[4].text = datosgatos[n1][13];
                j.transform.GetComponentsInChildren<TMP_Text>()[5].text = datosgatos[n1][8];
                j.transform.GetComponentsInChildren<TMP_Text>()[6].text = datosgatos[n1][9];
                j.transform.GetComponentsInChildren<TMP_Text>()[7].text = datosgatos[n1][10];
            }
        }
        if (escenita == "JueguesitoDia1")
        {
            switch (contadorPersonas)
            {
                case 1:
                    ra = 1;
                    h.transform.GetComponentsInChildren<TMP_Text>()[0].text = "Señor";
                    h.transform.GetComponentsInChildren<TMP_Text>()[1].text = "Director";
                    h.transform.GetComponentsInChildren<TMP_Text>()[2].text = "01/01/200 a.C.";
                    h.transform.GetComponentsInChildren<TMP_Text>()[3].text = "Profesor";
                    h.transform.GetComponentsInChildren<TMP_Text>()[4].text = "No caduca";
                    h.transform.GetComponentsInChildren<TMP_Text>()[5].text = "Cítera";
                    h.transform.GetComponentsInChildren<TMP_Text>()[6].text = "00000001A";
                    break;
                case 2:
                    ra = 1;
                    h.transform.GetComponentsInChildren<TMP_Text>()[0].text = "Skibididop";
                    h.transform.GetComponentsInChildren<TMP_Text>()[1].text = "Dop Dop";
                    h.transform.GetComponentsInChildren<TMP_Text>()[2].text = "Skibididop";
                    h.transform.GetComponentsInChildren<TMP_Text>()[3].text = "Dop dop";
                    h.transform.GetComponentsInChildren<TMP_Text>()[4].text = "Skibididop";
                    h.transform.GetComponentsInChildren<TMP_Text>()[5].text = "Filipinas";
                    h.transform.GetComponentsInChildren<TMP_Text>()[6].text = "Skibididop";
                    break;
                case 3:
                    ra = 1;
                    h.transform.GetComponentsInChildren<TMP_Text>()[0].text = "Ramón";
                    h.transform.GetComponentsInChildren<TMP_Text>()[1].text = "El Víbora";
                    h.transform.GetComponentsInChildren<TMP_Text>()[2].text = "16/07/2001";
                    h.transform.GetComponentsInChildren<TMP_Text>()[3].text = "Serpiente";
                    h.transform.GetComponentsInChildren<TMP_Text>()[4].text = "16/02/2026";
                    h.transform.GetComponentsInChildren<TMP_Text>()[5].text = "Bahamas";
                    h.transform.GetComponentsInChildren<TMP_Text>()[6].text = "53129131L";
                    break;
                case 4:
                    ra = 1;
                    h.transform.GetComponentsInChildren<TMP_Text>()[0].text = "Sujeto";
                    h.transform.GetComponentsInChildren<TMP_Text>()[1].text = "E. Necrófago";
                    h.transform.GetComponentsInChildren<TMP_Text>()[2].text = "21/99/2281";
                    h.transform.GetComponentsInChildren<TMP_Text>()[3].text = "Necrótico";
                    h.transform.GetComponentsInChildren<TMP_Text>()[4].text = "21/99/2287";
                    h.transform.GetComponentsInChildren<TMP_Text>()[5].text = "Mojave";
                    h.transform.GetComponentsInChildren<TMP_Text>()[6].text = "00000147E";
                    break;
                case 5:
                    ra = 1;
                    h.transform.GetComponentsInChildren<TMP_Text>()[0].text = "Fidel";
                    h.transform.GetComponentsInChildren<TMP_Text>()[1].text = "Castro";
                    h.transform.GetComponentsInChildren<TMP_Text>()[2].text = "13/08/1926";
                    h.transform.GetComponentsInChildren<TMP_Text>()[3].text = "Comandante";
                    h.transform.GetComponentsInChildren<TMP_Text>()[4].text = "Pa siempre";
                    h.transform.GetComponentsInChildren<TMP_Text>()[5].text = "Cuba";
                    h.transform.GetComponentsInChildren<TMP_Text>()[6].text = "C010159U";
                    break;
                case 6:
                    ra = 1;
                    h.transform.GetComponentsInChildren<TMP_Text>()[0].text = "Sr. Rodrigo";
                    h.transform.GetComponentsInChildren<TMP_Text>()[1].text = "Mendigo";
                    h.transform.GetComponentsInChildren<TMP_Text>()[2].text = "No tiene";
                    h.transform.GetComponentsInChildren<TMP_Text>()[3].text = "H";
                    h.transform.GetComponentsInChildren<TMP_Text>()[4].text = "Pronto";
                    h.transform.GetComponentsInChildren<TMP_Text>()[5].text = "Sotenbori";
                    h.transform.GetComponentsInChildren<TMP_Text>()[6].text = "12345678G";
                    break;
            }
        }
        if (escenita == "JueguesitoDia2")
        {
            switch (contadorPersonas)
            {
                case 1:
                    ra = 1;
                    h.transform.GetComponentsInChildren<TMP_Text>()[0].text = "Señor";
                    h.transform.GetComponentsInChildren<TMP_Text>()[1].text = "Director";
                    h.transform.GetComponentsInChildren<TMP_Text>()[2].text = "01/01/200 a.C.";
                    h.transform.GetComponentsInChildren<TMP_Text>()[3].text = "Profesor";
                    h.transform.GetComponentsInChildren<TMP_Text>()[4].text = "No caduca";
                    h.transform.GetComponentsInChildren<TMP_Text>()[5].text = "Cítera";
                    h.transform.GetComponentsInChildren<TMP_Text>()[6].text = "00000001A";

                    i.transform.GetComponentsInChildren<TMP_Text>()[0].text = "Soy el director";
                    i.transform.GetComponentsInChildren<TMP_Text>()[1].text = "01/01/200 a.C.";
                    i.transform.GetComponentsInChildren<TMP_Text>()[2].text = "Señor director";
                    i.transform.GetComponentsInChildren<TMP_Text>()[3].text = "Jefe";
                    break;
                case 2:
                    ra = 1;
                    h.transform.GetComponentsInChildren<TMP_Text>()[0].text = "Skibididop";
                    h.transform.GetComponentsInChildren<TMP_Text>()[1].text = "Dop Dop";
                    h.transform.GetComponentsInChildren<TMP_Text>()[2].text = "Skibididop";
                    h.transform.GetComponentsInChildren<TMP_Text>()[3].text = "Dop dop";
                    h.transform.GetComponentsInChildren<TMP_Text>()[4].text = "Skibididop";
                    h.transform.GetComponentsInChildren<TMP_Text>()[5].text = "Filipinas";
                    h.transform.GetComponentsInChildren<TMP_Text>()[6].text = "Skibididop";

                    i.transform.GetComponentsInChildren<TMP_Text>()[0].text = "Skibididop";
                    i.transform.GetComponentsInChildren<TMP_Text>()[1].text = "Skibididop";
                    i.transform.GetComponentsInChildren<TMP_Text>()[2].text = "Skibididop dop dop";
                    i.transform.GetComponentsInChildren<TMP_Text>()[3].text = "Skibididop";
                    break;
                case 3:
                    ra = 1;
                    h.transform.GetComponentsInChildren<TMP_Text>()[0].text = "Ramón";
                    h.transform.GetComponentsInChildren<TMP_Text>()[1].text = "El Víbora";
                    h.transform.GetComponentsInChildren<TMP_Text>()[2].text = "16/07/2001";
                    h.transform.GetComponentsInChildren<TMP_Text>()[3].text = "Serpiente";
                    h.transform.GetComponentsInChildren<TMP_Text>()[4].text = "16/02/2026";
                    h.transform.GetComponentsInChildren<TMP_Text>()[5].text = "Bahamas";
                    h.transform.GetComponentsInChildren<TMP_Text>()[6].text = "53129131L";

                    i.transform.GetComponentsInChildren<TMP_Text>()[0].text = "67";
                    i.transform.GetComponentsInChildren<TMP_Text>()[1].text = "16/07/2001";
                    i.transform.GetComponentsInChildren<TMP_Text>()[2].text = "Ramón El Víbora";
                    i.transform.GetComponentsInChildren<TMP_Text>()[3].text = "Emprendimiento";
                    break;
                case 4:
                    ra = 1;
                    h.transform.GetComponentsInChildren<TMP_Text>()[0].text = "Sujeto";
                    h.transform.GetComponentsInChildren<TMP_Text>()[1].text = "E. Necrófago";
                    h.transform.GetComponentsInChildren<TMP_Text>()[2].text = "21/99/2281";
                    h.transform.GetComponentsInChildren<TMP_Text>()[3].text = "Necrótico";
                    h.transform.GetComponentsInChildren<TMP_Text>()[4].text = "21/99/2287";
                    h.transform.GetComponentsInChildren<TMP_Text>()[5].text = "Mojave";
                    h.transform.GetComponentsInChildren<TMP_Text>()[6].text = "00000147E";

                    i.transform.GetComponentsInChildren<TMP_Text>()[0].text = "347";
                    i.transform.GetComponentsInChildren<TMP_Text>()[1].text = "21/99/2281";
                    i.transform.GetComponentsInChildren<TMP_Text>()[2].text = "Sujeto E. Necrófago";
                    i.transform.GetComponentsInChildren<TMP_Text>()[3].text = "Demacración";
                    break;
                case 5:
                    ra = 1;
                    h.transform.GetComponentsInChildren<TMP_Text>()[0].text = "Fidel";
                    h.transform.GetComponentsInChildren<TMP_Text>()[1].text = "Castro";
                    h.transform.GetComponentsInChildren<TMP_Text>()[2].text = "13/08/1926";
                    h.transform.GetComponentsInChildren<TMP_Text>()[3].text = "Comandante";
                    h.transform.GetComponentsInChildren<TMP_Text>()[4].text = "Pa siempre";
                    h.transform.GetComponentsInChildren<TMP_Text>()[5].text = "Cuba";
                    h.transform.GetComponentsInChildren<TMP_Text>()[6].text = "C010159U";

                    i.transform.GetComponentsInChildren<TMP_Text>()[0].text = "53";
                    i.transform.GetComponentsInChildren<TMP_Text>()[1].text = "13/08/1926";
                    i.transform.GetComponentsInChildren<TMP_Text>()[2].text = "Fidel Castro";
                    i.transform.GetComponentsInChildren<TMP_Text>()[3].text = "Socialismo";
                    break;
                case 6:
                    ra = 1;
                    h.transform.GetComponentsInChildren<TMP_Text>()[0].text = "Sr. Rodrigo";
                    h.transform.GetComponentsInChildren<TMP_Text>()[1].text = "Mendigo";
                    h.transform.GetComponentsInChildren<TMP_Text>()[2].text = "No tiene";
                    h.transform.GetComponentsInChildren<TMP_Text>()[3].text = "H";
                    h.transform.GetComponentsInChildren<TMP_Text>()[4].text = "Pronto";
                    h.transform.GetComponentsInChildren<TMP_Text>()[5].text = "Sotenbori";
                    h.transform.GetComponentsInChildren<TMP_Text>()[6].text = "12345678G";

                    i.transform.GetComponentsInChildren<TMP_Text>()[0].text = "0";
                    i.transform.GetComponentsInChildren<TMP_Text>()[1].text = "No tiene";
                    i.transform.GetComponentsInChildren<TMP_Text>()[2].text = "Sr. Rodrigo Mendigo";
                    i.transform.GetComponentsInChildren<TMP_Text>()[3].text = "Demacración";
                    break;
            }
        }
        if (escenita == "JueguesitoDia3")
        {
            switch (contadorPersonas)
            {
                case 1:
                    ra = 1;
                    h.transform.GetComponentsInChildren<TMP_Text>()[0].text = "Señor";
                    h.transform.GetComponentsInChildren<TMP_Text>()[1].text = "Director";
                    h.transform.GetComponentsInChildren<TMP_Text>()[2].text = "01/01/200 a.C.";
                    h.transform.GetComponentsInChildren<TMP_Text>()[3].text = "Profesor";
                    h.transform.GetComponentsInChildren<TMP_Text>()[4].text = "No caduca";
                    h.transform.GetComponentsInChildren<TMP_Text>()[5].text = "Cítera";
                    h.transform.GetComponentsInChildren<TMP_Text>()[6].text = "00000001A";

                    i.transform.GetComponentsInChildren<TMP_Text>()[0].text = "Soy el director";
                    i.transform.GetComponentsInChildren<TMP_Text>()[1].text = "01/01/200 a.C.";
                    i.transform.GetComponentsInChildren<TMP_Text>()[2].text = "Señor director";
                    i.transform.GetComponentsInChildren<TMP_Text>()[3].text = "Jefe";
                    break;
                case 2:
                    ra = 1;
                    h.transform.GetComponentsInChildren<TMP_Text>()[0].text = "Skibididop";
                    h.transform.GetComponentsInChildren<TMP_Text>()[1].text = "Dop Dop";
                    h.transform.GetComponentsInChildren<TMP_Text>()[2].text = "Skibididop";
                    h.transform.GetComponentsInChildren<TMP_Text>()[3].text = "Dop dop";
                    h.transform.GetComponentsInChildren<TMP_Text>()[4].text = "Skibididop";
                    h.transform.GetComponentsInChildren<TMP_Text>()[5].text = "Filipinas";
                    h.transform.GetComponentsInChildren<TMP_Text>()[6].text = "Skibididop";

                    i.transform.GetComponentsInChildren<TMP_Text>()[0].text = "Skibididop";
                    i.transform.GetComponentsInChildren<TMP_Text>()[1].text = "Skibididop";
                    i.transform.GetComponentsInChildren<TMP_Text>()[2].text = "Skibididop dop dop";
                    i.transform.GetComponentsInChildren<TMP_Text>()[3].text = "Skibididop";
                    break;
                case 3:
                    ra = 1;
                    h.transform.GetComponentsInChildren<TMP_Text>()[0].text = "Ramón";
                    h.transform.GetComponentsInChildren<TMP_Text>()[1].text = "El Víbora";
                    h.transform.GetComponentsInChildren<TMP_Text>()[2].text = "16/07/2001";
                    h.transform.GetComponentsInChildren<TMP_Text>()[3].text = "Serpiente";
                    h.transform.GetComponentsInChildren<TMP_Text>()[4].text = "16/02/2026";
                    h.transform.GetComponentsInChildren<TMP_Text>()[5].text = "Bahamas";
                    h.transform.GetComponentsInChildren<TMP_Text>()[6].text = "53129131L";

                    i.transform.GetComponentsInChildren<TMP_Text>()[0].text = "67";
                    i.transform.GetComponentsInChildren<TMP_Text>()[1].text = "16/07/2001";
                    i.transform.GetComponentsInChildren<TMP_Text>()[2].text = "Ramón El Víbora";
                    i.transform.GetComponentsInChildren<TMP_Text>()[3].text = "Emprendimiento";
                    break;
                case 4:
                    ra = 1;
                    h.transform.GetComponentsInChildren<TMP_Text>()[0].text = "Sujeto";
                    h.transform.GetComponentsInChildren<TMP_Text>()[1].text = "E. Necrófago";
                    h.transform.GetComponentsInChildren<TMP_Text>()[2].text = "21/99/2281";
                    h.transform.GetComponentsInChildren<TMP_Text>()[3].text = "Necrótico";
                    h.transform.GetComponentsInChildren<TMP_Text>()[4].text = "21/99/2287";
                    h.transform.GetComponentsInChildren<TMP_Text>()[5].text = "Mojave";
                    h.transform.GetComponentsInChildren<TMP_Text>()[6].text = "00000147E";

                    i.transform.GetComponentsInChildren<TMP_Text>()[0].text = "347";
                    i.transform.GetComponentsInChildren<TMP_Text>()[1].text = "21/99/2281";
                    i.transform.GetComponentsInChildren<TMP_Text>()[2].text = "Sujeto E. Necrófago";
                    i.transform.GetComponentsInChildren<TMP_Text>()[3].text = "Demacración";
                    break;
                case 5:
                    ra = 1;
                    h.transform.GetComponentsInChildren<TMP_Text>()[0].text = "Skibididop";
                    h.transform.GetComponentsInChildren<TMP_Text>()[1].text = "Dop Dop";
                    h.transform.GetComponentsInChildren<TMP_Text>()[2].text = "Skibididop";
                    h.transform.GetComponentsInChildren<TMP_Text>()[3].text = "Dop dop";
                    h.transform.GetComponentsInChildren<TMP_Text>()[4].text = "Skibididop";
                    h.transform.GetComponentsInChildren<TMP_Text>()[5].text = "Filipinas";
                    h.transform.GetComponentsInChildren<TMP_Text>()[6].text = "Skibididop";

                    i.transform.GetComponentsInChildren<TMP_Text>()[0].text = "Skibididop";
                    i.transform.GetComponentsInChildren<TMP_Text>()[1].text = "Skibididop";
                    i.transform.GetComponentsInChildren<TMP_Text>()[2].text = "Skibididop dop dop";
                    i.transform.GetComponentsInChildren<TMP_Text>()[3].text = "Skibididop";
                    break;
                case 6:
                    ra = 1;
                    h.transform.GetComponentsInChildren<TMP_Text>()[0].text = "Sr. Rodrigo";
                    h.transform.GetComponentsInChildren<TMP_Text>()[1].text = "Mendigo";
                    h.transform.GetComponentsInChildren<TMP_Text>()[2].text = "No tiene";
                    h.transform.GetComponentsInChildren<TMP_Text>()[3].text = "H";
                    h.transform.GetComponentsInChildren<TMP_Text>()[4].text = "Pronto";
                    h.transform.GetComponentsInChildren<TMP_Text>()[5].text = "Sotenbori";
                    h.transform.GetComponentsInChildren<TMP_Text>()[6].text = "12345678G";

                    i.transform.GetComponentsInChildren<TMP_Text>()[0].text = "0";
                    i.transform.GetComponentsInChildren<TMP_Text>()[1].text = "No tiene";
                    i.transform.GetComponentsInChildren<TMP_Text>()[2].text = "Sr. Rodrigo Mendigo";
                    i.transform.GetComponentsInChildren<TMP_Text>()[3].text = "Demacración";
                    break;
            }
        }
        if (escenita == "JueguesitoDia4")
        {
            switch (contadorPersonas)
            {
                case 1:
                    ra = 1;
                    h.transform.GetComponentsInChildren<TMP_Text>()[0].text = "Señor";
                    h.transform.GetComponentsInChildren<TMP_Text>()[1].text = "Director";
                    h.transform.GetComponentsInChildren<TMP_Text>()[2].text = "01/01/200 a.C.";
                    h.transform.GetComponentsInChildren<TMP_Text>()[3].text = "Profesor";
                    h.transform.GetComponentsInChildren<TMP_Text>()[4].text = "No caduca";
                    h.transform.GetComponentsInChildren<TMP_Text>()[5].text = "Cítera";
                    h.transform.GetComponentsInChildren<TMP_Text>()[6].text = "00000001A";

                    i.transform.GetComponentsInChildren<TMP_Text>()[0].text = "Soy el director";
                    i.transform.GetComponentsInChildren<TMP_Text>()[1].text = "01/01/200 a.C.";
                    i.transform.GetComponentsInChildren<TMP_Text>()[2].text = "Señor director";
                    i.transform.GetComponentsInChildren<TMP_Text>()[3].text = "Jefe";

                    j.transform.GetComponentsInChildren<TMP_Text>()[0].text = "Señor Director";
                    j.transform.GetComponentsInChildren<TMP_Text>()[1].text = "00000001A";
                    j.transform.GetComponentsInChildren<TMP_Text>()[2].text = "01/01/0001"; //fechas
                    j.transform.GetComponentsInChildren<TMP_Text>()[3].text = "01/01/0001";
                    j.transform.GetComponentsInChildren<TMP_Text>()[4].text = "01/01/0001";
                    j.transform.GetComponentsInChildren<TMP_Text>()[5].text = "Covid";
                    j.transform.GetComponentsInChildren<TMP_Text>()[6].text = "Tuberculosis";
                    j.transform.GetComponentsInChildren<TMP_Text>()[7].text = "Potato Virus Y";
                    break;
                case 2:
                    ra = 1;
                    h.transform.GetComponentsInChildren<TMP_Text>()[0].text = "Skibididop";
                    h.transform.GetComponentsInChildren<TMP_Text>()[1].text = "Dop Dop";
                    h.transform.GetComponentsInChildren<TMP_Text>()[2].text = "Skibididop";
                    h.transform.GetComponentsInChildren<TMP_Text>()[3].text = "Dop dop";
                    h.transform.GetComponentsInChildren<TMP_Text>()[4].text = "Skibididop";
                    h.transform.GetComponentsInChildren<TMP_Text>()[5].text = "Filipinas";
                    h.transform.GetComponentsInChildren<TMP_Text>()[6].text = "Skibididop";

                    i.transform.GetComponentsInChildren<TMP_Text>()[0].text = "Skibididop";
                    i.transform.GetComponentsInChildren<TMP_Text>()[1].text = "Skibididop";
                    i.transform.GetComponentsInChildren<TMP_Text>()[2].text = "Skibididop dop dop";
                    i.transform.GetComponentsInChildren<TMP_Text>()[3].text = "Skibididop";

                    j.transform.GetComponentsInChildren<TMP_Text>()[0].text = "Skibididop Dop Dop";
                    j.transform.GetComponentsInChildren<TMP_Text>()[1].text = "Skibididop";
                    j.transform.GetComponentsInChildren<TMP_Text>()[2].text = "Skibididop"; //fechas
                    j.transform.GetComponentsInChildren<TMP_Text>()[3].text = "Dop";
                    j.transform.GetComponentsInChildren<TMP_Text>()[4].text = "Dop";
                    j.transform.GetComponentsInChildren<TMP_Text>()[5].text = "Skibididop";
                    j.transform.GetComponentsInChildren<TMP_Text>()[6].text = "Dop";
                    j.transform.GetComponentsInChildren<TMP_Text>()[7].text = "Dop";
                    break;
                case 3:
                    ra = 1;
                    h.transform.GetComponentsInChildren<TMP_Text>()[0].text = "Ramón";
                    h.transform.GetComponentsInChildren<TMP_Text>()[1].text = "El Víbora";
                    h.transform.GetComponentsInChildren<TMP_Text>()[2].text = "16/07/2001";
                    h.transform.GetComponentsInChildren<TMP_Text>()[3].text = "Serpiente";
                    h.transform.GetComponentsInChildren<TMP_Text>()[4].text = "16/02/2026";
                    h.transform.GetComponentsInChildren<TMP_Text>()[5].text = "Bahamas";
                    h.transform.GetComponentsInChildren<TMP_Text>()[6].text = "53129131L";

                    i.transform.GetComponentsInChildren<TMP_Text>()[0].text = "67";
                    i.transform.GetComponentsInChildren<TMP_Text>()[1].text = "16/07/2001";
                    i.transform.GetComponentsInChildren<TMP_Text>()[2].text = "Ramón El Víbora";
                    i.transform.GetComponentsInChildren<TMP_Text>()[3].text = "Emprendimiento";

                    j.transform.GetComponentsInChildren<TMP_Text>()[0].text = "Ramón El Víbora";
                    j.transform.GetComponentsInChildren<TMP_Text>()[1].text = "53129131L";
                    j.transform.GetComponentsInChildren<TMP_Text>()[2].text = "30/03/2014"; //fechas
                    j.transform.GetComponentsInChildren<TMP_Text>()[3].text = "30/03/2014";
                    j.transform.GetComponentsInChildren<TMP_Text>()[4].text = "30/03/2014";
                    j.transform.GetComponentsInChildren<TMP_Text>()[5].text = "Disecdisis";
                    j.transform.GetComponentsInChildren<TMP_Text>()[6].text = "Ectoparásitos ";
                    j.transform.GetComponentsInChildren<TMP_Text>()[7].text = "Blister";
                    break;
                case 4:
                    ra = 1;
                    h.transform.GetComponentsInChildren<TMP_Text>()[0].text = "Skibididop";
                    h.transform.GetComponentsInChildren<TMP_Text>()[1].text = "Dop Dop";
                    h.transform.GetComponentsInChildren<TMP_Text>()[2].text = "Skibididop";
                    h.transform.GetComponentsInChildren<TMP_Text>()[3].text = "Dop dop";
                    h.transform.GetComponentsInChildren<TMP_Text>()[4].text = "Skibididop";
                    h.transform.GetComponentsInChildren<TMP_Text>()[5].text = "Filipinas";
                    h.transform.GetComponentsInChildren<TMP_Text>()[6].text = "Skibididop";

                    i.transform.GetComponentsInChildren<TMP_Text>()[0].text = "Skibididop";
                    i.transform.GetComponentsInChildren<TMP_Text>()[1].text = "Skibididop";
                    i.transform.GetComponentsInChildren<TMP_Text>()[2].text = "Skibididop dop dop";
                    i.transform.GetComponentsInChildren<TMP_Text>()[3].text = "Skibididop";

                    j.transform.GetComponentsInChildren<TMP_Text>()[0].text = "Skibididop Dop Dop";
                    j.transform.GetComponentsInChildren<TMP_Text>()[1].text = "Skibididop";
                    j.transform.GetComponentsInChildren<TMP_Text>()[2].text = "Skibididop"; //fechas
                    j.transform.GetComponentsInChildren<TMP_Text>()[3].text = "Dop";
                    j.transform.GetComponentsInChildren<TMP_Text>()[4].text = "Dop";
                    j.transform.GetComponentsInChildren<TMP_Text>()[5].text = "Skibididop";
                    j.transform.GetComponentsInChildren<TMP_Text>()[6].text = "Dop";
                    j.transform.GetComponentsInChildren<TMP_Text>()[7].text = "Dop";
                    break;
                case 5:
                    ra = 1;
                    h.transform.GetComponentsInChildren<TMP_Text>()[0].text = "Skibididop";
                    h.transform.GetComponentsInChildren<TMP_Text>()[1].text = "Dop Dop";
                    h.transform.GetComponentsInChildren<TMP_Text>()[2].text = "Skibididop";
                    h.transform.GetComponentsInChildren<TMP_Text>()[3].text = "Dop dop";
                    h.transform.GetComponentsInChildren<TMP_Text>()[4].text = "Skibididop";
                    h.transform.GetComponentsInChildren<TMP_Text>()[5].text = "Filipinas";
                    h.transform.GetComponentsInChildren<TMP_Text>()[6].text = "Skibididop";

                    i.transform.GetComponentsInChildren<TMP_Text>()[0].text = "Skibididop";
                    i.transform.GetComponentsInChildren<TMP_Text>()[1].text = "Skibididop";
                    i.transform.GetComponentsInChildren<TMP_Text>()[2].text = "Skibididop dop dop";
                    i.transform.GetComponentsInChildren<TMP_Text>()[3].text = "Skibididop";

                    j.transform.GetComponentsInChildren<TMP_Text>()[0].text = "Skibididop Dop Dop";
                    j.transform.GetComponentsInChildren<TMP_Text>()[1].text = "Skibididop";
                    j.transform.GetComponentsInChildren<TMP_Text>()[2].text = "Skibididop"; //fechas
                    j.transform.GetComponentsInChildren<TMP_Text>()[3].text = "Dop";
                    j.transform.GetComponentsInChildren<TMP_Text>()[4].text = "Dop";
                    j.transform.GetComponentsInChildren<TMP_Text>()[5].text = "Skibididop";
                    j.transform.GetComponentsInChildren<TMP_Text>()[6].text = "Dop";
                    j.transform.GetComponentsInChildren<TMP_Text>()[7].text = "Dop";
                    break;
                case 6:
                    ra = 1;
                    h.transform.GetComponentsInChildren<TMP_Text>()[0].text = "Skibididop";
                    h.transform.GetComponentsInChildren<TMP_Text>()[1].text = "Dop Dop";
                    h.transform.GetComponentsInChildren<TMP_Text>()[2].text = "Skibididop";
                    h.transform.GetComponentsInChildren<TMP_Text>()[3].text = "Dop dop";
                    h.transform.GetComponentsInChildren<TMP_Text>()[4].text = "Skibididop";
                    h.transform.GetComponentsInChildren<TMP_Text>()[5].text = "Filipinas";
                    h.transform.GetComponentsInChildren<TMP_Text>()[6].text = "Skibididop";

                    i.transform.GetComponentsInChildren<TMP_Text>()[0].text = "Skibididop";
                    i.transform.GetComponentsInChildren<TMP_Text>()[1].text = "Skibididop";
                    i.transform.GetComponentsInChildren<TMP_Text>()[2].text = "Skibididop dop dop";
                    i.transform.GetComponentsInChildren<TMP_Text>()[3].text = "Skibididop";

                    j.transform.GetComponentsInChildren<TMP_Text>()[0].text = "Skibididop Dop Dop";
                    j.transform.GetComponentsInChildren<TMP_Text>()[1].text = "Skibididop";
                    j.transform.GetComponentsInChildren<TMP_Text>()[2].text = "Skibididop"; //fechas
                    j.transform.GetComponentsInChildren<TMP_Text>()[3].text = "Dop";
                    j.transform.GetComponentsInChildren<TMP_Text>()[4].text = "Dop";
                    j.transform.GetComponentsInChildren<TMP_Text>()[5].text = "Skibididop";
                    j.transform.GetComponentsInChildren<TMP_Text>()[6].text = "Dop";
                    j.transform.GetComponentsInChildren<TMP_Text>()[7].text = "Dop";
                    break;
            }
        }
        if (escenita == "JueguesitoDia5")
        {
            switch (contadorPersonas)
            {
                case 1:
                    ra = 1;
                    h.transform.GetComponentsInChildren<TMP_Text>()[0].text = "Skibididop";
                    h.transform.GetComponentsInChildren<TMP_Text>()[1].text = "Dop Dop";
                    h.transform.GetComponentsInChildren<TMP_Text>()[2].text = "Skibididop";
                    h.transform.GetComponentsInChildren<TMP_Text>()[3].text = "Dop dop";
                    h.transform.GetComponentsInChildren<TMP_Text>()[4].text = "Skibididop";
                    h.transform.GetComponentsInChildren<TMP_Text>()[5].text = "Filipinas";
                    h.transform.GetComponentsInChildren<TMP_Text>()[6].text = "Skibididop";

                    i.transform.GetComponentsInChildren<TMP_Text>()[0].text = "Skibididop";
                    i.transform.GetComponentsInChildren<TMP_Text>()[1].text = "Skibididop";
                    i.transform.GetComponentsInChildren<TMP_Text>()[2].text = "Skibididop dop dop";
                    i.transform.GetComponentsInChildren<TMP_Text>()[3].text = "Skibididop";

                    j.transform.GetComponentsInChildren<TMP_Text>()[0].text = "Skibididop Dop Dop";
                    j.transform.GetComponentsInChildren<TMP_Text>()[1].text = "Skibididop";
                    j.transform.GetComponentsInChildren<TMP_Text>()[2].text = "Skibididop"; //fechas
                    j.transform.GetComponentsInChildren<TMP_Text>()[3].text = "Dop";
                    j.transform.GetComponentsInChildren<TMP_Text>()[4].text = "Dop";
                    j.transform.GetComponentsInChildren<TMP_Text>()[5].text = "Skibididop";
                    j.transform.GetComponentsInChildren<TMP_Text>()[6].text = "Dop";
                    j.transform.GetComponentsInChildren<TMP_Text>()[7].text = "Dop";
                    break;
                case 2:
                    ra = 1;
                    h.transform.GetComponentsInChildren<TMP_Text>()[0].text = "Skibididop";
                    h.transform.GetComponentsInChildren<TMP_Text>()[1].text = "Dop Dop";
                    h.transform.GetComponentsInChildren<TMP_Text>()[2].text = "Skibididop";
                    h.transform.GetComponentsInChildren<TMP_Text>()[3].text = "Dop dop";
                    h.transform.GetComponentsInChildren<TMP_Text>()[4].text = "Skibididop";
                    h.transform.GetComponentsInChildren<TMP_Text>()[5].text = "Filipinas";
                    h.transform.GetComponentsInChildren<TMP_Text>()[6].text = "Skibididop";

                    i.transform.GetComponentsInChildren<TMP_Text>()[0].text = "Skibididop";
                    i.transform.GetComponentsInChildren<TMP_Text>()[1].text = "Skibididop";
                    i.transform.GetComponentsInChildren<TMP_Text>()[2].text = "Skibididop dop dop";
                    i.transform.GetComponentsInChildren<TMP_Text>()[3].text = "Skibididop";

                    j.transform.GetComponentsInChildren<TMP_Text>()[0].text = "Skibididop Dop Dop";
                    j.transform.GetComponentsInChildren<TMP_Text>()[1].text = "Skibididop";
                    j.transform.GetComponentsInChildren<TMP_Text>()[2].text = "Skibididop"; //fechas
                    j.transform.GetComponentsInChildren<TMP_Text>()[3].text = "Dop";
                    j.transform.GetComponentsInChildren<TMP_Text>()[4].text = "Dop";
                    j.transform.GetComponentsInChildren<TMP_Text>()[5].text = "Skibididop";
                    j.transform.GetComponentsInChildren<TMP_Text>()[6].text = "Dop";
                    j.transform.GetComponentsInChildren<TMP_Text>()[7].text = "Dop";
                    break;
                case 3:
                    ra = 1;
                    h.transform.GetComponentsInChildren<TMP_Text>()[0].text = "Skibididop";
                    h.transform.GetComponentsInChildren<TMP_Text>()[1].text = "Dop Dop";
                    h.transform.GetComponentsInChildren<TMP_Text>()[2].text = "Skibididop";
                    h.transform.GetComponentsInChildren<TMP_Text>()[3].text = "Dop dop";
                    h.transform.GetComponentsInChildren<TMP_Text>()[4].text = "Skibididop";
                    h.transform.GetComponentsInChildren<TMP_Text>()[5].text = "Filipinas";
                    h.transform.GetComponentsInChildren<TMP_Text>()[6].text = "Skibididop";

                    i.transform.GetComponentsInChildren<TMP_Text>()[0].text = "Skibididop";
                    i.transform.GetComponentsInChildren<TMP_Text>()[1].text = "Skibididop";
                    i.transform.GetComponentsInChildren<TMP_Text>()[2].text = "Skibididop dop dop";
                    i.transform.GetComponentsInChildren<TMP_Text>()[3].text = "Skibididop";

                    j.transform.GetComponentsInChildren<TMP_Text>()[0].text = "Skibididop Dop Dop";
                    j.transform.GetComponentsInChildren<TMP_Text>()[1].text = "Skibididop";
                    j.transform.GetComponentsInChildren<TMP_Text>()[2].text = "Skibididop"; //fechas
                    j.transform.GetComponentsInChildren<TMP_Text>()[3].text = "Dop";
                    j.transform.GetComponentsInChildren<TMP_Text>()[4].text = "Dop";
                    j.transform.GetComponentsInChildren<TMP_Text>()[5].text = "Skibididop";
                    j.transform.GetComponentsInChildren<TMP_Text>()[6].text = "Dop";
                    j.transform.GetComponentsInChildren<TMP_Text>()[7].text = "Dop";
                    break;
                case 4:
                    ra = 1;
                    h.transform.GetComponentsInChildren<TMP_Text>()[0].text = "Skibididop";
                    h.transform.GetComponentsInChildren<TMP_Text>()[1].text = "Dop Dop";
                    h.transform.GetComponentsInChildren<TMP_Text>()[2].text = "Skibididop";
                    h.transform.GetComponentsInChildren<TMP_Text>()[3].text = "Dop dop";
                    h.transform.GetComponentsInChildren<TMP_Text>()[4].text = "Skibididop";
                    h.transform.GetComponentsInChildren<TMP_Text>()[5].text = "Filipinas";
                    h.transform.GetComponentsInChildren<TMP_Text>()[6].text = "Skibididop";

                    i.transform.GetComponentsInChildren<TMP_Text>()[0].text = "Skibididop";
                    i.transform.GetComponentsInChildren<TMP_Text>()[1].text = "Skibididop";
                    i.transform.GetComponentsInChildren<TMP_Text>()[2].text = "Skibididop dop dop";
                    i.transform.GetComponentsInChildren<TMP_Text>()[3].text = "Skibididop";

                    j.transform.GetComponentsInChildren<TMP_Text>()[0].text = "Skibididop Dop Dop";
                    j.transform.GetComponentsInChildren<TMP_Text>()[1].text = "Skibididop";
                    j.transform.GetComponentsInChildren<TMP_Text>()[2].text = "Skibididop"; //fechas
                    j.transform.GetComponentsInChildren<TMP_Text>()[3].text = "Dop";
                    j.transform.GetComponentsInChildren<TMP_Text>()[4].text = "Dop";
                    j.transform.GetComponentsInChildren<TMP_Text>()[5].text = "Skibididop";
                    j.transform.GetComponentsInChildren<TMP_Text>()[6].text = "Dop";
                    j.transform.GetComponentsInChildren<TMP_Text>()[7].text = "Dop";
                    break;
                case 5:
                    ra = 1;
                    h.transform.GetComponentsInChildren<TMP_Text>()[0].text = "Skibididop";
                    h.transform.GetComponentsInChildren<TMP_Text>()[1].text = "Dop Dop";
                    h.transform.GetComponentsInChildren<TMP_Text>()[2].text = "Skibididop";
                    h.transform.GetComponentsInChildren<TMP_Text>()[3].text = "Dop dop";
                    h.transform.GetComponentsInChildren<TMP_Text>()[4].text = "Skibididop";
                    h.transform.GetComponentsInChildren<TMP_Text>()[5].text = "Filipinas";
                    h.transform.GetComponentsInChildren<TMP_Text>()[6].text = "Skibididop";

                    i.transform.GetComponentsInChildren<TMP_Text>()[0].text = "Skibididop";
                    i.transform.GetComponentsInChildren<TMP_Text>()[1].text = "Skibididop";
                    i.transform.GetComponentsInChildren<TMP_Text>()[2].text = "Skibididop dop dop";
                    i.transform.GetComponentsInChildren<TMP_Text>()[3].text = "Skibididop";

                    j.transform.GetComponentsInChildren<TMP_Text>()[0].text = "Skibididop Dop Dop";
                    j.transform.GetComponentsInChildren<TMP_Text>()[1].text = "Skibididop";
                    j.transform.GetComponentsInChildren<TMP_Text>()[2].text = "Skibididop"; //fechas
                    j.transform.GetComponentsInChildren<TMP_Text>()[3].text = "Dop";
                    j.transform.GetComponentsInChildren<TMP_Text>()[4].text = "Dop";
                    j.transform.GetComponentsInChildren<TMP_Text>()[5].text = "Skibididop";
                    j.transform.GetComponentsInChildren<TMP_Text>()[6].text = "Dop";
                    j.transform.GetComponentsInChildren<TMP_Text>()[7].text = "Dop";
                    break;
                case 6:
                    ra = 1;
                    h.transform.GetComponentsInChildren<TMP_Text>()[0].text = "Skibididop";
                    h.transform.GetComponentsInChildren<TMP_Text>()[1].text = "Dop Dop";
                    h.transform.GetComponentsInChildren<TMP_Text>()[2].text = "Skibididop";
                    h.transform.GetComponentsInChildren<TMP_Text>()[3].text = "Dop dop";
                    h.transform.GetComponentsInChildren<TMP_Text>()[4].text = "Skibididop";
                    h.transform.GetComponentsInChildren<TMP_Text>()[5].text = "Filipinas";
                    h.transform.GetComponentsInChildren<TMP_Text>()[6].text = "Skibididop";

                    i.transform.GetComponentsInChildren<TMP_Text>()[0].text = "Skibididop";
                    i.transform.GetComponentsInChildren<TMP_Text>()[1].text = "Skibididop";
                    i.transform.GetComponentsInChildren<TMP_Text>()[2].text = "Skibididop dop dop";
                    i.transform.GetComponentsInChildren<TMP_Text>()[3].text = "Skibididop";

                    j.transform.GetComponentsInChildren<TMP_Text>()[0].text = "Skibididop Dop Dop";
                    j.transform.GetComponentsInChildren<TMP_Text>()[1].text = "Skibididop";
                    j.transform.GetComponentsInChildren<TMP_Text>()[2].text = "Skibididop"; //fechas
                    j.transform.GetComponentsInChildren<TMP_Text>()[3].text = "Dop";
                    j.transform.GetComponentsInChildren<TMP_Text>()[4].text = "Dop";
                    j.transform.GetComponentsInChildren<TMP_Text>()[5].text = "Skibididop";
                    j.transform.GetComponentsInChildren<TMP_Text>()[6].text = "Dop";
                    j.transform.GetComponentsInChildren<TMP_Text>()[7].text = "Dop";
                    break;
            }
        }
    }

    public void Lista()
    {
        string nombre = "Assets\\Game\\Nombres\\datos.csv";
        datosgatos = new List<string[]>();
        using (StreamReader reader = new StreamReader(nombre))
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                datosgatos.Add(line.Split(','));
            }
        }
    }

    // Lineas agregadas

    void PuntuacionInfinito()
    {
        if (tiempoRestante <= 30f)
            puntuacion += 5;
        else if (tiempoRestante <= 60f)
            puntuacion += 4;
        else if (tiempoRestante <= 90f)
            puntuacion += 3;
        else if (tiempoRestante <= 120f)
            puntuacion += 2;
        else
            puntuacion++;
    }

    private void ActualizarPuntuacion()
    {
        if (escenita == "JueguesitoContrarreloj")
        {
            puntuacion = correctito * 100 - incorrectito * 100;

            if (puntuacion < 0)
                puntuacion = 0;

            puntuacionTexto.text = puntuacion + " pts.";
        }
        if (escenita == "JueguesitoInfinito")
        {
            PuntuacionInfinito();
            puntuacionTexto.text = puntuacion.ToString();
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

    public void MostrarCanvasTabla()
    {
        // Activa el Canvas de la escena final
        canvasTabla.gameObject.SetActive(true);
        canvasJuego.gameObject.SetActive(false);
        canvasFinal.gameObject.SetActive(false);
    }

    // Fin de lineas agregadas

    public TMP_Text textitoDialogo;
    public GameObject bocadillo;

    IEnumerator Dialogo()
    {
        if (escenita == "JueguesitoDia1")
        {
            if (GameObject.FindGameObjectsWithTag("Personaje").Length != 0)
            {
                switch (contadorPersonas)
                {
                    case 0:
                        yield return new WaitForSeconds(15);
                        bocadillo.SetActive(true);
                        textitoDialogo.text = "Buenos días.";
                        yield return new WaitForSeconds(3);
                        textitoDialogo.text = "Bienvenido a Seiryo High.";
                        yield return new WaitForSeconds(3);
                        textitoDialogo.text = "Haz tu trabajo y no tendras problemas.";
                        yield return new WaitForSeconds(3);
                        bocadillo.SetActive(false);
                        StartCoroutine(SpawnDocument());
                        contadorPersonas++;
                        break;
                    case 1:
                        yield return new WaitForSeconds(1);
                        audioSource.Play();
                        bocadillo.SetActive(true);
                        textitoDialogo.text = "Skibididop.";
                        yield return new WaitForSeconds(1.75f);
                        textitoDialogo.text = " ";
                        yield return new WaitForSeconds(0.25f);
                        textitoDialogo.text = "Dop.";
                        yield return new WaitForSeconds(1.75f);
                        textitoDialogo.text = " ";
                        yield return new WaitForSeconds(0.25f);
                        textitoDialogo.text = "Dop.";
                        yield return new WaitForSeconds(2);
                        audioSource.Stop();
                        bocadillo.SetActive(false);
                        StartCoroutine(SpawnDocument());
                        contadorPersonas++;
                        break;
                    case 2:
                        yield return new WaitForSeconds(1);
                        bocadillo.SetActive(true);
                        textitoDialogo.text = "Hisssss.";
                        yield return new WaitForSeconds(2);
                        textitoDialogo.text = "Espero poder emprender.";
                        yield return new WaitForSeconds(2);
                        textitoDialogo.text = "Mente de tiburón.";
                        yield return new WaitForSeconds(2);
                        bocadillo.SetActive(false);
                        StartCoroutine(SpawnDocument());
                        contadorPersonas++;
                        break;
                    case 3:
                        yield return new WaitForSeconds(1);
                        bocadillo.SetActive(true);
                        textitoDialogo.text = "GRUAAARGH.";
                        yield return new WaitForSeconds(2);
                        textitoDialogo.text = "Buenos días, Piel Suave.";
                        yield return new WaitForSeconds(2);
                        bocadillo.SetActive(false);
                        StartCoroutine(SpawnDocument());
                        contadorPersonas++;
                        break;
                    case 4:
                        yield return new WaitForSeconds(1);
                        bocadillo.SetActive(true);
                        textitoDialogo.text = "¡Buenos días mi helmano!";
                        yield return new WaitForSeconds(2);
                        textitoDialogo.text = "Acá vengo a ganar conosimiento.";
                        yield return new WaitForSeconds(2);
                        bocadillo.SetActive(false);
                        StartCoroutine(SpawnDocument());
                        contadorPersonas++;
                        break;
                    case 5:
                        yield return new WaitForSeconds(1);
                        bocadillo.SetActive(true);
                        textitoDialogo.text = "...";
                        yield return new WaitForSeconds(2);
                        bocadillo.SetActive(false);
                        StartCoroutine(SpawnDocument());
                        contadorPersonas++;
                        break;
                }
            }
        }
        if (escenita == "JueguesitoDia2")
        {
            if (GameObject.FindGameObjectsWithTag("Personaje").Length != 0)
            {
                switch (contadorPersonas)
                {
                    case 0:
                        yield return new WaitForSeconds(15);
                        bocadillo.SetActive(true);
                        textitoDialogo.text = "Buenos días.";
                        yield return new WaitForSeconds(3);
                        textitoDialogo.text = "Veo que ya se ha adaptado a su puesto.";
                        yield return new WaitForSeconds(3);
                        textitoDialogo.text =
                            "Recuerde que hemos repartido las tarjetas de estudiante.";
                        yield return new WaitForSeconds(3);
                        bocadillo.SetActive(false);
                        StartCoroutine(SpawnDocument());
                        contadorPersonas++;
                        break;
                    case 1:
                        yield return new WaitForSeconds(1);
                        audioSource.Play();
                        bocadillo.SetActive(true);
                        textitoDialogo.text = "Skibididop.";
                        yield return new WaitForSeconds(1.75f);
                        textitoDialogo.text = " ";
                        yield return new WaitForSeconds(0.25f);
                        textitoDialogo.text = "Dop.";
                        yield return new WaitForSeconds(1.75f);
                        textitoDialogo.text = " ";
                        yield return new WaitForSeconds(0.25f);
                        textitoDialogo.text = "Dop.";
                        yield return new WaitForSeconds(2);
                        bocadillo.SetActive(false);
                        audioSource.Stop();
                        StartCoroutine(SpawnDocument());
                        contadorPersonas++;
                        break;
                    case 2:
                        yield return new WaitForSeconds(1);
                        bocadillo.SetActive(true);
                        textitoDialogo.text = "Hisssss.";
                        yield return new WaitForSeconds(2);
                        textitoDialogo.text = "Mi esfuerzo personal ya da sus frutos.";
                        yield return new WaitForSeconds(2);
                        textitoDialogo.text = "Mente de tiburón.";
                        yield return new WaitForSeconds(2);
                        bocadillo.SetActive(false);
                        StartCoroutine(SpawnDocument());
                        contadorPersonas++;
                        break;
                    case 3:
                        yield return new WaitForSeconds(1);
                        bocadillo.SetActive(true);
                        textitoDialogo.text = "GRUAAARGH.";
                        yield return new WaitForSeconds(2);
                        textitoDialogo.text = "Dónde me he metido, Piel Suave.";
                        yield return new WaitForSeconds(2);
                        bocadillo.SetActive(false);
                        StartCoroutine(SpawnDocument());
                        contadorPersonas++;
                        break;
                    case 4:
                        yield return new WaitForSeconds(1);
                        bocadillo.SetActive(true);
                        textitoDialogo.text = "¡Mi helmano!";
                        yield return new WaitForSeconds(2);
                        textitoDialogo.text = "Este lugar parese mu raro.";
                        yield return new WaitForSeconds(2);
                        textitoDialogo.text = "Noto una presensia extraña siguiéndome.";
                        yield return new WaitForSeconds(2);
                        bocadillo.SetActive(false);
                        StartCoroutine(SpawnDocument());
                        contadorPersonas++;
                        break;
                    case 5:
                        yield return new WaitForSeconds(1);
                        bocadillo.SetActive(true);
                        textitoDialogo.text = "...";
                        yield return new WaitForSeconds(2);
                        textitoDialogo.text = "... maldita mano del mono.";
                        yield return new WaitForSeconds(2);
                        bocadillo.SetActive(false);
                        StartCoroutine(SpawnDocument());
                        contadorPersonas++;
                        break;
                }
            }
        }
        if (escenita == "JueguesitoDia3")
        {
            if (GameObject.FindGameObjectsWithTag("Personaje").Length != 0)
            {
                switch (contadorPersonas)
                {
                    case 0:
                        yield return new WaitForSeconds(15);
                        bocadillo.SetActive(true);
                        textitoDialogo.text = "Buenos días.";
                        yield return new WaitForSeconds(3);
                        textitoDialogo.text = "Le veo muy buena cara.";
                        yield return new WaitForSeconds(3);
                        bocadillo.SetActive(false);
                        StartCoroutine(SpawnDocument());
                        contadorPersonas++;
                        break;
                    case 1:
                        yield return new WaitForSeconds(1);
                        audioSource.Play();
                        bocadillo.SetActive(true);
                        textitoDialogo.text = "Skibididop.";
                        yield return new WaitForSeconds(1.75f);
                        textitoDialogo.text = " ";
                        yield return new WaitForSeconds(0.25f);
                        textitoDialogo.text = "Dop.";
                        yield return new WaitForSeconds(1.75f);
                        textitoDialogo.text = " ";
                        yield return new WaitForSeconds(0.25f);
                        textitoDialogo.text = "Dop.";
                        yield return new WaitForSeconds(2);
                        bocadillo.SetActive(false);
                        audioSource.Stop();
                        StartCoroutine(SpawnDocument());
                        contadorPersonas++;
                        break;
                    case 2:
                        yield return new WaitForSeconds(1);
                        bocadillo.SetActive(true);
                        textitoDialogo.text = "Hisssss.";
                        yield return new WaitForSeconds(2);
                        textitoDialogo.text = "El señor Jeremías es el mejor profesor.";
                        yield return new WaitForSeconds(2);
                        textitoDialogo.text = "Mente de tiburón.";
                        yield return new WaitForSeconds(2);
                        bocadillo.SetActive(false);
                        StartCoroutine(SpawnDocument());
                        contadorPersonas++;
                        break;
                    case 3:
                        yield return new WaitForSeconds(1);
                        bocadillo.SetActive(true);
                        textitoDialogo.text = "GRUAAARGH.";
                        yield return new WaitForSeconds(2);
                        textitoDialogo.text = "Piel Suave, en el Mojave me sentía más seguro.";
                        yield return new WaitForSeconds(2);
                        bocadillo.SetActive(false);
                        StartCoroutine(SpawnDocument());
                        contadorPersonas++;
                        break;
                    case 4:
                        yield return new WaitForSeconds(1);
                        audioSource.Play();
                        bocadillo.SetActive(true);
                        textitoDialogo.text = "Skibididop.";
                        yield return new WaitForSeconds(1.75f);
                        textitoDialogo.text = " ";
                        yield return new WaitForSeconds(0.25f);
                        textitoDialogo.text = "Dop.";
                        yield return new WaitForSeconds(1.75f);
                        textitoDialogo.text = " ";
                        yield return new WaitForSeconds(0.25f);
                        textitoDialogo.text = "Dop.";
                        yield return new WaitForSeconds(2);
                        bocadillo.SetActive(false);
                        audioSource.Stop();
                        StartCoroutine(SpawnDocument());
                        contadorPersonas++;
                        break;
                    case 5:
                        yield return new WaitForSeconds(1);
                        bocadillo.SetActive(true);
                        textitoDialogo.text = "...";
                        yield return new WaitForSeconds(2);
                        textitoDialogo.text = "Dónde está Fidel.";
                        yield return new WaitForSeconds(2);
                        bocadillo.SetActive(false);
                        StartCoroutine(SpawnDocument());
                        contadorPersonas++;
                        break;
                }
            }
        }
        if (escenita == "JueguesitoDia4")
        {
            if (GameObject.FindGameObjectsWithTag("Personaje").Length != 0)
            {
                switch (contadorPersonas)
                {
                    case 0:
                        yield return new WaitForSeconds(15);
                        bocadillo.SetActive(true);
                        textitoDialogo.text = "Buenos días.";
                        yield return new WaitForSeconds(3);
                        textitoDialogo.text =
                            "Recuerde que a partir de hoy pedimos los papeles de vacunación.";
                        yield return new WaitForSeconds(3);
                        bocadillo.SetActive(false);
                        StartCoroutine(SpawnDocument());
                        contadorPersonas++;
                        break;
                    case 1:
                        yield return new WaitForSeconds(1);
                        audioSource.Play();
                        bocadillo.SetActive(true);
                        textitoDialogo.text = "Skibididop.";
                        yield return new WaitForSeconds(1.75f);
                        textitoDialogo.text = " ";
                        yield return new WaitForSeconds(0.25f);
                        textitoDialogo.text = "Dop.";
                        yield return new WaitForSeconds(1.75f);
                        textitoDialogo.text = " ";
                        yield return new WaitForSeconds(0.25f);
                        textitoDialogo.text = "Dop.";
                        yield return new WaitForSeconds(2);
                        bocadillo.SetActive(false);
                        audioSource.Stop();
                        StartCoroutine(SpawnDocument());
                        contadorPersonas++;
                        break;
                    case 2:
                        yield return new WaitForSeconds(1);
                        bocadillo.SetActive(true);
                        textitoDialogo.text = "Hisssss.";
                        yield return new WaitForSeconds(2);
                        textitoDialogo.text =
                            "Gracias al señor Jeremías y a mi propio esfuerzo, voy a trabajar por 0 euros.";
                        yield return new WaitForSeconds(2);
                        textitoDialogo.text = "Mente de tiburón.";
                        yield return new WaitForSeconds(2);
                        bocadillo.SetActive(false);
                        StartCoroutine(SpawnDocument());
                        contadorPersonas++;
                        break;
                    case 3:
                        yield return new WaitForSeconds(1);
                        audioSource.Play();
                        bocadillo.SetActive(true);
                        textitoDialogo.text = "Skibididop.";
                        yield return new WaitForSeconds(1.75f);
                        textitoDialogo.text = " ";
                        yield return new WaitForSeconds(0.25f);
                        textitoDialogo.text = "Dop.";
                        yield return new WaitForSeconds(1.75f);
                        textitoDialogo.text = " ";
                        yield return new WaitForSeconds(0.25f);
                        textitoDialogo.text = "Dop.";
                        yield return new WaitForSeconds(2);
                        bocadillo.SetActive(false);
                        audioSource.Stop();
                        StartCoroutine(SpawnDocument());
                        contadorPersonas++;
                        break;
                    case 4:
                        yield return new WaitForSeconds(1);
                        audioSource.Play();
                        bocadillo.SetActive(true);
                        textitoDialogo.text = "Skibididop.";
                        yield return new WaitForSeconds(1.75f);
                        textitoDialogo.text = " ";
                        yield return new WaitForSeconds(0.25f);
                        textitoDialogo.text = "Dop.";
                        yield return new WaitForSeconds(1.75f);
                        textitoDialogo.text = " ";
                        yield return new WaitForSeconds(0.25f);
                        textitoDialogo.text = "Dop.";
                        yield return new WaitForSeconds(2);
                        bocadillo.SetActive(false);
                        audioSource.Stop();
                        StartCoroutine(SpawnDocument());
                        contadorPersonas++;
                        break;
                    case 5:
                        yield return new WaitForSeconds(1);
                        audioSource.Play();
                        bocadillo.SetActive(true);
                        textitoDialogo.text = "Skibididop.";
                        yield return new WaitForSeconds(1.75f);
                        textitoDialogo.text = " ";
                        yield return new WaitForSeconds(0.25f);
                        textitoDialogo.text = "Dop.";
                        yield return new WaitForSeconds(1.75f);
                        textitoDialogo.text = " ";
                        yield return new WaitForSeconds(0.25f);
                        textitoDialogo.text = "Dop.";
                        yield return new WaitForSeconds(2);
                        bocadillo.SetActive(false);
                        audioSource.Stop();
                        StartCoroutine(SpawnDocument());
                        contadorPersonas++;
                        break;
                }
            }
        }
        if (escenita == "JueguesitoDia5")
        {
            if (GameObject.FindGameObjectsWithTag("Personaje").Length != 0)
            {
                switch (contadorPersonas)
                {
                    case 0:
                        yield return new WaitForSeconds(15);
                        audioSource.Play();
                        bocadillo.SetActive(true);
                        textitoDialogo.text = "Skibididop.";
                        yield return new WaitForSeconds(1.75f);
                        textitoDialogo.text = " ";
                        yield return new WaitForSeconds(0.25f);
                        textitoDialogo.text = "Dop.";
                        yield return new WaitForSeconds(1.75f);
                        textitoDialogo.text = " ";
                        yield return new WaitForSeconds(0.25f);
                        textitoDialogo.text = "Dop.";
                        yield return new WaitForSeconds(2);
                        bocadillo.SetActive(false);
                        audioSource.Stop();
                        StartCoroutine(SpawnDocument());
                        contadorPersonas++;
                        break;
                    case 1:
                        yield return new WaitForSeconds(1);
                        audioSource.Play();
                        bocadillo.SetActive(true);
                        textitoDialogo.text = "Skibididop.";
                        yield return new WaitForSeconds(1.75f);
                        textitoDialogo.text = " ";
                        yield return new WaitForSeconds(0.25f);
                        textitoDialogo.text = "Dop.";
                        yield return new WaitForSeconds(1.75f);
                        textitoDialogo.text = " ";
                        yield return new WaitForSeconds(0.25f);
                        textitoDialogo.text = "Dop.";
                        yield return new WaitForSeconds(2);
                        bocadillo.SetActive(false);
                        audioSource.Stop();
                        StartCoroutine(SpawnDocument());
                        contadorPersonas++;
                        break;
                    case 2:
                        yield return new WaitForSeconds(1);
                        audioSource.Play();
                        bocadillo.SetActive(true);
                        textitoDialogo.text = "Skibididop.";
                        yield return new WaitForSeconds(1.75f);
                        textitoDialogo.text = " ";
                        yield return new WaitForSeconds(0.25f);
                        textitoDialogo.text = "Dop.";
                        yield return new WaitForSeconds(1.75f);
                        textitoDialogo.text = " ";
                        yield return new WaitForSeconds(0.25f);
                        textitoDialogo.text = "Dop.";
                        yield return new WaitForSeconds(2);
                        bocadillo.SetActive(false);
                        audioSource.Stop();
                        StartCoroutine(SpawnDocument());
                        contadorPersonas++;
                        break;
                    case 3:
                        yield return new WaitForSeconds(1);
                        audioSource.Play();
                        bocadillo.SetActive(true);
                        textitoDialogo.text = "Skibididop.";
                        yield return new WaitForSeconds(1.75f);
                        textitoDialogo.text = " ";
                        yield return new WaitForSeconds(0.25f);
                        textitoDialogo.text = "Dop.";
                        yield return new WaitForSeconds(1.75f);
                        textitoDialogo.text = " ";
                        yield return new WaitForSeconds(0.25f);
                        textitoDialogo.text = "Dop.";
                        yield return new WaitForSeconds(2);
                        bocadillo.SetActive(false);
                        audioSource.Stop();
                        StartCoroutine(SpawnDocument());
                        contadorPersonas++;
                        break;
                    case 4:
                        yield return new WaitForSeconds(1);
                        audioSource.Play();
                        bocadillo.SetActive(true);
                        textitoDialogo.text = "Skibididop.";
                        yield return new WaitForSeconds(1.75f);
                        textitoDialogo.text = " ";
                        yield return new WaitForSeconds(0.25f);
                        textitoDialogo.text = "Dop.";
                        yield return new WaitForSeconds(1.75f);
                        textitoDialogo.text = " ";
                        yield return new WaitForSeconds(0.25f);
                        textitoDialogo.text = "Dop.";
                        yield return new WaitForSeconds(2);
                        bocadillo.SetActive(false);
                        audioSource.Stop();
                        StartCoroutine(SpawnDocument());
                        contadorPersonas++;
                        break;
                    case 5:
                        yield return new WaitForSeconds(1);
                        audioSource.Play();
                        bocadillo.SetActive(true);
                        textitoDialogo.text = "Skibididop.";
                        yield return new WaitForSeconds(1.75f);
                        textitoDialogo.text = " ";
                        yield return new WaitForSeconds(0.25f);
                        textitoDialogo.text = "Dop.";
                        yield return new WaitForSeconds(1.75f);
                        textitoDialogo.text = " ";
                        yield return new WaitForSeconds(0.25f);
                        textitoDialogo.text = "Dop.";
                        yield return new WaitForSeconds(2);
                        bocadillo.SetActive(false);
                        audioSource.Stop();
                        StartCoroutine(SpawnDocument());
                        contadorPersonas++;
                        break;
                }
            }
        }
    }
}
