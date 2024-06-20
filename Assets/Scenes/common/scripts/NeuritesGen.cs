using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;


public class NeuritesGen : MonoBehaviour
{

    public Material material;

    public int numSegments = 3;
    public string path;
    public float scale = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        var pathDevice = Application.persistentDataPath;
        var fullPath = Path.GetFullPath(pathDevice);
       
        Neurites neurites= TraceLoader.loadTrace(path, scale);
        neurites.process();

        GameObject neuritesObj = new GameObject("neurites");
        neuritesObj.transform.parent = gameObject.transform;
        neuritesObj.transform.localPosition = new Vector3(0,0,0);
        neuritesObj.transform.localScale = Vector3.one;

        MeshFilter meshFilter = neuritesObj.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = neuritesObj.AddComponent<MeshRenderer>();
        meshRenderer.material = material;
        meshFilter.mesh = neurites.mesh(numSegments);
    }

    void Update()
    {

    }

}
