using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
#if UNITY_IOS
using UnityEditor.iOS.Xcode;
#endif


public sealed class BuilderPostProcessor
{
    [PostProcessBuild(1)]
    public static void OnPostProcessBuild(BuildTarget target, string path)
    {
#if UNITY_IOS
        if (target == BuildTarget.iOS)
        {
            var infoPlist = new PlistDocument();
            var infoPlistPath = path + "/Info.plist";
            infoPlist.ReadFromFile(infoPlistPath);

            PlistElementDict dict = infoPlist.root.AsDict();
            dict.SetString("NSBluetoothAlwaysUsageDescription", "App requires access to Bluetooth to allow you connect to device");
            dict.SetString("NSBluetoothPeripheralUsageDescription", "App uses Bluetooth to connect with your Brainbit device");
            infoPlist.WriteToFile(infoPlistPath);
        }
#endif

    }

}
