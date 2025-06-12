using UnityEngine;

public class HandTracker : MonoBehaviour
{
    public UDPListner uDPListner;
    [SerializeField] private GameObject[] points_Go;
    [SerializeField] private Material lineMat;
    [SerializeField] private GameObject boneParent;
    [SerializeField] private float boneWidth;

    private LineRenderer[] boneLines;



    private int[,] bonePairs = new int[,] {
    {0,1}, {1,2}, {2,3}, {3,4},     // Thumb
    {0,5}, {5,6}, {6,7}, {7,8},     // Index
    {0,9}, {9,10}, {10,11}, {11,12},// Middle
    {0,13}, {13,14}, {14,15}, {15,16}, // Ring
    {0,17}, {17,18}, {18,19}, {19,20},  // Pinky
    {5, 9}, {9,13 }, { 13,17} 
    };

    void Awake()
    {
        uDPListner = transform.GetComponent<UDPListner>();


        //MAKE THE BONES
        boneLines = new LineRenderer[bonePairs.GetLength(0)];

        for (int i = 0; i < boneLines.Length; i++)
        {
            GameObject lineObj = new GameObject("BoneLine_" + i);
            lineObj.transform.parent = boneParent.transform;
            LineRenderer lr = lineObj.AddComponent<LineRenderer>();
            lr.material = lineMat;
            lr.startWidth = boneWidth;
            lr.endWidth = boneWidth;
            lr.positionCount = 2;
            boneLines[i] = lr; 
        }
    }

    void Update()
    {
        string data = uDPListner.data;
        if (string.IsNullOrEmpty(data)) return;

        data = data.Trim('[', ']');
        string[] point_str = data.Split(',');


        for (int i = 0; i < 21; i++)
        {
            float x = 5 - float.Parse(point_str[i * 3]) / 100;
            float y = (float.Parse(point_str[i * 3 + 1]) / 100);
            float z = float.Parse(point_str[i * 3 + 2]) / 100;

            points_Go[i].transform.localPosition = new Vector3(x, y, z);
        }

        for (int i = 0; i < boneLines.Length; i++)
        {
            int startIdx = bonePairs[i, 0];
            int endIdx = bonePairs[i, 1];

            Vector3 start = points_Go[startIdx].transform.localPosition;
            Vector3 end = points_Go[endIdx].transform.localPosition;

            boneLines[i].SetPosition(0, start);
            boneLines[i].SetPosition(1, end);
        }
    }
}
