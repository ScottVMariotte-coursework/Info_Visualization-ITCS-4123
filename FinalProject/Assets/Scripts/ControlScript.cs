using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlScript : MonoBehaviour
{   
    List<string> keys;
    Dictionary<string,string[,]> Data = new Dictionary<string, string[,]>();
    Dictionary<string,GameObject> Arrows = new Dictionary<string, GameObject>();
    Dictionary<string,GameObject> Rain = new Dictionary<string, GameObject>();

    int numDays;
    int currentFrame = 0;

    int[] monthsToDays = new int[12];
    

    int current_day = 0;
    // Start is called before the first frame update
    void Start()
    {
        monthsToDays[0] = 0;
        monthsToDays[1] = monthsToDays[0]+31;
        monthsToDays[2] = monthsToDays[1]+28;
        monthsToDays[3] = monthsToDays[2]+31;
        monthsToDays[4] = monthsToDays[3]+30;
        monthsToDays[5] = monthsToDays[4]+31;
        monthsToDays[6] = monthsToDays[5]+30;
        monthsToDays[7] = monthsToDays[6]+31;
        monthsToDays[8] = monthsToDays[7]+31;
        monthsToDays[9] = monthsToDays[8]+30;
        monthsToDays[10] = monthsToDays[9]+31;
        monthsToDays[11] = monthsToDays[10]+30;
        
        readData();
        string[,] data = Data["Albury"];

        for(int i = 700; i < 1000; i ++){
            //Debug.Log(data[i,0]);
        }

        GameObject arrowPrefab  = Resources.Load("Arrow_1") as GameObject;
        Mesh arrowMesh = arrowPrefab.GetComponent<MeshFilter>().sharedMesh;

        GameObject rainPrefab  = Resources.Load("Rain_1") as GameObject;
        Mesh rainMesh = rainPrefab.GetComponent<MeshFilter>().sharedMesh;

        keys = new List<string>(Data.Keys);
        for(int i = 0; i < keys.Count; i ++){
            GameObject placeHolder = GameObject.Find(keys[i].ToLower ());
            if(placeHolder == null){continue;}

            GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube) as GameObject;
            obj.name = keys[i];
            obj.transform.position = new Vector3(placeHolder.transform.position.x,placeHolder.transform.position.y,-.5f);
            obj.GetComponent<MeshFilter>().sharedMesh = arrowMesh;
            Arrows.Add(keys[i],obj);

            obj = GameObject.CreatePrimitive(PrimitiveType.Cube) as GameObject;
            obj.name = keys[i];
            obj.transform.position = new Vector3(placeHolder.transform.position.x+2.0f,placeHolder.transform.position.y+1.0f,-0.7f);
            obj.GetComponent<MeshFilter>().sharedMesh = rainMesh;
            Rain.Add(keys[i],obj);
        }
        keys = new List<string>(Arrows.Keys);
    }
    
    // Update is called once per frame
    //min/maxTemp, Windir9/3, WinSpeed9/3, Rain
    void Update()
    {
        //Debug.Log(Input.GetKeyDown(""));
        int count = 0;
        if(Input.GetKey(KeyCode.LeftAlt)){
            if(Input.GetKey(KeyCode.RightArrow) && currentFrame < numDays*2){
            count ++;
            }
            if(Input.GetKey(KeyCode.LeftArrow) && currentFrame > 0){
                count --;
            }
        }else{
            if(Input.GetKeyDown(KeyCode.RightArrow) && currentFrame < numDays*2){
                count ++;
            }
            if(Input.GetKeyDown(KeyCode.LeftArrow) && currentFrame > 0){
                count --;
            }
        }

        currentFrame += count;
        for(int i = 0; i < keys.Count; i ++){
            int index = currentFrame/2;
            int time = currentFrame%2;

            string location = keys[i];
            string[,] data = Data[location];


            
            //WindDirection
            GameObject obj = Arrows[location];
            obj.transform.rotation = Quaternion.Euler(0,0,windToAngle(data[index,3 - time]));
            //WindSpeed
            if(data[index,5-time] != null && data[index,5-time] != "NA"){
                //Debug.Log(time);
                Debug.Log(data[index,5-time]);
                float value = float.Parse(data[index,5-time]);
                value = (value / 60.0f) *  3.0f + 1;
                obj.transform.localScale = new Vector3(1,value,1);
            }
            //Temp
            if(data[index,1] != null && data[index,1] != "NA"){
                float value = float.Parse(data[index,1]);
                value = ((value+20.0f) / 63);

                float r = value;
                float b = 1.0f - value;
                obj.GetComponent<Renderer>().material.color = new Color(r,0,b);
            }
            obj = Rain[location];
            if(data[index,6] != null && data[index,6] != "NA"){
                float value = float.Parse(data[index,6]);
                if(value == 0.0){
                    obj.transform.position = new Vector3(obj.transform.position.x,obj.transform.position.y,4.0f);
                }else{
                    obj.transform.position = new Vector3(obj.transform.position.x,obj.transform.position.y,-0.7f);

                    value = (value / 40.0f) + 0.5f;
                    obj.transform.localScale = new Vector3(value,value,value);
                }
            }
            
            //Debug.Log(obj.GetComponent<Renderer>().material.color);
        }
    }
    
    //Start = 1/1/2007 > 6/25/2017
    //min/maxTemp, Windir9/3, WinSpeed9/3, Rain
    void readData(){
        //Date,Location,MinTemp,MaxTemp,Rainfall,
        //Evaporation,Sunshine,WindGustDir,WindGustSpeed,WindDir9am,
        //WindDir3pm,WindSpeed9am,WindSpeed3pm,Humidity9am,Humidity3pm,
        //Pressure9am,Pressure3pm,Cloud9am,Cloud3pm,Temp9am,
        //Temp3pm,RainToday,RISK_MM,RainTomorrow

        int endIndex = getIndexFromDate("2017-6-25");
        numDays = endIndex;

        string[] reader1 = System.IO.File.ReadAllLines("Assets/Data/WeatherData.csv");
        string[,] str_data = new string[endIndex+1,7];
    
        //string[] blank = {"NA","NA","NA","NA","NA","NA","NA","NA"};
        string currentLocation = "";

        string destinationLocation = reader1[0].Split(',')[1];
        int Index = getIndexFromDate(reader1[0].Split(',')[0]);

        //for(int i = 0; i < startingIndex; i ++){
        //    str_data[i] = blank;
        //}

        for (int i = 1; i < reader1.Length; i ++){
            if( reader1[i] == ""){continue;}

            string[] line1 = reader1[i].Split(',');
            currentLocation = line1[1];
            if(currentLocation != destinationLocation){
                Data.Add(destinationLocation,str_data);
                str_data = new string[endIndex+1,7];

                destinationLocation = line1[1];
                Index = getIndexFromDate(line1[0]);

                //for(int i = 0; i < startingIndex; i ++){
                //    str_data[i] = blank;
                //}
            }

            str_data[Index,0] = line1[4];//temp
            str_data[Index,1] = line1[2];//temp
            str_data[Index,2] = line1[9];//winddir
            str_data[Index,3] = line1[10];//winddir
            str_data[Index,4] = line1[11];//windspeed
            str_data[Index,5] = line1[12];//windspeed
            str_data[Index,6] = line1[4];//rain
            Index += 1;
        }
    }

    void fillBlank(int endIndex){
        for(int i = 0; i < endIndex; i ++){

        }
    }

    float windToAngle(string windDir){
        switch(windDir){
            case "N":
                return 0.0f;
                break;

            case "NNW":
                return 22.5f;
                break;
            
            case "NW":
                return 45.0f;
                break;

            case "WNW":
                return 67.5f;
                break;

            case "S":
                return 180.0f;
                break;

            case "SSW":
                return 157.5f;
                break;

            case "SW":
                return 135.0f;
                break;

            case "WSW":
                return 112.5f;
                break;

            case "W":
                return 90.0f;
                break;
            
            case "NNE":
                return 337.5f;
                break;

            case "NE":
                return 315.0f;
                break;

            case "ENE":
                return 292.5f;
                break;

            case "SSE":
                return 202.5f;
                break;

            case "SE":
                return 225.0f;
                break;

            case "ESE":
                return 247.5f;
                break;

            case "E":
                return 270.0f;
                break;
        }
        return -1;
    }

    string getDateFromIndex(int index){
        int month = 0;
        int year = 2007;
        //int endYear = 2017;

        while(index >= 365){
            if(leapYear(year)){
                if(index >= 366){
                    index -= 366;
                    year ++;
                }else{
                    break;
                }
            }else{
                if(year == 2007){
                    index -= 61;
                    year ++;
                }else{
                    index -= 365;
                    year ++;
                }
            }
        }
        while(month < 11 && monthsToDays[month+1] < index){
            month++;
        }
        
        index -= monthsToDays[month];

        if(leapYear(year) && month > 2){
            index -= 1;
        }

        return year + "-" + month + 1 + "-" + index + 1;
    }

    int getIndexFromDate(string date){
        string[] _date = date.Split('-');
        int year = int.Parse(_date[0]);
        int month = int.Parse(_date[1]);
        int day = int.Parse(_date[2]);

        int startMonth = 11;
        int startYear = 2007;
        int endYear = 2017;

        int count = 0;
        int i = startYear;
        while(i != year){
            if(leapYear(i)){
                count += 366;
            }else{
                if(year == 2007){
                    count += 61;
                }else{
                    count += 365;
                }
            }
            i++;
        }
        if(leapYear(year) && month > 2){
            count += 1;
        }
        count += monthsToDays[month - 1] + day - 1;
        return count;
    }

    bool leapYear(int year){
        return year % 4 == 0 && year % 100 != 0;
    }
}
