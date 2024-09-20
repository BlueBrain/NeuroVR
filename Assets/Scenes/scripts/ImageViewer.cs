using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using System.Threading.Tasks;


public class ImageViewer : MonoBehaviour
{

    public GameObject mainViewer;
    public GameObject prevViewer;
    public GameObject nextViewer;

    
    private List<string> imagesURLs = new List<string>();
    private List<bool> imagesLoaded = new List<bool>();
    private List<Texture2D> images = new List<Texture2D>();
    
    public int offset = 0;
    private int currentImg = 0;
    private int l = 0;

    // Start is called before the first frame update
    void Start()
    {
        loadURLs();
        loadURLs("png");
        l = imagesURLs.Count;
        Debug.Log($"Number of images: {l}");
        for ( int i = 0; i < l; ++i)
        {
            imagesLoaded.Add(false);
            images.Add(new Texture2D(2,2)); 
        }

        currentImg = clamp(offset);
        setImage(mainViewer, currentImg);       
        setImage(prevViewer, currentImg-1);     
        setImage(nextViewer, currentImg+1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void loadImage(int index)
    {
        byte[] bytes = File.ReadAllBytes(imagesURLs[index]);
        images[index].LoadImage(bytes);
        // images[index].Compress(true);
        imagesLoaded[index] = true;
    }
    private void unloadImage(int index)
    {
    
        Object.Destroy (images[index]);
        images[index] = new Texture2D(2,2);
        imagesLoaded[index] = false;
    }

    private void loadURLs(string extension = "jpg")
    {
        var path = Application.persistentDataPath + Path.DirectorySeparatorChar;
        var paths = Directory.EnumerateFiles(path, $"*.{extension}");
        foreach (var p in paths)
            imagesURLs.Add(p);
    }

    private int clamp(int id)
    {
        int newId = id;
         if (newId < 0)
            newId += l;
         if (newId > l - 1)
            newId -= l;
        return newId;
    }

    private void setImage(GameObject viewer, int id)
    {
        id = clamp(id);
        if (!imagesLoaded[id])
            loadImage(id);

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
        
        int uIndex = clamp(currentImg-2);
        unloadImage(uIndex);
    }

    public void previousImage()
    {
        --currentImg;
        currentImg = clamp(currentImg);
        setImage(mainViewer, currentImg);        
        setImage(prevViewer, currentImg-1);     
        setImage(nextViewer, currentImg+1);
        
        int uIndex = clamp(currentImg+2);
        unloadImage(uIndex);
    }

}
