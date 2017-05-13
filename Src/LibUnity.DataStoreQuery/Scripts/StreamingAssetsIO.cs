using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace LibUnity.DataStore {
  public class StreamingAssetsIO : IOBase {
    /**
     * Constructor
     */
    public StreamingAssetsIO(string file_path) {
      this.file_path = file_path;
    }

    public bool Has(string config_name) {
      return System.IO.File.Exists(file_path + "/" + config_name + ".json");
    }

    public Dictionary<string, object> Load(FormatterBase formatter, string config_name) {
      string text = System.IO.File.ReadAllText(file_path + "/" + config_name + ".json");
      return formatter.Decode<Dictionary<string, object>>(text);
    }

    public void Save(FormatterBase formatter, string config_name, object data) {
      bool has = Has(config_name);
      string str = formatter.Encode(data, true);
      System.IO.File.WriteAllText(file_path + "/" + config_name + ".json", str);
#if UNITY_EDITOR
      if (!has) {
        AssetDatabase.Refresh();
      }
#endif
    }

    public void Delete(string config_name) {
      System.IO.File.Delete(file_path + "/" + config_name + ".json");
#if UNITY_EDITOR
      AssetDatabase.Refresh();
#endif
    }
     
    private string file_path;
  }
}
