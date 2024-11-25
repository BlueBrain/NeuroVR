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
using System.Threading.Tasks;



public class Node
{

    public Node(Vector3 position_, float radius_)
    {
        position = position_;
        radius = radius_;
        tangent = new Vector3(0,0,0);
    }
    public Vector3 position;
    public float radius;
    public Vector3 tangent;
}

public class Section : List<Node>
{

    public void process()
    {        
        for (int i = 1; i < this.Count; ++i)
        {
            Vector3 axis = Vector3.Normalize(this[i].position - this[i-1].position);
            this[i].tangent += axis;
            this[i-1].tangent += axis;
        }
        foreach (Node node in this) 
        {
            node.tangent = Vector3.Normalize(node.tangent);
            if (node.tangent.sqrMagnitude < 0.01f)
                node.tangent = Vector3.forward;
        }
    }

    public void mesh(int numSegments, Vector3[] refVertices, Vector3 refAxis, List<Vector3> vertices, List<Vector3>normals, List<int> indices)
    {
        int off = vertices.Count;

        Vector3[] cVertices = (Vector3[])refVertices.Clone();
        Vector3 pTangent = refAxis;
        foreach (Node node in this)
        {

            Quaternion rotation = new Quaternion();
            rotation.SetFromToRotation(pTangent,node.tangent);
            rotation *= Quaternion.AngleAxis(180.0f/numSegments,pTangent);
            for (int j = 0; j < numSegments; ++j)   
            {
                cVertices[j] =  (rotation * cVertices[j]);
                vertices.Add( node.position + cVertices[j] * node.radius);
                normals.Add( cVertices[j] * node.radius);
            }
            pTangent = node.tangent;
        }   

        for ( int i = 1; i < this.Count; ++i)
        {
            int pi = off + (i - 1) * numSegments;
            int ci = off + i * numSegments;
            for (int s = 0; s < numSegments; ++s)
            {
                int v0 = pi + s;
                int v1 = pi + (s + 1) % numSegments;
                int v2 = ci + s;
                int v3 = ci + (s + 1) % numSegments;
                indices.Add(v0);
                indices.Add(v1);
                indices.Add(v2);
                indices.Add(v3);
                indices.Add(v2);
                indices.Add(v1);
            }
        }
        int startId =  vertices.Count;
        int endId = startId + 1;
        Node n = this[0];
        vertices.Add(n.position - n.tangent * n.radius);
        normals.Add(-n.tangent * n.radius);
        int vOff = off;
        for (int s = 0; s < numSegments; ++s)
        {
            int v0 = vOff + s;
            int v1 = vOff + (s + 1) % numSegments;
            indices.Add(v1);
            indices.Add(v0);
            indices.Add(startId);
        }
        n = this[this.Count - 1];   
        vertices.Add(n.position + n.tangent * n.radius);
        normals.Add(n.tangent * n.radius);
        vOff = off + (this.Count - 1) * numSegments;
        for (int s = 0; s < numSegments; ++s)
        {
            int v0 = vOff + s;
            int v1 = vOff + (s + 1) % numSegments;
            indices.Add(v0);
            indices.Add(v1);
            indices.Add(endId);
        }
    }
}


public class Neurites : List<Section>
{
    async public void process()
    {
        await Task.Run( () =>
        {
            foreach (Section section in this)
                section.process(); 
            return;
        });
    }



    async public Task<Mesh> mesh(int numSegments)
    {
        List<Vector3> vertices = new List<Vector3>();
        List<Vector3> normals = new List<Vector3>();
        List<int> indices = new List<int>();    
        await Task.Run( () =>
        {
        //  await Task.Run( () => createDendrites(neurites));

               

            Vector3[] refVertices = new Vector3[numSegments];
            float angleIncrement = 2 * Mathf.PI / numSegments;
            for (int i = 0; i < numSegments; ++i)
            {
                refVertices[i] = new Vector3(Mathf.Cos(angleIncrement * i),Mathf.Sin(angleIncrement * i),0.0f);
            }        
            Vector3 refAxis = new Vector3(0,0,1);

       
            foreach (Section section in this)
            {
                section.mesh(numSegments, refVertices, refAxis, vertices, normals, indices);
            }
        });
        
        Mesh mesh = new Mesh();
        mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        mesh.vertices = vertices.ToArray();
        mesh.normals = normals.ToArray();
        mesh.triangles = indices.ToArray();
        return mesh;
    }
}


// public class Section : List<Node>
// {

//     public void process()
//     {        
//         fore (int i = 1; i < this.Count; ++i)
//         {
//             Vector3 axis = Vector3.Normalize(this[i].position - this[i-1].position);
//             this[i].tangent += axis;
//             this[i-1].tangent += axis;
//         }
//         foreach (Node node in this) node.tangent = Vector3.Normalize(node.tangent);
//     }

//     public (List<Vector3>, List<Vector3>, List<int>) mesh(int numSegments)
//     {
//         // System.DateTime startTime = System.DateTime.UtcNow;
//         Mesh mesh = new Mesh();
//         // Create reference polygon
//         Vector3[] refVertices = new Vector3[numSegments];
//         float angleIncrement = 2 * Mathf.PI / numSegments;
//         for (int i = 0; i < numSegments; ++i)
//         {
//             refVertices[i] = new Vector3(Mathf.Cos(angleIncrement * i),Mathf.Sin(angleIncrement * i),0.0f);
//         }        
//         Vector3 refAxis = new Vector3(0,0,1);

//         List<Vector3> vertices = new List<Vector3>();
//         List<Vector3> normals = new List<Vector3>();
//         List<int> indices = new List<int>();
//         // Generate neurites vertices
//         foreach (Node node in this)
//         {
//             Vector3[] pVertices;
//             Vector3 pTangent;

//             if (node.type == Node.Type.start)
//             {
//                 pVertices = (Vector3[])refVertices.Clone();
//                 pTangent = refAxis;
//             }
//             else 
//             {
//                 Node pNode = this[node.parentId]; 
//                 pVertices = new Vector3[numSegments];
//                 for (int i = 0; i < numSegments; ++i)
//                 {
//                     pVertices[i] = (vertices[node.parentId * numSegments + i] - pNode.position) / pNode.radius;
//                 }
//                 pTangent = pNode.tangent;
//             }

//             Quaternion rotation = new Quaternion();
//             rotation.SetFromToRotation(pTangent,node.tangent);
//             for (int i = 0; i < numSegments; ++i)
//             {
//                 pVertices[i] =  rotation * pVertices[i];
//                 vertices.Add( node.position + pVertices[i] * (node.radius));
//                 normals.Add( pVertices[i] * node.radius);
//             }
//         }
//         // Generate neurites indices
//         for (int i = 0; i < this.Count; ++i)
//         {
//             int parentId = this[i].parentId;
//             if (parentId < 0)
//                 continue;
//             int ci = _nodeIndexToVertexIndex(i, numSegments);
//             int pi = _nodeIndexToVertexIndex(parentId, numSegments);

//             for (int s = 0; s < numSegments; ++s)
//             {
//                 int v0 = pi + s;
//                 int v1 = pi + (s + 1) % numSegments;
//                 int v2 = ci + s;
//                 int v3 = ci + (s + 1) % numSegments;

//                 // indices.Add(v0);
//                 // indices.Add(v1);
//                 // indices.Add(v2);
//                 // indices.Add(v3);
            
                
//                 indices.Add(v0);
//                 indices.Add(v1);
//                 indices.Add(v2);
//                 // indices.Add(v2);
                
//                 indices.Add(v3);
//                 indices.Add(v2);
//                 indices.Add(v1);
//                 // indices.Add(v1);
            
            
//             }
//         }

//         // End nodes
//         int count = this.Count * numSegments;
//         for (int i = 0; i < this.Count; ++i)
//         {
//             var node = this[i];
//             if (node.type != Node.Type.end) continue;
//             int ci = _nodeIndexToVertexIndex(i, numSegments);
//             vertices.Add(node.position + node.tangent * node.radius);
//             normals.Add(node.tangent * node.radius);
//              for (int s = 0; s < numSegments; ++s)
//             {
//                 int v0 = ci + s;
//                 int v1 = ci + (s + 1) % numSegments;
//                 indices.Add(v0);
//                 indices.Add(v1);
//                 indices.Add(count);
//                 // indices.Add(count);
//             }
//             ++count;
//         }


//         // Start nodes
//         for (int i = 0; i < this.Count; ++i)
//         {
//             var node = this[i];
//             if (node.type != Node.Type.start) continue;

//             Vector3[] pVertices = new Vector3[numSegments];
//             for (int j = 0; j < numSegments; ++j)
//             {
//                 pVertices[j] = (vertices[i * numSegments + j] - node.position) / node.radius;
//             }
//             Vector3 pTangent = node.tangent;
//             Vector3 tangent = Vector3.Normalize(node.position - _somaCenter);
//             Quaternion rotation = new Quaternion();
//             rotation.SetFromToRotation(pTangent,tangent);
//             Vector3 projPos = _somaCenter + tangent * _somaRadius * 0.8f;
//             for (int j = 0; j < numSegments; ++j)
//             {
//                 pVertices[j] = rotation * pVertices[j];
//                 vertices.Add( projPos + pVertices[j] * node.radius);
//                 normals.Add( pVertices[j] * node.radius);
//             }
//             int ci = _nodeIndexToVertexIndex(i, numSegments);
//             int pi = count;
//             for (int s = 0; s < numSegments; ++s)
//             {
//                 int v0 = pi + s;
//                 int v1 = pi + (s + 1) % numSegments;
//                 int v2 = ci + s;
//                 int v3 = ci + (s + 1) % numSegments;
              
//             //     indices.Add(v0);
//             //     indices.Add(v1);
//             //     indices.Add(v2);
//             //     indices.Add(v3);
    
//                 indices.Add(v0);
//                 indices.Add(v1);
//                 indices.Add(v2);
                
//                 indices.Add(v3);
//                 indices.Add(v2);
//                 indices.Add(v1);
         
//             }
//             count += numSegments;
//         }


//         mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
//         mesh.vertices = vertices.ToArray();
//         mesh.normals = normals.ToArray();
//         mesh.triangles = indices.ToArray();
        
//         // mesh.SetIndices(indices.ToArray(), MeshTopology.Quads, 0);

//         // float numVert = vertices.Count * 0.000001f;
//         // System.TimeSpan ts = System.DateTime.UtcNow - startTime;
//         // Debug.Log("Mesh generated with " + numVert + " millions vertices in " + ts.TotalMilliseconds.ToString() + " miliseconds."); 
//         return mesh;
//     }

//     public Mesh meshSoma()
//     {
//         Mesh mesh = new Mesh();
//         List<Vector3> vertices = new List<Vector3>();
//         List<Vector3> normals = new List<Vector3>();
//         List<int> indices = new List<int>();

//         Vector3 p = new Vector3(-1, 0, 0);
//         vertices.Add(p * _somaRadius + _somaCenter);
//         normals.Add(p * _somaRadius);
//         p = new Vector3(1, 0, 0);
//         vertices.Add(p * _somaRadius + _somaCenter);
//         normals.Add(p * _somaRadius);
//         p = new Vector3(0, -1, 0);
//         vertices.Add(p * _somaRadius + _somaCenter);
//         normals.Add(p * _somaRadius);
//         p = new Vector3(0, 1, 0);
//         vertices.Add(p * _somaRadius + _somaCenter);
//         normals.Add(p * _somaRadius);
//         p = new Vector3(0, 0, -1);
//         vertices.Add(p * _somaRadius + _somaCenter);
//         normals.Add(p * _somaRadius);
//         p = new Vector3(0, 0, 1);
//         vertices.Add(p * _somaRadius + _somaCenter);
//         normals.Add(p * _somaRadius);
//         indices.Add(0); indices.Add(3); indices.Add(4);
//         indices.Add(0); indices.Add(4); indices.Add(2);
//         indices.Add(4); indices.Add(3); indices.Add(1);
//         indices.Add(4); indices.Add(1); indices.Add(2);
//         indices.Add(1); indices.Add(3); indices.Add(5);
//         indices.Add(1); indices.Add(5); indices.Add(2);
//         indices.Add(5); indices.Add(3); indices.Add(0);
//         indices.Add(5); indices.Add(0); indices.Add(2);
//         mesh.vertices = vertices.ToArray();
//         mesh.normals = normals.ToArray();
//         mesh.triangles = indices.ToArray();
//         return mesh;
//     }

//     private int _nodeIndexToVertexIndex(int index, int numSegments)
//     {
//        return index * numSegments;
//     }
// }
