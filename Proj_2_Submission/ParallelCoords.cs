using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ParallelCoords : MonoBehaviour
{
    public string file1;
    public string file2;

    private List<GameObject> lineObjs = new List<GameObject>();
    private List<Vector3[]> lines = new List<Vector3[]>();
    private List<string> FIPS = new List<string>();
    // Start is called before the first frame update
    void Start()
    {
        readData();
        Debug.Log("Translateing");
        TranslateData();

        float angleY = getAngle(-45);
        float angleX = getAngle(-30);
        float[] rotateY = {Mathf.Cos(angleY),0,Mathf.Sin(angleY),0,
                             0,1,0,0,
                             -Mathf.Sin(angleY),0,Mathf.Cos(angleY),0,
                             0,0,0,0};
        
        

        float[] rotateX = {1,0,0,0,
                             0,Mathf.Cos(angleX),-Mathf.Sin(angleX),0,
                             0,Mathf.Sin(angleX),Mathf.Cos(angleX),0,
                             0,0,0,1};
                             
        
        TranslateData(rotateX);
        TranslateData(rotateY);

        
        float[] translate = {1,0,0,85,
                            0,1,0,-44,
                            0,0,1,151,
                             0,0,0,1};
        TranslateData(translate);
        Debug.Log("updating");
        updateLines(lines);
    }

    float getAngle(int degree){
        return degree/57.29578f;
    }

    // Update is called once per frame
    void Update()
    {
    }

    float[] matMult(float[] vec, float[] mat, int size, int[] dems){
        float[] newVec = new float[size];
        for(int y = 0; y < dems[1]; y ++){
            float sum = 0;
            for(int x = 0; x < dems[0]; x ++){
                int offset = y*dems[0];
                sum += vec[x] * mat[x + offset];
            }
            
            newVec[y] = sum;
        }
        
        return newVec;
    }
    
    //TRANSLATE
    void TranslateData(float[] translate){
        for(int i = 0; i < lines.Count; i ++){
            Vector3[] line = lines[i];

            for(int v = 0; v < line.Length; v ++){
                Vector3 vector = line[v];
                float[] vec = {vector[0],vector[1],vector[2],1};
                int[] dem = {4,4};
                float[] newVert = matMult(vec,translate,4,dem);
                line[v] = new Vector3(newVert[0],newVert[1],newVert[2]);
            }
            lines[i] = line;
         }   
    }

    //TRANSLATE
    void TranslateData(){
        for(int i = 0; i < lines.Count; i ++){
            Vector3[] line = lines[i];

            string fips = FIPS[i];
            int translateX = int.Parse(fips.Substring(0,1));
            translateX *= line.Length;
            float[] translate = {1,0,0,translateX*10,
                                    0,1,0,0,
                                    0,0,1,0,
                                    0,0,0,1};
            for(int v = 0; v < line.Length; v ++){
                Vector3 vector = line[v];
                float[] vec = {vector[0],vector[1],vector[2],1};
                int[] dem = {4,4};
                float[] newVert = matMult(vec,translate,4,dem);
                //Debug.Log("Old" + vec[0]);
                //Debug.Log("New" + newVert[0]);
                line[v] = new Vector3(newVert[0],newVert[1],newVert[2]);
            }
            lines[i] = line;
         }   
    }

    void readData()
    {
        string[] reader1 = System.IO.File.ReadAllLines(file1);
        string[] reader2 = System.IO.File.ReadAllLines(file2);
        
        for (int i = 0; i < reader1.Length/3; i ++){
            string[] line1 = reader1[i].Split(',');
            string[] line2 = reader2[i].Split(',');

            string fips = line1[0];
            FIPS.Add(fips);
            //SCALEING
            Vector3[] verticies = new Vector3[line1.Length - 2];
            for (int v = 1; v < line1.Length; v ++){
                if(line1[v] != "" && line2[v] != ""){
                    float[] scale = {10,0,0,0,
                                      0,100,0,0,
                                      0,0,500,0,
                                      0,0,0,1
                    };
                   float[] vec = {v + (line1.Length/-2), float.Parse(line1[v]), float.Parse(line2[v]),1}; 
                    int[] dem = {4,4};
                   vec = matMult(vec,scale,4,dem);
                   verticies[v-1] = new Vector3(vec[0],vec[1],vec[2]);
                }
            }
            float r = (float.Parse(fips.Substring(1,2))/255.0f);
            float g = (float.Parse(fips.Substring(0,1))/255.0f) * 30.0f;
            float b = (float.Parse(fips.Substring(2,3))/255.0f);
            r=0;
            b=0;
            lines.Add(verticies);
            makeLine(verticies, new Color(r,g,b), "line:" + (i+1).ToString());
        }
    }

    void updateLines(List<Vector3[]> lines){
        for(int i = 0; i < lines.Count; i ++){
            LineRenderer render = lineObjs[i].GetComponent(typeof(LineRenderer)) as LineRenderer;
            //Debug.Log(render.name);
            render.SetPositions(lines[i]);
        }
    }

    void makeLine(Vector3[] verticies, Color color, string name){
        GameObject lineObj = new GameObject(name);
        LineRenderer render = lineObj.AddComponent(typeof(LineRenderer)) as LineRenderer;
        render.material = new Material(Shader.Find("Sprites/Default"));
        render.startWidth = 0.05f;
        render.endWidth = 0.05f;
        render.positionCount  = verticies.Length;
        render.startColor = color;
        render.endColor = color;
        render.SetPositions(verticies);

        lineObjs.Add(lineObj);
    }
}
