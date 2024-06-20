using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LandmarkInterface;
using System;
using System.IO;

public class LoadLandmarkAndRender : MonoBehaviour
{
    // obtain the gameobject named 'HandLandmarkSet' from the scene
    public GameObject LeftHandGt;
    HandLandmark LeftHandLandmarkGt;
    public GameObject RightHandGt;
    HandLandmark rightHandLandmarkGt;

    public GameObject LeftHandPred;
    HandLandmark LeftHandLandmarkPred;
    public GameObject RightHandPred;
    HandLandmark rightHandLandmarkPred;

    private StreamReader readerGt, readerPred;
    public string testFilePathGt;
    public float timeInterval = 0.0524f;

    public float standardHandLength = 0.1456f; // from wrist to middle finger mcp
    // Default Right Hand Length: 0.1455934
    // Default Left Hand Length: 0.2144152
    public float factor;

    // Start is called before the first frame update
    void Start()
    {   
        LeftHandLandmarkGt = LeftHandGt.GetComponent<HandLandmark>();
        rightHandLandmarkGt = RightHandGt.GetComponent<HandLandmark>();
        LeftHandLandmarkPred = LeftHandPred.GetComponent<HandLandmark>();
        rightHandLandmarkPred = RightHandPred.GetComponent<HandLandmark>();

        readerGt = new StreamReader(testFilePathGt);
        readerGt.ReadLine(); // skip the first line (header line)
        
        string testFilePathPred = testFilePathGt.Replace("gt.csv", "pred.csv");
        readerPred = new StreamReader(testFilePathPred);
        readerPred.ReadLine(); // skip the first line (header line)

        InvokeRepeating("ReadCSVGtWrapper", 0.5f, timeInterval);
        InvokeRepeating("ReadCSVPredWrapper", 0.5f, timeInterval);
    }

    void ReadCSV(StreamReader reader, HandLandmark rHandLandmark)
    {
        // print right hand's local rotation
        // Debug.Log("right hand local rotation:"+RightHand.transform.localEulerAngles.ToString());
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
                rHandLandmark.LandmarkObjects[pointId].transform.localPosition = (new Vector3(
                    float.Parse(items[i]), 
                    float.Parse(items[i + 1]), 
                    float.Parse(items[i + 2])) - wristPosition)/factor;
            }
        }
        catch (Exception e)
        {
            Debug.Log("读取文件"+reader.ToString()+"时发生错误: " + e.Message);
        }

    }

    void ReadCSVGtWrapper()
    {
        ReadCSV(readerGt, rightHandLandmarkGt);
    }
    void ReadCSVPredWrapper()
    {
        ReadCSV(readerPred, rightHandLandmarkPred);
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDestroy()
    {
        readerGt.Close();
        readerPred.Close();
    }
}
