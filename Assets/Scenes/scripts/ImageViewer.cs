/* Copyright (c) 2024, EPFL/Blue Brain Project
 * All rights reserved. Do not distribute without permission.
 * Responsible author: Juan Jose Garcia <juanjose.garcia@epfl.ch>
 * This file is part of NeuroVR <https://github.com/BlueBrain/NeuroVR>
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.Networking;
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
    public async void Start()
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
        await setImage(mainViewer, currentImg);       
        await setImage(nextViewer, currentImg+1);
        await setImage(prevViewer, currentImg-1);     
    }

    // Update is called once per frame
    public async void Update()
    {
        await setImage(mainViewer, currentImg);
        await setImage(prevViewer, currentImg-1);     
        await setImage(nextViewer, currentImg+1);
        
        int uIndex = clamp(currentImg-2);
        unloadImage(uIndex);
        uIndex = clamp(currentImg+2);
        unloadImage(uIndex);
    }

    private async Task loadImage(int index)
    {
        byte[] bytes = await File.ReadAllBytesAsync(imagesURLs[index]);
        images[index].LoadImage(bytes);
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

    private async Task setImage(GameObject viewer, int id)
    {
        id = clamp(id);
        if (!imagesLoaded[id])
            await loadImage(id);

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
        return;
    }

    public void nextImage()
    {
        ++currentImg;
        currentImg = clamp(currentImg);
     
    }

    public void previousImage()
    {
        --currentImg;
        currentImg = clamp(currentImg);
    }

}
