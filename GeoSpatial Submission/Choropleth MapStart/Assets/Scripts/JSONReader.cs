/* Authored by Christian Motley
 * For use in Dr. Aidong Lu's Immersive Vis Repository for her ITCS - 4123/5123 - Visualization and Visual Communication course at UNC Charlotte.
 * Using the Quick Outline asset from the Unity Asset Store with slight modifications - https://assetstore.unity.com/packages/tools/particles-effects/quick-outline-115488
 * Last Modified: October 19th, 2020 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JSONReader : MonoBehaviour
{
    [Tooltip("Drag your JSON file here.")]
    public TextAsset jsonFile;
    public string DataPath;

    Dictionary<string, float[]> states =
    new Dictionary<string, float[]>();

    Choropleth choropleth;
    Outline outline;

    void Start()
    {
        
        //tates statesInJson = JsonUtility.FromJson<States>(jsonFile.text);

        //foreach (State state in statesInJson.states)
        //{
            /* Make sure the name of the gameobject in the Hierarchy is exactly the same as the name in the JSON file.
             * If the state's name is "North Carolina" in the JSON file then the gameobject in the hierarchy must also be named North Carolina
             * otherwise there will be an NullReferenceException and all following states will not be read. */
            //choropleth = GameObject.Find(state.name).GetComponent<Choropleth>(); //Finds the gameobject with the name that matches the name in the JSON file and gets the Choropleth script.
            //choropleth.dataValue = state.number; // Changes the Chroropleth script that is attatched to the state's dataValue variable to the number from the JSON file.
            //Debug.Log("Found state: " + state.name + "\n\t  With data value: " + state.number);
        //}
        
        string[] reader1 = System.IO.File.ReadAllLines(DataPath);
        //var listOfStates = new List<string[]>()

        //1,6,7,8,9
        /*
        1,state
        5,total_population
        6,confirmed
        7,confirmed_per_100000
        8,deaths
        9,deaths_per_100000
        */
        float[] stateInfo = new float[5];
        string currentState =  reader1[0].Split(',')[1];
        for (int i = 1; i < reader1.Length; i ++){
            string[] line1 = reader1[i].Split(',');

            if(currentState != line1[1]){
                currentState = line1[1];
                states.Add(currentState, stateInfo);
                stateInfo = new float[5];
            }
            //Debug.Log(reader1[i]);


             /*
            0,total_population
            1,confirmed
            2,confirmed_per_100000
            3,deaths
            4,deaths_per_100000
            */
            stateInfo[0] += float.Parse(line1[6]);
            stateInfo[1] += float.Parse(line1[7]);
            stateInfo[2] += float.Parse(line1[8]);
            stateInfo[3] += float.Parse(line1[9]);
            stateInfo[4] += float.Parse(line1[10]);
            //Debug.Log(currentState);
            //Debug.Log("-------------------------------------------------" + currentState);
            //Debug.Log(stateInfo[0]);
        }
        updateMap(0);
    }

    void Update(){
        if (Input.GetKeyDown("0"))
        {
            updateMap(0);
        }
        if (Input.GetKeyDown("1"))
        {
            updateMap(1);
        }
        if (Input.GetKeyDown("2"))
        {
            updateMap(2);
        }
        if (Input.GetKeyDown("3"))
        {
            updateMap(3);
        }
        if (Input.GetKeyDown("4"))
        {
            updateMap(4);
        }
    }

    void updateMap(int column){
        List<string> keyList = new List<string>(states.Keys);
        float max = 0;
        float min = 99999999;

        for(int i = 0; i < keyList.Count; i ++){
            float value = states[keyList[i]][column];
            if(max < value){
                max = value;
            }
            if(min > value){
                min = value;
            }
        }
        float denom = max - min;

        for(int i = 0; i < keyList.Count; i ++){
            string key = keyList[i];
            //Debug.Log(key);
            //Debug.Log(states[key][column]);
            updateState(key, (states[key][column] - min) / denom);
        }
    }

    void updateState(string state, float number){
        choropleth = GameObject.Find(state).GetComponent<Choropleth>();
        choropleth.dataValue = number; 
    } 
}



[System.Serializable]
public class State
{
    /* Change to whatever your JSON file has the variable named. 
     * In this example it is "name": "WhatevertheStatesNameIs"
     * And "number":"whateverTheNumberIs" */
    public string name;
    public float number;
}

[System.Serializable]
public class States
{
    // Array of states (and the values) found in the JSON file.
    public State[] states;
}