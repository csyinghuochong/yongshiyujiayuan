using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public class GetPrefabPath : MonoBehaviour
{
    [MenuItem("Tools/GetPrefabPath")] 
    public static void testselect() 
    {
        UnityEngine.Object selectgo = Selection.activeObject;
        UnityEngine.GameObject go = selectgo as UnityEngine.GameObject;
        UI_FunctionOpen ttt = go.GetComponent<UI_FunctionOpen>();

        //获取实例组件的所有字段（BindingFlags限制枚举）
        FieldInfo[] allFieldInfo = (ttt.GetType()).GetFields(BindingFlags.NonPublic | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly | BindingFlags.Static);

        string prefabPath = "";
        
        for (int i = 0; i < allFieldInfo.Length; i++)
        {
            if (allFieldInfo[i].FieldType == typeof(UnityEngine.GameObject))
            {

                UnityEngine.GameObject totoot = allFieldInfo[i].GetValue(ttt) as UnityEngine.GameObject;

                if (totoot != null)
                {
                    string path_1 = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(totoot as UnityEngine.Object);

                    if (path_1 != "")
                    {
                        path_1 = path_1.Substring( 17, path_1.Length - 17);

                        prefabPath = prefabPath + allFieldInfo[i].Name + "    " + path_1;
                        prefabPath = prefabPath + "\n";
                    }
                }
            }
        }

        string txt_path = "F:/1.txt";
        StreamWriter sw = new StreamWriter(txt_path);
        sw.WriteLine(prefabPath);
        sw.Close();

        Debug.Log(prefabPath);
    }
}
