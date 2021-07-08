using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Barracuda;

public class PoseNet : MonoBehaviour
{
    [Tooltip("The input image that will be fed to the model")]
    public RenderTexture videoTexture;

    [Tooltip("The ComputeShader that will perform the model-specific preprocessing")]
    public ComputeShader posenetShader;

    [Tooltip("The height of the image being fed to the model")]
    public int imageHeight = 360;

    [Tooltip("The width of the image being fed to the model")]
    public int imageWidth = 360;

    [Tooltip("The model asset file to use when performing inference")]
    public NNModel modelAsset;

    [Tooltip("The backend to use when performing inference")]
    public WorkerFactory.Type workerType = WorkerFactory.Type.Auto;

    // The compiled model used for performing inference
    private Model m_RunTimeModel;

    // The interface used to execute the neural network
    private IWorker engine;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Preprocess the image for the current frame
        Texture2D processedImage = PreprocessImage();
        
        // Remove the processedImage variable
        Destroy(processedImage);
    }

    /// <summary>
    /// Prepare the image to be fed into the neural network
    /// </summary>
    /// <returns>The Processed image</returns>
    private Texture2D PreprocessImage()
    {
        // Create a new Texture2D with the same dimensions as videoTexture
        Texture2D imageTexture = new Texture2D(videoTexture.width,
            videoTexture.height, TextureFormat.RGBA32, false);

        // Copy the RenderTexture contents to the new Texture2D
        Graphics.CopyTexture(videoTexture, imageTexture);

        // Make a temporary Texture2D to store the resized image
        Texture2D tempTex = Resize(imageTexture, imageHeight, imageWidth);
        // Remove the original imageTexture
        Destroy(imageTexture);

        // Apply model-specific preprocessing
        imageTexture = PreprocessResNet(tempTex);
        // Remove the temporary Texture2D
        Destroy(tempTex);
        return imageTexture;
    }

    /// <summary>
    /// Perform model-specific preprocessing on the GPU
    /// </summary>
    /// <param name="inputImage">The image to be processed</param>
    /// <returns>The processed image</returns>
    private Texture2D PreprocessResNet(Texture2D inputImage)
    {
        // Specify the number of threads on the GPU        
        int numthreads = 8;
        // Get the index for the PreprocessResNet function in the ComputeShader
        int kernelHandle = posenetShader.FindKernel("PreprocessResNet");
        // Define an HDR RenderTexture
        RenderTexture rTex = new RenderTexture(inputImage.width, 
            inputImage.height, 24, RenderTextureFormat.ARGBHalf);
        // Enable random write access
        rTex.enableRandomWrite = true;
        // Create the HDR RenderTexture
        rTex.Create();
        
        // Set the value for the Result variable in the ComputeShader
        posenetShader.SetTexture(kernelHandle, "Result", rTex);
        // Set the value for the InputImage variable in the ComputeShader
        posenetShader.SetTexture(kernelHandle, "InputImage", inputImage);

        // Execute the ComputeShader
        posenetShader.Dispatch(kernelHandle, inputImage.width / numthreads, 
            inputImage.height / numthreads, 1);
        // Make the HDR RenderTexture the active RenderTexture
        RenderTexture.active = rTex;

        // Create a new HDR Texture2D
        Texture2D nTex = new Texture2D(rTex.width, rTex.height, TextureFormat.RGBAHalf, false);
        
        // Copy the RenderTexture to the new Texture2D
        Graphics.CopyTexture(rTex, nTex);
        // Make the HDR RenderTexture not the active RenderTexture
        RenderTexture.active = null;
        // Remove the HDR RenderTexture
        Destroy(rTex);
        return nTex;
    }

    /// <summary>
    /// Resize the provided Texture2D
    /// </summary>
    /// <returns>The resized image</returns>
    private Texture2D Resize(Texture2D image, int newWidth, int newHeight)
    {
        // Create a temporary RenderTexture
        RenderTexture rTex = RenderTexture.GetTemporary(newWidth, newHeight, 24);
        // Make the temporary RenderTexture the active RenderTexture
        RenderTexture.active = rTex;

        // Copy the Texture2D to the temporary RenderTexture
        Graphics.Blit(image, rTex);
        // Create a new Texture2D with the new dimensions
        Texture2D nTex = new Texture2D(newWidth, newHeight, TextureFormat.RGBA32, false);
        
        // Copy the temporary RenderTexture to the new Texture2D
        Graphics.CopyTexture(rTex, nTex);

        // Make the temporary RenderTexture not the active RenderTexture
        RenderTexture.active = null;
        
        // Release the temporary RenderTexture
        RenderTexture.ReleaseTemporary(rTex);
        return nTex;
    }
}
