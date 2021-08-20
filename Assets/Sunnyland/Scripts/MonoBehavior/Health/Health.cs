using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public Sprite[] sprites = new Sprite[7];
    Image image;
    int i = 0;
    private void Start()
    {
        image = GetComponent<Image>();
        
        InvokeRepeating("animationCherry", 0.0f, 0.1f);
    }
    
    void animationCherry()
    {
        
            image.sprite = sprites[i];
            i++;
            if (i > 6)
                i = 0;
            //yield return new WaitForSeconds(0.1f);
        
        
    }
}
