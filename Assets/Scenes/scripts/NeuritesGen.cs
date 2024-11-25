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
using System.Linq;
using System.Threading.Tasks;



public class NeuritesGen : MonoBehaviour
{

    public Material material;

    public int numSegments = 3;
    public string path;
    public float scale = 1.0f;
    public Quaternion rot = Quaternion.identity;
    public Text display_Text;
    private Mesh _mesh;


    // Start is called before the first frame update
    public async void Start()
    {
        
        // display_Text.text = fullPath;

        GameObject neuritesObj = new GameObject("neurites");
        neuritesObj.transform.parent = gameObject.transform;
        neuritesObj.transform.localPosition = new Vector3(0,0,0);
        neuritesObj.transform.localScale = Vector3.one;

        MeshFilter meshFilter = neuritesObj.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = neuritesObj.AddComponent<MeshRenderer>();
        meshRenderer.material = material;
                
        var pathDevice = Application.streamingAssetsPath;
        var fullPath = pathDevice + Path.DirectorySeparatorChar + path;

        Neurites neurites= await TraceLoader.loadTrace(fullPath, scale, rot);
        neurites.process();
        Mesh mesh = await neurites.mesh(numSegments);
        meshFilter.mesh = mesh;
    }

    void Update()
    {

    }

}
