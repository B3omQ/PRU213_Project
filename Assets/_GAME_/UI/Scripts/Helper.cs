using UnityEditor.PackageManager;
using UnityEngine;

public static class Helper
{
   public static string GenerateUniqueID(GameObject obj)
    {
        return $"{obj.scene.name}_{obj.transform.position.x}_{obj.transform.position.y}";
    }
   
}
