# Pose Estimation with Unity Barracuda

The purpose of this project is to perform real-time human pose estimation on video input.  This is accomplished by implementing a pre-trained pose estimation machine learning model in Unity with the Barracuda inference library.  The outcome will be a rendered animated skeleton that represents the human poses.

## Prerequisites

* A graphics processing unit (GPU) on your local machine, preferably from a recent generation.  This machine used a NVIDIA Quadro P1000.

* Unity 2019.4.13f1.  You can download Unity from [here]( https://unity3d.com/get-unity/download).  I recommend downloading Unity Hub, which lets you manage multiple projects and different versions of Unity.


## Installing and running the project

* Download or clone this repository to your local machine.

* If you are using Unity Hub, under “Projects”, click “Add” and open the location of the project on your machine.  Unity Hub will automatically detect the version of Unity required for the project and prompt you to install it if necessary.  Click the project from the list to open it inside the Unity Interface.

* If you are not using Unity Hub, simply open the project from inside the Unity Interface.

### Convert the machine learning model into ONNX format
* The Barracuda inference library in Unity requires the ML model to be in ONNX format (Open Neural Network Exchange).

* A tutorial for converting a TensorFlow model to ONNX can be found [here](https://christianjmills.com/tensorflow/onnx/tutorial/2020/10/21/How-to-Convert-a-TensorFlow-SavedModel-to-ONNX.html).

* Documentation to convert a PyTorch model to ONNX can be found [here](https://pytorch.org/tutorials/advanced/super_resolution_with_onnxruntime.html).

* This project uses a pre-trained PoseNet TensorFlow model.  I created a Colab notebook [here](https://colab.research.google.com/drive/1DE0meDsiVmhMqphGYVlHuxDK1AT3J2vp?usp=sharing) that you can use to convert a TensorFlow model.  Just upload your TF model, run the cells and then download the resulting ONNX model.

### Load the video into the player
* In the “Hierarchy” section in the top left, click on “Video Player” to open it in the “Inspector” section to the right.

![video player](Images\video_player.png)

* In the “Project” section at the bottom, click on the `Assets/Videos` folder.  There are already some video files in there, but you can drag and drop any additional videos you want into there. 

* This project only works on single-person video, so make sure there is only 1 person in the video.

![assets video](Images\assets_video.png)
