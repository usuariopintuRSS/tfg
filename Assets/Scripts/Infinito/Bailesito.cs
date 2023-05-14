using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bailesito : MonoBehaviour
{
    [SerializeField] float moveSpeed=10f;
    Rigidbody2D rb;

    public bool enter;
    public bool exitLeft;
    public bool exitRight;
    public bool parao;

    [SerializeField] Button b1;
    [SerializeField] Button b2;

    void Start()
    {
        rb=GameObject.FindGameObjectWithTag("Personaje").AddComponent<Rigidbody2D>();
        rb.isKinematic=true;
        enter=true;
        exitLeft=false;
        exitRight=false;
        parao=false;
        b1=GameObject.FindGameObjectsWithTag("Botonsito")[0].GetComponent<Button>();
        b2=GameObject.FindGameObjectsWithTag("Botonsito")[1].GetComponent<Button>();
        b1.onClick.AddListener(Left);
        b2.onClick.AddListener(Right);
    }

    void Update()
    {
        if(enter)
        {
            parao=false;
            rb.velocity=new Vector2(moveSpeed, 0f);
            if(transform.position.x>=-4.5)
            {
                enter=false;
                rb.velocity=new Vector2(0f,0f);
                parao=true;
            }
        }
        if(exitLeft)
        {
            rb.velocity=new Vector2(-moveSpeed, -0f);
            if(transform.position.x<=-10) Destroy(this.gameObject);
        }
        if(exitRight)
        {
            rb.velocity=new Vector2(moveSpeed, -0f);
            if(transform.position.x>=2) Destroy(this.gameObject);
        }
    }

    //PAL BOTONSITO DE LA ISKIERDA
    public void Left()
    {
        if(parao&&GameManager.isOpen)
        {
            parao=false;
            exitLeft=true;
        }
    }

    //PAL BOTONSITO DE LA DERECHA
    public void Right()
    {
        if(parao&&GameManager.isOpen)
        {
            parao=false;
            exitRight=true;
        }
    }
}
