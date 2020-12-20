﻿using System;
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.Callbacks;

public class PostBuildActions
{
    [PostProcessBuild]
    public static void OnPostProcessBuild(BuildTarget target, string targetPath)
    {
        var path = Path.Combine(targetPath, "Build/UnityLoader.js");
        var text = File.ReadAllText(path);
        text = Regex.Replace(text, @"compatibilityCheck:function\(e,t,r\)\{.+,Blobs:\{\},loadCode",
            "compatibilityCheck:function(e,t,r){t()},Blobs:{},loadCode");
        File.WriteAllText(path, text);
    }
}
