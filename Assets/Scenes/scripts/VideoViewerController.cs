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
