using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardView : MonoBehaviour
{
    public static Vector2 proportions = new Vector2(2, 3);
    Image image;

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        if (image != null)
        {
            image.color = Random.ColorHSV();
        }
    }
}
