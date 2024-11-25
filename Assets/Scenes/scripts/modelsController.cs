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

public class modelsController : MonoBehaviour
{

    public float speedHigh = 1.0f;
    public float speedSlow = 0.1f;
    
    public float maxLen = 10.0f;


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

        Vector2 s = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.RTouch);
        var rot = OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTouch);
        var dir = rot * new Vector3(-s.x, 0, -s.y);
        float tri = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.RTouch);

        float inc = speedSlow;
        if (tri > 0.5){
            inc = speedHigh;
        }
        var pos = transform.localPosition + dir * inc * Time.deltaTime;

        if (pos.magnitude > maxLen)
        {
            pos = pos.normalized * maxLen;
        } 

        transform.localPosition = pos;
        
    }
}
