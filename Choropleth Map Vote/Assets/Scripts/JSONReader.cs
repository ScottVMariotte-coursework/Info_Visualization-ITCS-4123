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

    Dictionary<string, List<(string,int)>> states =
    new Dictionary<string, List<(string,int)>>();

    Dictionary<string,GameObject> dataPrimitives = 
    new Dictionary<string, GameObject>();

    Choropleth choropleth;
    Outline outline;
    void Start()
    {
        States statesInJson = JsonUtility.FromJson<States>(jsonFile.text);

        foreach (State state in statesInJson.states)
        {
            /* Make sure the name of the gameobject in the Hierarchy is exactly the same as the name in the JSON file.
             * If the state's name is "North Carolina" in the JSON file then the gameobject in the hierarchy must also be named North Carolina
             * otherwise there will be an NullReferenceException and all following states will not be read. */
            choropleth = GameObject.Find(state.name).GetComponent<Choropleth>(); //Finds the gameobject with the name that matches the name in the JSON file and gets the Choropleth script.
            choropleth.dataValue = state.number; // Changes the Chroropleth script that is attatched to the state's dataValue variable to the number from the JSON file.
            //Debug.Log("Found state: " + state.name + "\n\t  With data value: " + state.number);

        }
        //updateMap(0);
        readData();
        createCube();
    }
    
    void Update(){
        if (Input.GetKeyDown("1"))
        {
            changeViz(1);
        }
        if (Input.GetKeyDown("2"))
        {
            changeViz(2);
        }
    }

    void readData(){
        string[] reader1 = System.IO.File.ReadAllLines("trimedData.csv");
        //year,state,party,candidatevotes,totalvotes
        List<(string,int)> stateInfo = new List<(string,int)>();
        string currentState =  reader1[0].Split(',')[1];
        string currentYear = reader1[0].Split(',')[0];
        string totalVotes = reader1[0].Split(',')[4];

        //string[] partyInfo = new string[2];
        for (int i = 1; i < reader1.Length; i ++){
            string[] line1 = reader1[i].Split(',');
            
            if(currentState != line1[1]){
                states.Add(currentState + ":" + currentYear + ":" + totalVotes, stateInfo);
                //Debug.Log(currentYear);
                currentState = line1[1];
                currentYear = line1[0];
                totalVotes = line1[4];
                stateInfo = new List<(string,int)>();
            }

            stateInfo.Add((line1[2],int.Parse(line1[3])));
        }
        
        /*
        List<string> keyList = new List<string>(states.Keys);
        for(int i = 0 ; i < keyList.Count; i ++){
            string key = keyList[i];
            //Debug.Log(key + "<<<<");
        }
        */
    }

    void changeViz(int num){
        float[] votes = new float[3];

                int startYear = 1976;
                int year = 0;
                string stateName = "";
                int totalVotes = 0;

                List<(string,int)> stateInfo = new List<(string,int)>();
                List<string> keyList = new List<string>(states.Keys);
        
        switch(num){
            case 1:

                for(int i = 0 ; i < keyList.Count; i ++){
                    string[] keyInfo = keyList[i].Split(':');
                    if(GameObject.Find(keyInfo[0]) == null){

                        continue;
                    }

                    //Loop through state info and tally votes for that year
                    stateInfo = states[keyList[i]];
                    int numVotes; string party;
                    for(int s = 0; s < stateInfo.Count; s ++){
                        numVotes = stateInfo[s].Item2;
                        party = stateInfo[s].Item1;
                        switch(party){
                            case "democrat":
                                votes[0] += numVotes;
                            break;
                            case "republican":
                                votes[1] += numVotes;
                            break;
                            default://Other parties
                                votes[2] += numVotes;
                            break;
                        }
                    }
                    
                    //Ratio of votes for each party
                    totalVotes = int.Parse(keyInfo[2]);
                    votes[0] = votes[0] / totalVotes;
                    votes[1] = votes[1] / totalVotes;
                    votes[2] = votes[2] / totalVotes;

                    year = int.Parse(keyInfo[1]);
                    stateName = keyInfo[0];

                    GameObject obj = GameObject.Find(stateName+ year);
                    if(votes[1] > votes[0]){
                        obj.GetComponent<Renderer>().material.color = new Color(1, 0, 0);
                    }else{
                        obj.GetComponent<Renderer>().material.color = new Color(0, 0, 1);
                    }
                    
                    votes = new float[3];
                }

            break;
            case 2:

                for(int i = 0 ; i < keyList.Count; i ++){
                    string[] keyInfo = keyList[i].Split(':');
                    if(GameObject.Find(keyInfo[0]) == null){

                        continue;
                    }

                    //Loop through state info and tally votes for that year
                    stateInfo = states[keyList[i]];
                    int numVotes; string party;
                    for(int s = 0; s < stateInfo.Count; s ++){
                        numVotes = stateInfo[s].Item2;
                        party = stateInfo[s].Item1;
                        switch(party){
                            case "democrat":
                                votes[0] += numVotes;
                            break;
                            case "republican":
                                votes[1] += numVotes;
                            break;
                            default://Other parties
                                votes[2] += numVotes;
                            break;
                        }
                    }
                    
                    //Ratio of votes for each party
                    totalVotes = int.Parse(keyInfo[2]);
                    votes[0] = votes[0] / totalVotes;
                    votes[1] = votes[1] / totalVotes;
                    votes[2] = votes[2] / totalVotes;

                    year = int.Parse(keyInfo[1]);
                    stateName = keyInfo[0];

                    GameObject obj = GameObject.Find(stateName+ year);
                    obj.GetComponent<Renderer>().material.color = new Color(votes[1], 0, votes[0]);
                    
                    votes = new float[3];
                }
            break;
        }
    }

    void createCube(){
        
        float[] votes = new float[3];

        int startYear = 1976;
        int year = 0;
        string stateName = "";
        int totalVotes = 0;

        List<(string,int)> stateInfo = new List<(string,int)>();
        List<string> keyList = new List<string>(states.Keys);
        for(int i = 0 ; i < keyList.Count; i ++){
            string[] keyInfo = keyList[i].Split(':');
            if(GameObject.Find(keyInfo[0]) == null){

                continue;
            }

            //Loop through state info and tally votes for that year
            stateInfo = states[keyList[i]];
            int numVotes; string party;
            for(int s = 0; s < stateInfo.Count; s ++){
                numVotes = stateInfo[s].Item2;
                party = stateInfo[s].Item1;
                switch(party){
                    case "democrat":
                        votes[0] += numVotes;
                    break;
                    case "republican":
                        votes[1] += numVotes;
                    break;
                    default://Other parties
                        votes[2] += numVotes;
                    break;
                }
            }
            
            //Ratio of votes for each party
            totalVotes = int.Parse(keyInfo[2]);
            votes[0] = votes[0] / totalVotes;
            votes[1] = votes[1] / totalVotes;
            votes[2] = votes[2] / totalVotes;

            year = int.Parse(keyInfo[1]);
            Vector3 offset = new Vector3(0,0,((year-startYear)/-4)-3);

            GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube) as GameObject;;

            stateName = keyInfo[0];
            obj.name = stateName + year;
            
            obj.GetComponent<Renderer>().material.color = new Color(votes[1], 0, votes[0]);
            obj.transform.position = GameObject.Find(stateName).transform.position + offset;
            dataPrimitives[keyList[i]] = obj;

            votes = new float[3];
        }
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