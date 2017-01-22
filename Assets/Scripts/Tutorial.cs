using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{

    TextMesh text;
    public float fadeDuration = 2.0f;
    public float speed = 2.0f;

    void Start()
    {
        text = GetComponent<TextMesh>();
        StartCoroutine(Fade());
    }

    public IEnumerator Fade()
    {
        float fadeSpeed = (float)1.0 / fadeDuration;
        Color c = text.color;

        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime * fadeSpeed)
        {
            c.a = Mathf.Lerp(1, 0, t);
            text.color = c;
            yield return true;
        }

        Destroy(gameObject);
    }

    void Update()
    {
        //transform.Translate(new Vector3(0, -1, 0) * Time.deltaTime * speed);
    }
}
