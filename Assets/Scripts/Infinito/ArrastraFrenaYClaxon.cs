using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrastraFrenaYClaxon : MonoBehaviour
{
    Vector3 mousePositionOffset;

    bool gravedad = false;

    Rigidbody2D rb;

    private Vector3 GetMouseWorldPosition()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void OnMouseDown()
    {
        mousePositionOffset = gameObject.transform.position - GetMouseWorldPosition();
    }

    private void OnMouseDrag()
    {
        transform.position = GetMouseWorldPosition() + mousePositionOffset;
    }

    private void Update()
    {
        if (transform.position.x <= -0.75)
        {
            this.gameObject.transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
            this.gameObject.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
        }
        else
        {
            this.gameObject.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
            this.gameObject.transform.GetChild(0).GetChild(1).gameObject.SetActive(true);
        }
        if (transform.position.x <= -0.75 && transform.position.y <= -1.5)
        {
            Destroy(rb);
            gravedad = false;
        }
        if (gravedad == true)
        {
            rb.velocity = new Vector2(0f, -Time.deltaTime * 5000);
        }
    }

    private void OnMouseUp()
    {
        if (transform.position.x <= -0.75 && transform.position.y > -1.5)
            Flota();
    }

    private void Flota()
    {
        rb = this.gameObject.AddComponent<Rigidbody2D>();
        rb.isKinematic = true;
        gravedad = true;
    }
}
