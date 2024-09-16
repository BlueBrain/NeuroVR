using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageViewer : MonoBehaviour
{

    public Texture[] images;
    private int currentImg = 0;


    // Start is called before the first frame update
    void Start()
    {
       setImage();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void setImage()
    {
        RawImage rawImage = GetComponent<RawImage>();
        RectTransform rec = GetComponent<RectTransform>();

        rawImage.texture = images[currentImg];    
        
        float ratio = (float)images[currentImg].width / images[currentImg].height; 

        float x = 1.0f;
        float y = 1.0f;
        
        if (ratio >= 1.0 )
            y = 1.0f/ratio;
        else
            x = ratio;

        rec.localScale = new Vector3(x, y, 1);
    }

    public void nextImage()
    {
        ++currentImg;
        if (currentImg > images.Length - 1)
            currentImg = 0;
        setImage();
    }

    public void previousImage()
    {
        --currentImg;
        if (currentImg < 0)
            currentImg = images.Length - 1;
        setImage();
    }

}
