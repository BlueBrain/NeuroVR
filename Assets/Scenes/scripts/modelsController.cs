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
