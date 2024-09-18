using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
public class ImageViewer : MonoBehaviour
{

    public GameObject mainViewer;
    public GameObject prevViewer;
    public GameObject nextViewer;

    
    private List<Texture> images = new List<Texture>();
    private List<VideoPlayer> players = new List<VideoPlayer>();
    public int offset = 0;
    private int currentImg = 0;
    private int l = 0;

    // Start is called before the first frame update
    void Start()
    {
        loadImages();

        l = images.Count;
        currentImg = clamp(offset);
        setImage(mainViewer, currentImg);       
        setImage(prevViewer, currentImg-1);     
        setImage(nextViewer, currentImg+1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void loadImages(string extension = "jpg")
    {
        var path = Application.persistentDataPath + Path.DirectorySeparatorChar;
        var paths = Directory.EnumerateFiles(path, $"*.{extension}");
        foreach (var p in paths)
        {
            byte[] pngBytes = File.ReadAllBytes(p);
            Texture2D tex = new Texture2D(2,2);
            tex.LoadImage(pngBytes);
            images.Add(tex);
        }
    }

    private int clamp(int id)
    {
        int newId = id;
         if (newId < 0)
            newId = l - 1;
         if (newId > l - 1)
            newId = 0;
        return newId;
    }

    void setImage(GameObject viewer, int id)
    {
        id = clamp(id);

        RawImage rawImage = viewer.GetComponent<RawImage>();
        RectTransform rec = viewer.GetComponent<RectTransform>();

        rawImage.texture = images[id];    
        
        float ratio = (float)images[id].width / images[id].height; 

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
        currentImg = clamp(currentImg);
        setImage(mainViewer, currentImg);
        setImage(prevViewer, currentImg-1);     
        setImage(nextViewer, currentImg+1);
    }

    public void previousImage()
    {
        --currentImg;
        currentImg = clamp(currentImg);
        setImage(mainViewer, currentImg);        
        setImage(prevViewer, currentImg-1);     
        setImage(nextViewer, currentImg+1);
    }

}
