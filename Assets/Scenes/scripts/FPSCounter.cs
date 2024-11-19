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
using UnityEngine.UI;

public class FPSCounter : MonoBehaviour
{
    // Start is called before the first frame update

    public Text display_Text;

    int frameCounter = 0;
    float timeCounter = 0.0f;
   
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        ++frameCounter;
        timeCounter +=  Time.deltaTime;

        if (timeCounter >= 1.0f)
        {
            int count = (int)(frameCounter / timeCounter);
            display_Text.text = count.ToString() + " fps";
            frameCounter = 0;
            timeCounter = 0.0f;
        }



    }
}
