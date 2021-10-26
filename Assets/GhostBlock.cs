using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostBlock : MonoBehaviour
{

    public BoxCollider2D[] collider2D;
    public SpriteRenderer spriteRenderer;
    public float flickerSpeed;

    public bool Fade;

    bool Disabled = false;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        collider2D = GetComponents<BoxCollider2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Disabled)
        {
            return;
        }

        if (spriteRenderer.color.a <= 0.5 && Fade != true)
        {
            Fade = true;
        }else if(spriteRenderer.color.a >= 1 && Fade != false )
        {
            Fade = false;
        }

        if (Fade)
        {
            spriteRenderer.color += new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, (Mathf.Sin(flickerSpeed * Time.fixedDeltaTime)));
        }
        else
        {
            spriteRenderer.color += new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, -(Mathf.Sin(flickerSpeed * Time.fixedDeltaTime)));

        }


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        StartCoroutine(GhostBlock());
        IEnumerator GhostBlock()
        {
            yield return new WaitForSecondsRealtime(0.15f);

            Disabled = true;
            foreach (BoxCollider2D c in collider2D)
            {
                c.enabled = false;
            }
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0.25f);

            yield return new WaitForSecondsRealtime(3);

            foreach (BoxCollider2D c in collider2D)
            {
                c.enabled = true;
            }
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1f);
            Disabled = false;



        }
    }
}
