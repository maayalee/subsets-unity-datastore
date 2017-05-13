using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace LibUnity.DataStore {
  public class ResourcesIO : IOBase {
    /**
     * Constructor
     *  
     * @param resource_path 설정 파일들이 있는 폴더 패스. 
     *        유니티 Resources 폴더 하위의 패스를 지정.
     */
    public ResourcesIO(string resource_path) {
      this.resource_path = resource_path;
    }

    public bool Has(string config_name) {
      string path = GetSubPath() + "/" + config_name;
      TextAsset asset = Resources.Load<TextAsset>(path);
      return null == asset ? false : true;
    }

    public Dictionary<string, object> Load(FormatterBase formatter, string config_name) {
      string path = GetSubPath() + "/" + config_name;
      TextAsset asset = Resources.Load<TextAsset>(path);
      if (null == asset)
        throw new System.Exception("load failed. \"" + path +
          "\" asset is not exist in Resources folder");
      return formatter.Decode<Dictionary<string, object>>(asset.text);
    }

    private string GetSubPath() {
      string[] tokens = resource_path.Split('/');
      int start_index = 0;
      for (int i = 0; i < tokens.Length; ++i) {
        if (tokens[i] == "Resources") {
          start_index = i + 1;
        }
      }
      string result = "";
      for (int i = start_index; i < tokens.Length; ++i) {
        result += tokens[i];
        if (i != (tokens.Length - 1)) {
          result += "/";
        }
      }
      return result;
    }

    public void Save(FormatterBase formatter, string config_name, object data) {
      bool has = Has(config_name);
      string str = formatter.Encode(data, true);
      System.IO.File.WriteAllText(resource_path + "/" + config_name + ".json", str);
#if UNITY_EDITOR
      if (!has) {
        AssetDatabase.Refresh();
      }
#endif
    }

    public void Delete(string config_name) {
      System.IO.File.Delete(resource_path + "/" + config_name + ".json");
#if UNITY_EDITOR
      AssetDatabase.Refresh();
#endif
    }

    private string resource_path;
  }
}
