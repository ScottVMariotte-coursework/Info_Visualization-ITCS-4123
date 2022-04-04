using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;
using UnityEngine;


public class RandomData : MonoBehaviour
{
    public GameObject BarPrefab;
    public string filename;

    public int col = 5;
    private int[] Times = new int[25];

    private void Start()
    {
        readData();
        GenerateBars();
    }
    void readData()
    {

        string[] reader = System.IO.File.ReadAllLines(filename);
        for (int i = 1; i < reader.Length; i ++){
            string[] line = Regex.Split(reader[i], ",(?=(?:[^']*'[^']*')*[^']*$)");
            //string[] line = reader[i].Split(',');
            string time = line[col];
            if(time.Split(':')[0] != "NONE"){
                int index = int.Parse(time.Split(':')[0]);
                Times[index] += 1;
            }
            
        }
    }

    void GenerateBars()
    {
        
        for (int i = 0; i < Times.Length; i++)
        {
            // for Bar prefeb
            Vector3 tarPos = new Vector3(2 * i - Times.Length, 0, 0);
            // for Cylinder prefeb
            //Vector3 tarPos = new Vector3(2 * i - number, data[i], 0);
            var p = Instantiate(BarPrefab, tarPos, Quaternion.identity);
            p.transform.localScale = new Vector3(1,
                                                 p.transform.localScale.y * Times[i] / 1000,
                                                 1);
        }
    }
}
