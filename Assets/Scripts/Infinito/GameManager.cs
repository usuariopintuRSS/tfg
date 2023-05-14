using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;

public class GameManager : MonoBehaviour
{
    // VARIABLES Y DEMAS DEL SISTEMA ANIQUILA ENEMIGOS
    public int a=0;
    public int b=0;
    public int correctito=0;
    public int incorrectito=0;

    public Button b1;
    public Button b2;

    // VARIABLES Y DEMAS MIERDAS DEL SPAWN
    public GameObject enemyPrefab;
    public GameObject document;
    public GameObject document2;
    public GameObject document3;
 
    public float timeBetweenEnemySpawn;
    public float timeBetweenWaves;
 
    public Transform spawnPoint;

    public Transform spawnDocumentPoint;
 
    bool spawningWave;

    List<string> n;

    private GameObject h;
    private GameObject i;

    public static bool isOpen;

    private int doks;

    public bool fin;

    public float time=90;

    public TMP_Text tmp;

    void Start()
    {
        StartCoroutine(SpawnWave());
        b1.onClick.AddListener(TaBien);
        b2.onClick.AddListener(TaMal);
        fin=false;
        Lista();
    }

    void Update()
    {
        if(time>0) time-=Time.deltaTime;
        else time=0;
        // SPAWN
        if (GameObject.FindGameObjectsWithTag("Personaje").Length == 0 && !spawningWave && !fin) StartCoroutine(SpawnWave());
        if(GameObject.FindGameObjectsWithTag("Documento").Length!=0)
        {
            if(GameObject.FindGameObjectsWithTag("Documento")[0].transform.position.x>-0.75&&GameObject.FindGameObjectsWithTag("Documento")[1].transform.position.x>-0.75) isOpen=true;
            else isOpen=false;
        }
        Relojito(time);
    }

    void Relojito(float timeToDisplay)
    {
        if(time<0) time=0;
        float minutos=Mathf.FloorToInt(time/60);
        float segundos=Mathf.FloorToInt(time%60);
        tmp.text=string.Format("{0:00}:{1:00}", minutos, segundos);
    }
 
    IEnumerator SpawnWave()
    {
        spawningWave = true;
        yield return new WaitForSeconds(timeBetweenWaves);
        GameObject o=Instantiate(enemyPrefab, spawnPoint.position, enemyPrefab.transform.rotation);
        ChangeSprite(o);
        StartCoroutine(SpawnDocument());
        yield return new WaitForSeconds(timeBetweenEnemySpawn);
        spawningWave = false;
    }

    IEnumerator SpawnDocument()
    {
        yield return new WaitForSeconds(timeBetweenWaves);
        int n=Random.Range(0,2);
        if(n==0)
        {
            h=Instantiate(document, spawnDocumentPoint.position, document.transform.rotation);
            i=Instantiate(document2, spawnDocumentPoint.position, document2.transform.rotation);
            doks=0;
        }
        else
        {
            h=Instantiate(document3, spawnDocumentPoint.position, document.transform.rotation);
            i=Instantiate(document2, spawnDocumentPoint.position, document2.transform.rotation);
            doks=1;
        }
        Nombresitos();
        yield return new WaitForSeconds(timeBetweenEnemySpawn);
    }

    public void TaBien()
    {
        if(GameObject.FindGameObjectWithTag("Personaje")!=null&&isOpen)
        {
            GameObject[] docs=GameObject.FindGameObjectsWithTag("Documento");
            foreach(GameObject doc in docs) GameObject.Destroy(doc);
            a++;
            Debug.Log("le has dado TaBien a: " + a);
            //
            if(h.transform.GetComponentInChildren<TMP_Text>().text == i.transform.GetComponentInChildren<TMP_Text>().text) correctito++;
            else incorrectito ++;
            CaPasao();
        }
        
    }

    public void CaPasao(){
        Debug.Log("correctitas: " + correctito);
        Debug.Log("incorrectitas: " + incorrectito);
        //if(incorrectito==1) fin=true;
    }

    public void TaMal()
    {
        if(GameObject.FindGameObjectWithTag("Personaje")!=null&&isOpen)
        {
            GameObject[] docs=GameObject.FindGameObjectsWithTag("Documento");
            foreach(GameObject doc in docs) GameObject.Destroy(doc);
            b++;
            Debug.Log("le has dado TaMal a: " + b);
            if(h.transform.GetComponentInChildren<TMP_Text>().text != i.transform.GetComponentInChildren<TMP_Text>().text) correctito++;
            else incorrectito++;
            CaPasao();
        }
        
    }

    public Sprite[] sprites;

    public void ChangeSprite(GameObject o)
    {
        SpriteRenderer sr=o.transform.GetComponentInChildren<SpriteRenderer>();
        int r=Random.Range(1,sprites.Length+1);
        sr.sprite=sprites[r-1];
    }

    public void Nombresitos()
    {
        int ra=Random.Range(0,2);
        int n1=Random.Range(0,1001);
        int n2;
        if(ra==0) n2=Random.Range(0,1001);
        else n2=n1;
        if(doks==0)
        {
            h.transform.GetComponentInChildren<TMP_Text>().text=datosgatos[n1][1];
            i.transform.GetComponentInChildren<TMP_Text>().text=datosgatos[n2][1];
        }
        else if(doks==1)
        {
            h.transform.GetComponentsInChildren<TMP_Text>()[0].text=datosgatos[n1][1] + " " +datosgatos[n1][2];
            h.transform.GetComponentsInChildren<TMP_Text>()[1].text=datosgatos[n1][3];
            h.transform.GetComponentsInChildren<TMP_Text>()[2].text=datosgatos[n1][4] + " " +datosgatos[n1][5];
            i.transform.GetComponentInChildren<TMP_Text>().text=datosgatos[n2][1];
        }
        
    }

    List<string[]> datosgatos;

    public void Lista(){
        string nombre="Assets\\Game\\Nombres\\myFile0.csv";
        datosgatos=new List<string[]>();
        using (StreamReader reader = new StreamReader(nombre))  
        {  
            string line;  
            while ((line = reader.ReadLine()) != null)  
            {  
                datosgatos.Add(line.Split(','));
            }
        }
    }
}
