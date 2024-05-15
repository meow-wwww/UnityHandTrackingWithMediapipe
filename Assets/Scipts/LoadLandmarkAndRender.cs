﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LandmarkInterface;
using System;
using System.IO;

public class LoadLandmarkAndRender : MonoBehaviour
{
    // obtain the gameobject named 'HandLandmarkSet' from the scene
    public GameObject LeftHand;
    public HandLandmark LeftHandLandmark;
    public GameObject RightHand;
    public HandLandmark rightHandLandmark;

    private StreamReader reader;
    public string testFilePath = "Assets/CsvData/landmarks_20240325150810_processed.csv";
    public float timeInterval = 0.0524f;

    public float standardHandLength = 0.1456f; // from wrist to middle finger mcp
    // Default Right Hand Length: 0.1455934
    // Default Left Hand Length: 0.2144152
    public float factor;

    // Start is called before the first frame update
    void Start()
    {   
        LeftHandLandmark = LeftHand.GetComponent<HandLandmark>();
        rightHandLandmark = RightHand.GetComponent<HandLandmark>();

        // // print the default right hand landmark position
        // string rightHandDefaultLocalPosition = "";
        // for (int i = 0; i < rightHandLandmark.LandmarkObjects.Count; i++)
        // {
        //     rightHandDefaultLocalPosition += rightHandLandmark.LandmarkObjects[i].transform.localPosition.x.ToString() + ",";
        //     rightHandDefaultLocalPosition += rightHandLandmark.LandmarkObjects[i].transform.localPosition.y.ToString() + ",";
        //     rightHandDefaultLocalPosition += rightHandLandmark.LandmarkObjects[i].transform.localPosition.z.ToString() + ",";
        // }
        // Debug.Log("Local Position: " + rightHandDefaultLocalPosition);
        // float defaultRightHandLength = Vector3.Distance(
        //     rightHandLandmark.LandmarkObjects[0].transform.localPosition,
        //     rightHandLandmark.LandmarkObjects[9].transform.localPosition
        // );
        // Debug.Log("Default Right Hand Length: " + defaultRightHandLength);

        // float defaultLeftHandLength = Vector3.Distance(
        //     LeftHandLandmark.LandmarkObjects[0].transform.localPosition,
        //     LeftHandLandmark.LandmarkObjects[9].transform.localPosition
        // );
        // Debug.Log("Default Left Hand Length: " + defaultLeftHandLength);

        reader = new StreamReader(testFilePath);
        reader.ReadLine(); // skip the first line (header line)

        InvokeRepeating("ReadCSV", 0.5f, timeInterval);
    }

    // void ContinueRead()
    // {
    //     string line = reader.ReadLine();
    //     Debug.Log("[Read a new line]" + line);
    // }

    void ReadCSV()
    {
        // print right hand's local rotation
        Debug.Log("right hand local rotation:"+RightHand.transform.localEulerAngles.ToString());
        try
        {
            string line;
            line = reader.ReadLine(); // read next line's landmarks
            // Debug.Log(line);
            string[] items = line.Split(','); // len=64
            // wrist position (for centralization)
            Vector3 wristPosition = new Vector3(float.Parse(items[1]),
                float.Parse(items[2]),
                float.Parse(items[3]));
            // compute the factor: lm 0: 1,2,3; lm 9: 28,29,30
            float nowHandLength = Vector3.Distance(
                new Vector3(float.Parse(items[1]), float.Parse(items[2]), float.Parse(items[3])),
                new Vector3(float.Parse(items[9*3+1]), float.Parse(items[9*3+2]), float.Parse(items[9*3+3]))
            );
            factor = nowHandLength/standardHandLength;
            // all points
            for (int i = 1; i < items.Length; i += 3)
            {
                int pointId = (i - 1) / 3;
                // Debug.Log("Parse point" + pointId);
                rightHandLandmark.LandmarkObjects[pointId].transform.localPosition = (new Vector3(
                    float.Parse(items[i]), 
                    float.Parse(items[i + 1]), 
                    float.Parse(items[i + 2])) - wristPosition)/factor;
            }
        }
        catch (IOException e)
        {
            Debug.Log("读取文件时发生错误: " + e.Message);
        }


        // LeftHandLandmark.LandmarkObjects[0].transform.position = new Vector3(0.0f, 0.1f, 0.0f) + LeftHandLandmark.LandmarkObjects[0].transform.position;
        // Debug.Log(LeftHandLandmark.LandmarkObjects[0].transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDestroy()
    {
        reader.Close();
    }
}