using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;

public class MediaStyleNotificationPostProcessBuild
{
#if UNITY_IOS
    [PostProcessBuildAttribute(1)]
    public static void OnPostProcessBuild(BuildTarget buildTarget, string path)
    {
        if (buildTarget == BuildTarget.iOS)
        {
            var exportPath = new DirectoryInfo(path).FullName;
            var projectPath = new DirectoryInfo(
                Path.Combine(Path.Combine(exportPath, "Unity-iPhone.xcodeproj"), "project.pbxproj")).FullName;

            var project = new PBXProject();
            project.ReadFromFile(projectPath);
            //var frameworks = project.GetUnityMainTargetGuid();
            var frameworks = project.TargetGuidByName("UnityFramework");

            project.AddFrameworkToProject(frameworks, "MediaPlayer.framework", false);
            project.AddBuildProperty(frameworks, "OTHER_LDFLAGS", "-ObjC");
            project.WriteToFile(projectPath);
        }
    }
#endif    
}
