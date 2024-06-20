# Unity Hand Tracking with Mediapipe
This is a demo of realtime hand tracking and finger tracking in Unity using Mediapipe.

The tracking section is built on Android but a similar approach should also be applicable for desktop or IOS.

It works by first detecting the hand landmarks by Mediapipe in Android, and then sending the results to PC via adb and protobuf, and finally interpreting the results in PC Unity.

![Hand Landmark Demo](gifs/demo_landmarks.gif)
![Model Demo](gifs/demo_model.gif)

## Prerequisites
* Windows 10 PC recommended
* Android mobile device (recommended with Android version 8.0 or above)
* Unity with Android Build Support and Android SDK & NDK Tools (recommended with version 2019.4.6f1 or 2019.4.x)

## Installation
1. Enable Android Developer Mode and USB debugging in the mobile device. Connect the device with PC and allow permissions.

2. Install the "UnityHandTracking.apk" to the device: 
drag and drop the apk into device and click the apk in the device's FileManager
, or by command: `adb install UnityHandTracking.apk`. 
A built version of apk is included in release. The source code of the apk is available in [mediapipe_multi_hands_tracking_aar_unity](https://github.com/TesseraktZero/mediapipe_multi_hands_tracking_aar_unity).
   
3. Open the SampleScene in Unity project. In the scene, navigate to `unitychan`>`HandLandmarkAndRigs`>`HandLandmarkSet`. Update the `Adb Path` according to point to local adb which installed along with Unity Android Build Support. The path should have patterns similar to one of the belows:

   - `C:\Unity\2019.4.6f1\Editor\Data\PlaybackEngines\AndroidPlayer\SDK\platform-tools\adb.exe`

   - `C:\Users\user\AppData\Local\Android\Sdk\platform-tools\adb.exe`

4. Start the SampleScene in Unity project. This should automatically start the Android app and receive data from it.
   
5. Hold the device vertically and capture both hands for best tracking.

## Customization
To apply hand tracking on your own avatar, follow the below steps:

1. Setup `Animtaion Rigging` on the model:
   - Add Prefab `HandLandmarkAndRigs` as child of the model. Add component `Rig Builder` to the model. In the `Rig Layers` of `Rig Builder`, add the four rigs located under `HandLandmarkAndRigs/Rigs` (`LeftHandRig`, `LeftFingerRig`, `RightHandRig`, `RightFingerRig`).
   - In the four Hand Rigs, reassign the `Root` and `Tip` bones based on your model armature. Refer to the sample scene for details.
   - For each of the Hand Rigs, align their transform with the `Tip` transform. To do so, select that object and hold control select object assign in `Tip`. Then, navigate to menu: `Animation Rigging` -> `Align Transform`

2. Adjust the position and rotation of the prefab `HandLandmarkSet` to fit with the model.

## License

This project is under `Apache License 2.0`.

Third party assets used in this project are under their own licenses which can be found in corresponding asset folders.

## HandTracking

### Environment Issue

These two error messages may appear when opening the project, but they can be ignored.

- `Unable to retrieve package list from http://www.nuget.org/api/v2/FindPackagesById()?id='sharpadbclient'&$orderby=Version asc&$filter=Version eq '2.3.22'`
`System.Net.WebException: The remote server returned an error: (400) Bad Request.`

- `Could not find sharpadbclient 2.3.22 or greater.`

### Entry Point

Object 'LandmarkLoader' in the scene 'SampleScene'.

In Inspector, set `Test File Path Gt` to be the absolute path of ground truth data (a csv file). 
The ground truth file should have name like `<path>/cross_0_gt.csv`. 

There should also be a prediction file, the file name should be `<path>/cross_0_pred.csv`.

#### CSV File Format

Here is an example file: 

```
ts,0_x,0_y,0_z,1_x,1_y,1_z,2_x,2_y,2_z,3_x,3_y,3_z,4_x,4_y,4_z,5_x,5_y,5_z,6_x,6_y,6_z,7_x,7_y,7_z,8_x,8_y,8_z,9_x,9_y,9_z,10_x,10_y,10_z,11_x,11_y,11_z,12_x,12_y,12_z,13_x,13_y,13_z,14_x,14_y,14_z,15_x,15_y,15_z,16_x,16_y,16_z,17_x,17_y,17_z,18_x,18_y,18_z,19_x,19_y,19_z,20_x,20_y,20_z
unknown,0.4,0.6,0.0,0.3,0.6,0.0,0.3,0.6,0.0,0.2,0.5,0.0,0.2,0.5,0.0,0.3,0.5,0.0,0.3,0.4,0.0,0.3,0.4,0.0,0.3,0.4,0.0,0.4,0.5,0.0,0.4,0.4,0.0,0.4,0.4,0.0,0.4,0.3,0.0,0.4,0.5,0.0,0.4,0.4,0.0,0.4,0.4,0.0,0.4,0.3,-0.0,0.5,0.5,0.0,0.4,0.4,0.0,0.4,0.4,-0.0,0.4,0.4,-0.0
unknown,0.4,0.7,0.0,0.4,0.7,0.0,0.3,0.6,0.0,0.3,0.6,0.0,0.3,0.6,0.0,0.4,0.5,0.0,0.3,0.4,0.0,0.3,0.4,0.0,0.3,0.4,0.0,0.4,0.5,0.0,0.4,0.4,0.0,0.4,0.3,0.0,0.4,0.3,0.0,0.4,0.5,0.0,0.4,0.4,0.0,0.4,0.4,-0.0,0.4,0.3,-0.0,0.5,0.5,-0.0,0.4,0.4,-0.0,0.4,0.4,-0.0,0.4,0.4,-0.0
unknown,0.4,0.7,0.0,0.4,0.7,0.0,0.3,0.6,0.0,0.3,0.6,0.0,0.3,0.5,0.0,0.4,0.5,0.0,0.3,0.4,0.0,0.3,0.4,0.0,0.3,0.4,0.0,0.4,0.5,0.0,0.4,0.4,0.0,0.4,0.3,0.0,0.4,0.3,0.0,0.4,0.5,0.0,0.4,0.4,0.0,0.4,0.4,-0.0,0.4,0.3,-0.0,0.4,0.5,-0.0,0.4,0.4,-0.0,0.4,0.4,-0.0,0.4,0.4,-0.0
```

`ts` should be the timestamp of its corresponding frame, but it has not been used by the simulator yet.