﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawSkeleton : MonoBehaviour
{
    [Tooltip("The list of key point GameObjects that make up the pose skeleton")]
    public GameObject[] keypoints;

    // The GameObjects that contain data for the lines between key points
    private GameObject[] lines;

    // The line renderers that draw the lines between key points
    private LineRenderer[] lineRenderers;

    // The pairs of key points that should be connected on a body
    private int[][] jointPairs;

    // The width for the skeleton lines
    private float lineWidth = 5.0f;
    
    // Start is called before the first frame update
    void Start()
    {
        // The number of joint pairs
        int numPairs = keypoints.Length + 1;
        // Initialize the lines array
        lines = new GameObject[numPairs];
        // Initialize the lineRenderers array
        lineRenderers = new LineRenderer[numPairs];
        // Initialize the jointPairs array
        jointPairs = new int[numPairs][];
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    #region Additional Methods

    /// <summary>
    /// Initialize the pose skeleton
    /// </summary>
    private void InitializeSkeleton()
    {
        // Nose to left eye
        InitializeLine(0, 0, 1, lineWidth, Color.magenta);
        // Nose to right eye
        InitializeLine(1, 0, 2, lineWidth, Color.magenta);
        // Left eye to left ear
        InitializeLine(2, 1, 3, lineWidth, Color.magenta);
        // Right eye to right ear
        InitializeLine(3, 2, 4, lineWidth, Color.magenta);

        // Left shoulder to right shoulder
        InitializeLine(4, 5, 6, lineWidth, Color.red);
        // Left shoulder to left hip
        InitializeLine(5, 5, 11, lineWidth, Color.red);
        // Right shoulder to right hip
        InitializeLine(6, 6, 12, lineWidth, Color.red);
        // Left shoulder to right hip
        InitializeLine(7, 5, 12, lineWidth, Color.red);
        // Right shoulder to left hip
        InitializeLine(8, 6, 11, lineWidth, Color.red);
        // Left hip to right hip
        InitializeLine(9, 11, 12, lineWidth, Color.red);

        // Left Arm
        InitializeLine(10, 5, 7, lineWidth, Color.green);
        InitializeLine(11, 7, 9, lineWidth, Color.green);
        // Right Arm
        InitializeLine(12, 6, 8, lineWidth, Color.green);
        InitializeLine(13, 8, 10, lineWidth, Color.green);

        // Left Leg
        InitializeLine(14, 11, 13, lineWidth, Color.blue);
        InitializeLine(15, 13, 15, lineWidth, Color.blue);
        // Right Leg
        InitializeLine(16, 12, 14, lineWidth, Color.blue);
        InitializeLine(17, 14, 16, lineWidth, Color.blue);
    }
    
    /// <summary>
    /// Create a line between the key point specified by the start and end point indices
    /// </summary>
    private void InitializeLine(int pairIndex, int startIndex, int endIndex, float width, Color color)
    {
        // Create a new joint pair with the specified start and end point indices
        jointPairs[pairIndex] = new int[] {startIndex, endIndex};

        // Create new line GameObject
        string name = $"{keypoints[startIndex].name}_to_{keypoints[endIndex].name}";
        lines[pairIndex] = new GameObject(name);

        // Add LineRenderer component
        lineRenderers[pairIndex] = lines[pairIndex].AddComponent<LineRenderer>();
        // Make LineRenderer Shader Unlit
        lineRenderers[pairIndex].material = new Material(Shader.Find("Unlit/Color"));
        // Set the material color
        lineRenderers[pairIndex].material.color = color;

        // The line will consist of two points
        lineRenderers[pairIndex].positionCount = 2;

        // Set the width from the start point
        lineRenderers[pairIndex].startWidth = width;
        // Set the width from the end point
        lineRenderers[pairIndex].endWidth = width;
    }

    #endregion
}
