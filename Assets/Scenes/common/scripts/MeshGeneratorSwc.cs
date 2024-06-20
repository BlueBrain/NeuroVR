using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;


public class MeshGeneratorSwc : MonoBehaviour
{

    // // public Material neuritesMaterial;
    // // public Material somaMaterial;

    // // public int numSegments = 3;
    // // public float scaleFactor = 0.1f;

    // // public float scaleInc = 0.001f;
    // // public float displaceInc = 0.1f;

    
    // // public Text display_Text;



    // // void addNeuron(string name, Nodes nodes, Vector3 origin)
    // // {
    // //     GameObject newObject = new GameObject(name);
    // //     newObject.transform.parent = transform;
    // //     newObject.transform.localPosition = origin;
    // //     Neuron neuron = newObject.AddComponent<Neuron>();
    // //     neuron.nodes = nodes;
    // //     neuron.neuritesMaterial = neuritesMaterial;
    // //     neuron.somaMaterial = somaMaterial;
    // //     neuron.numSegments = numSegments;

    // // }

    // // Start is called before the first frame update
    // void Start()
    // {
    //     var pathDevice = Application.persistentDataPath;
    //     var fullPath = Path.GetFullPath(pathDevice);
       
    //     display_Text.text = fullPath + "\n"; 
    
        
    //     var jsonFiles = System.IO.Directory.EnumerateFiles(fullPath, "*.json");

    //     // foreach (var file in jsonFiles)
    //     // {
    //     var file = jsonFiles.ToList()[0];
    //         (var neurons, var morphos) = CircuitLoader.loadCircuit(file);
    //         foreach(var neuron in neurons)
    //         {
    //             Debug.Log(neuron.id + "   " + neuron.pos);
    //             addNeuron("neuron_"+neuron.id, morphos[neuron.morphologyId], neuron.pos);
    //         }
    //     // }
    //     scale(scaleFactor);
    // }

    // void Update()
    // {

    //     Vector2 s = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);
    //     float tri = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger);
    //     var rot = OVRInput.GetLocalControllerRotation(OVRInput.GetActiveController());
    //     if (tri > 0.5){
    //         if (s.y>0.5f)
    //             scale(scale() * (1 + scaleInc));
    //         else if (s.y<-0.5f)
    //             scale(scale() * (1 - scaleInc));
    //     }
    //     else
    //     {
    //         var dir = rot * new Vector3(-s.x, 0, -s.y);

    //         displace(dir);
    //     }
    // }

    // public void scale(float scale)
    // {
    //     scale = Mathf.Min(scale, scaleFactor);
    //     transform.localScale = new Vector3(scale, scale, scale);
    // }

    // public float scale()
    // {
    //     return transform.localScale.x;
    // }

    // public void displace(Vector3 displace)
    // {
    //     transform.localPosition += displace * displaceInc;
    // }
}
