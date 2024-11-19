/* Copyright (c) 2020-2024, EPFL/Blue Brain Project
 * All rights reserved. Do not distribute without permission.
 * Responsible author: Juan Jose Garcia <juanjose.garcia@epfl.ch>
 *
 * This file is part of NeuroVR <https://github.com/BlueBrain/NeuroVR>
 *
 * This library is free software; you can redistribute it and/or modify it under
 * the terms of the GNU Lesser General Public License version 3.0 as published
 * by the Free Software Foundation.
 *
 * This library is distributed in the hope that it will be useful, but WITHOUT
 * ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS
 * FOR A PARTICULAR PURPOSE.  See the GNU Lesser General Public License for more
 * details.
 *
 * You should have received a copy of the GNU Lesser General Public License
 * along with this library; if not, write to the Free Software Foundation, Inc.,
 * 51 Franklin Street, Fifth Floor, Boston, MA 02110-1301 USA.
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
