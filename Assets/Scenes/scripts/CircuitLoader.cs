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
using System;
using System.IO;


[System.Serializable]
public class CircuitNeuron
{
    public int id;
    public int morphologyId;
    public float[] position;
    public Vector3 pos;
}

[System.Serializable]
public class CircuitNeurons
{
    public CircuitNeuron[] neurons;
}


[System.Serializable]
public class CircuitMorphologyPaths
{
    public string[] morphologies;
}

public class CircuitLoader
{
    // public static (List<CircuitNeuron>, List<Section>) loadCircuit(string path)
    // {
    //     string content = File.ReadAllText(path);
    //     string folderPath = Path.GetDirectoryName(path);

    //     CircuitMorphologyPaths morphoPaths = JsonUtility.FromJson<CircuitMorphologyPaths>(content);        
    //     List<Nodes> nodesList = new List<Nodes>();
    //     foreach (var morphoPath in morphoPaths.morphologies)
    //     {      
    //         string mp = folderPath + Path.DirectorySeparatorChar + morphoPath;
    //         nodesList.Add(loadSwc(mp));
    //     }

    //     CircuitNeurons neurons = JsonUtility.FromJson<CircuitNeurons>(content);
    //     foreach (var neuron in neurons.neurons)
    //         if (neuron.position.Length >= 3)
    //             neuron.pos = new Vector3(neuron.position[0], neuron.position[1], neuron.position[2]);

    //     return (new List<CircuitNeuron>(neurons.neurons), nodesList);
    // }


    // public static  loadSwc(string path, float scaleFactor = 1.0f)
    // {
    //     Nodes nodes = new Nodes();
    //     Dictionary<int, int> indices = new Dictionary<int, int>();
    //     int somaIndex = 1;

    //     StreamReader sr = new StreamReader(path);
    //     string line = sr.ReadLine();
       
    //     char[] whitespace = new char[] {' ', '\t'};
    //     while(line != null)
    //     {
    //         line = line.Trim();
    //         if ( !string.IsNullOrEmpty(line))
    //         if (line[0] != '#') 
    //         {
    //             string[] sl = line.Split(whitespace, System.StringSplitOptions.RemoveEmptyEntries);
    //             int id = System.Int32.Parse(sl[0]);
    //             int type = System.Int32.Parse(sl[1]);
    //             float x = float.Parse(sl[2]);
    //             float y = float.Parse(sl[3]);
    //             float z = float.Parse(sl[4]);
    //             float r = float.Parse(sl[5]);
    //             int pId = System.Int32.Parse(sl[6]);

    //             if (type == 1) somaIndex = Mathf.Max(somaIndex, id);
    //             else if (type > 1)
    //             {
    //                 if (pId > somaIndex) pId -= (somaIndex+1);
    //                 else pId = -1;
    //                 nodes.Add(new Node(new Vector3(x,y,z) * scaleFactor, r*scaleFactor, pId));
    //             }
    //         }
    
    //         line = sr.ReadLine();
    //     }
    //     sr.Close();

    //     nodes.process();
    //     return nodes;
    // }
}


