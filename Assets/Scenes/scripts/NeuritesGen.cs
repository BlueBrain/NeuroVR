using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Threading.Tasks;



public class NeuritesGen : MonoBehaviour
{

    public Material material;

    public int numSegments = 3;
    public string path;
    public float scale = 1.0f;
    public Quaternion rot = Quaternion.identity;
    public Text display_Text;
    private Mesh _mesh;


    // Start is called before the first frame update
    public async void Start()
    {
        
        // display_Text.text = fullPath;

        GameObject neuritesObj = new GameObject("neurites");
        neuritesObj.transform.parent = gameObject.transform;
        neuritesObj.transform.localPosition = new Vector3(0,0,0);
        neuritesObj.transform.localScale = Vector3.one;

        MeshFilter meshFilter = neuritesObj.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = neuritesObj.AddComponent<MeshRenderer>();
        meshRenderer.material = material;
                
        var pathDevice = Application.streamingAssetsPath;
        var fullPath = pathDevice + Path.DirectorySeparatorChar + path;

        Neurites neurites= await TraceLoader.loadTrace(fullPath, scale, rot);
        neurites.process();
        Mesh mesh = await neurites.mesh(numSegments);
        meshFilter.mesh = mesh;
    }

    void Update()
    {

    }

}
