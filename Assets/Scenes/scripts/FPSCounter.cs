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
