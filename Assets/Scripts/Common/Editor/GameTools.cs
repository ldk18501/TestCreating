using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ItemKey : ICSVDeserializable
{
    public string mKey;

    public void CSVDeserialize(Dictionary<string, string[]> data, int index)
    {
        mKey = data["ID"][index];
    }
}

public class GameTools
{
    [MenuItem("GameTools/Rename Atlas")]
    static void RenameSpineAtlas()
    {
        EditorWindow.GetWindow<RenameSpineAtlasByPath>(false, "Rename", true);
    }

    [MenuItem("GameTools/Import Item Key to Consts")]
    static void ImportItemKey2Consts()
    {
        string scriptPath = Path.Combine(Application.dataPath, "Scripts/Consts.cs");
        StreamReader fileReader = new StreamReader(scriptPath);
        List<string> allLines = new List<string>();

        string line;
        while ((line = fileReader.ReadLine()) != null)
        {
            allLines.Add(line);
        }
        fileReader.Close();

        int regionStart = -1;
        int regionEnd = -1;
        for (int i = 0; i < allLines.Count; ++i)
        {
            if (allLines[i].Trim().StartsWith("#region 物品ID"))
            {
                regionStart = i;
            }
            else if (allLines[i].Trim().StartsWith("#endregion") && regionStart >= 0)
            {
                regionEnd = i;
                break;
            }
        }

        if (regionStart >= 0 && regionEnd >= 0)
        {
            allLines.RemoveRange(regionStart + 1, regionEnd - regionStart - 1);

            List<ItemKey> keyList = SerializationManager.LoadFromCSV<ItemKey>("Data/Items");
            if (keyList == null)
            {
                LogUtil.LogErrorNoTag("Error with loading 'Data/Items.csv'");
                return;
            }

            for (int i = 0; i < keyList.Count; ++i)
            {
                allLines.Insert(regionStart + 1 + i, string.Format(@"    public const string {0} = ""{1}"";", keyList[i].mKey.ToUpper(), keyList[i].mKey));
            }
        }
        else
        {
            LogUtil.LogErrorNoTag("make sure '#region 物品ID' and '#endregion' come in pair");
        }

        StreamWriter fileWriter = new StreamWriter(scriptPath);
        for (int i = 0; i < allLines.Count; ++i)
        {
            fileWriter.WriteLine(allLines[i]);
        }
        fileWriter.Close();
        AssetDatabase.ImportAsset("Assets/Scripts/Consts.cs");

        Resources.UnloadUnusedAssets();
    }

    public class RenameSpineAtlasByPath : EditorWindow
    {
        private string folderName = "";

        private void OnGUI()
        {
            EditorGUIUtility.labelWidth = 100f;
            EditorGUIUtility.fieldWidth = 90f;
            folderName = EditorGUILayout.TextField("FolderName", folderName);
            
            bool create = GUILayout.Button("Create", GUILayout.Width(150f));

            if (create)
            {
                string asset_path = "Assets/Prefabs/Spine/";
                if (folderName != "")
                {
                    asset_path += folderName + "/";
                }

                string[] paths = Directory.GetDirectories(asset_path);

                foreach (string p in paths)
                {
                    ForeachPath(p);
                }

                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();

                EditorUtility.DisplayDialog("RenameSpineAtlasSuffix", "Success", "Ok");
            }
        }


        //! 遍历文件夹
        private void ForeachPath(string path)
        {
            //! 处理当前文件夹内的
            RenameSuffix(path);

            //string prefab_path = model_path.Replace("Meshes/", "Resources/Prefabs/");
            //! 遍历子文件夹
            string[] subPaths = Directory.GetDirectories(path + "/");
            foreach (string p in subPaths)
            {
                ForeachPath(p);
            }
        }

        private void RenameSuffix(string path)
        {
            string[] files = Directory.GetFiles(path);
            foreach (string file in files)
            {
            
                if (Path.GetExtension(file) == ".atlas")
                {
                    FileInfo fi = new FileInfo(file);
                    fi.MoveTo(Path.ChangeExtension(file, ".atlas.txt"));
                }
            }
        }
    }
}