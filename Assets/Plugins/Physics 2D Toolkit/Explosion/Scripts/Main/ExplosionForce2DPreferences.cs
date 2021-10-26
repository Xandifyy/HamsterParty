using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using System.IO;
[InitializeOnLoad]
class EditorLaunchCheck
{
    static EditorLaunchCheck()
    {
        if (File.Exists("Assets/Plugins/Physics 2D Toolkit/Gizmos/ExplosionGizmo.png"))
        {
            if (!AssetDatabase.IsValidFolder("Assets/Gizmos"))
                AssetDatabase.CreateFolder("Assets", "Gizmos");
            
            //Debug.Log(AssetDatabase.ValidateMoveAsset("Assets/Plugins/Physics 2D Toolkit/Gizmos/ExplosionGizmo.png", "Assets/Gizmos/ExplosionGizmo.png"));
            if (string.IsNullOrEmpty(AssetDatabase.ValidateMoveAsset("Assets/Plugins/Physics 2D Toolkit/Gizmos/ExplosionGizmo.png", "Assets/Gizmos/ExplosionGizmo.png")))
            {
                AssetDatabase.MoveAsset("Assets/Plugins/Physics 2D Toolkit/Gizmos/ExplosionGizmo.png", "Assets/Gizmos/ExplosionGizmo.png");
            }
        }
    }
}
#endif

public class ExplosionForce2DPreferences
{
    // Colors
    public static readonly Color firstRadiusColor = Color.red;
    public static readonly Color secondRadiusColor = Color.blue;
    public static readonly Color thirdRadiusColor = Color.green;

    // Handle Text
    public static readonly bool showTextAboveHandles = true;
    public static readonly Color aboveHandleTextColor = Color.white;

    // Gizmos
    public static readonly bool showGizoms = true;


    public static bool DoesExplosionGizmoExist()
    {
#if UNITY_EDITOR
        if (File.Exists("Assets/Gizmos/ExplosionGizmo.png"))
            return true;
#endif
        return false;
    }

}


