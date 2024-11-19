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
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Assertions;
using UnityEngine.Serialization;
using Oculus.Interaction;



public class brainController : MonoBehaviour
{
    private IInteractableView interactableView;

    public float rotationSpeed = 1.0f;
    public int sceneIndex = 0;
    public bool loadAsync = false;
    public Light light;
    public float normalIntensity = 5;
    public float highIntensity = 10;

    // Start is called before the first frame update
    void Start()
    {
    

        // #if !UNITY_STANDALONE_WIN
        // Debug.Log("not windows");
        // if (windowsOnly)
        //     selectable = false;
        // #endif

        // foreach ( Transform child in transform)
        // {
        //     if (!selectable)
        //         child.gameObject.GetComponent<Renderer>().material = noSelectMat;
        //     else 
        //     {
        //         child.gameObject.GetComponent<Renderer>().material = mat;
        //         // Material mat = child.gameObject.GetComponent<Renderer>().material;
        //         // Color color = _mat.GetColor("_BaseColor");
        //         // color.a = baseAlpha;
        //         // mat.SetColor("_BaseColor", color);
        //     }
        // }     
        // if (loadAsync)
        //     SceneManager.LoadScene(sceneIndex,  LoadSceneMode.Additive);

    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, Time.deltaTime * rotationSpeed, 0, Space.Self);
       
        // if (selectable)
        // {   
        
        if (interactableView == null)
            interactableView = (IInteractableView)gameObject.GetComponent(typeof(IInteractableView));
            
        foreach (Transform child in transform)
        {
            // Material mat = child.gameObject.GetComponent<Renderer>().material;
            // Color color = mat.GetColor("_BaseColor");
            switch (interactableView.State)
            {
                case InteractableState.Normal:
                    light.GetComponent<Light>().intensity = normalIntensity;
                    // child.gameObject.GetComponent<Renderer>().material = mat;
                    break;
                case InteractableState.Hover:     
                    light.GetComponent<Light>().intensity = highIntensity;       
                    // child.gameObject.GetComponent<Renderer>().material = selectMat;
                    break;
                case InteractableState.Select:
                    if (!loadAsync)
                        SceneManager.LoadScene(sceneIndex);
                    else 
                        SceneManager.LoadSceneAsync(sceneIndex);
                    break;
            }
        }
        // }
    }

    // IEnumerator LoadScene()
    // {
    //     //Begin to load the Scene you specify
    //     AsyncOperation asyncOperation = 
    //     //Don't let the Scene activate until you allow it to
    //     // asyncOperation.allowSceneActivation = false;
    //     // Debug.Log("Pro :" + asyncOperation.progress);
    //     //When the load is still in progress, output the Text and progress bar
    //     while (!asyncOperation.isDone)
    //     {
    //         //Output the current progress
    //         // m_Text.text = "Loading progress: " + (asyncOperation.progress * 100) + "%";

    //         // Check if the load has finished
    //         // if (asyncOperation.progress >= 0.9f)
    //         // {
    //             //Change the Text to show the Scene is ready
    //             // m_Text.text = "Press the space bar to continue";
    //             //Wait to you press the space key to activate the Scene
    //             // if (Input.GetKeyDown(KeyCode.Space))
    //                 //Activate the Scene
    //         yield return null;
    //     }
    //     // asyncOperation.allowSceneActivation = true;
    // }

  
}
