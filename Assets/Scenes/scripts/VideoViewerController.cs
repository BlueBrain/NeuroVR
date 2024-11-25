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
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Assertions;
using UnityEngine.Serialization;
using UnityEngine.UI;

using Oculus.Interaction;



public class VideoViewerController : MonoBehaviour
{
    private IInteractableView interactableView;
    public VideoViewer videoViewer;
    public bool updateColor = true;
    public bool forward = true;

    private RawImage image;
    private InteractableState prevState = InteractableState.Normal;
    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<RawImage>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (interactableView == null)
            interactableView = (IInteractableView)gameObject.GetComponent(typeof(IInteractableView));
            
        switch (interactableView.State)
        {
            case InteractableState.Normal:
                if (updateColor)
                    image.color = Color.grey;
                break;
            case InteractableState.Hover:     
                if (updateColor)
                    image.color = Color.white;
                break;
            case InteractableState.Select:
                if (updateColor)
                    image.color = Color.white;
                if (prevState != InteractableState.Select)
                    if (forward)
                        videoViewer.nextVideo();
                    else    
                        videoViewer.previousVideo();
                break;
        }
        prevState = interactableView.State;
    }

  
}
