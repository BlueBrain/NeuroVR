using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;


public class TraceLoader
{

    public static Neurites loadTrace(string path, float scale)
    {
        List<Node> nodes = new List<Node>();
        List<(int, int)> sections = new List<(int, int)>();
        StreamReader sr = new StreamReader(path);
        string line; 
       
        char[] whitespace = new char[] {' ', '\t'};
        while((line = sr.ReadLine()) != null)
        {
            line = line.Trim();
            if ( !string.IsNullOrEmpty(line))
            if (line[0] != '#') 
            {
                string[] sl = line.Split(whitespace, StringSplitOptions.RemoveEmptyEntries);
                if (sl[0] == "n")
                {
                    float x =float.Parse(sl[1]) * scale;
                    float y =float.Parse(sl[2]) * scale;
                    float z =float.Parse(sl[3]) * scale;
                    float r =float.Parse(sl[4]) * scale;
                    nodes.Add(new Node( new Vector3(x,y,z), r));
                }
                else if (sl[0] == "s")
                {
                    int id0 = Int32.Parse(sl[1]);
                    int id1 = Int32.Parse(sl[2]);
                    sections.Add((id0, id1));
                }
            }    
        }
        
        Neurites neurites = new Neurites();
        foreach ((int, int) sec in sections)
        {
            Section section = new Section();
            for (int i = sec.Item1 ; i < sec.Item2 + 1; ++i)
                section.Add(nodes[i]);        
            neurites.Add(section);
        }
        
        return neurites;
    }
 
    // public static  loadCircuit(string path)
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


