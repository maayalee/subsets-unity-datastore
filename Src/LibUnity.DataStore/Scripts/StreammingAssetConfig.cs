using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace LibUnity.Core {
  public class StreammingAssetConfig : ConfigSelectorBase {
    /**
     * Constructor
     *  
     */
    public StreammingAssetConfig(string file_path) {
      this.file_path = file_path;
    }

    public bool Has(string config_name) {
      return System.IO.File.Exists(file_path + "/" + config_name + ".json");
    }

    public Dictionary<string, object> Load(string config_name) {
      string text = System.IO.File.ReadAllText(file_path + "/" + config_name + ".json");
      return Json.Decode<Dictionary<string, object>>(text);
    }

    public void Save(string config_name, object data) {
      bool has = Has(config_name);
      string str = Json.Encode(data);//.Replace("{", Environment.NewLine + "{");
      System.IO.File.WriteAllText(file_path + "/" + config_name + ".json", str);
      if (!has) {
        AssetDatabase.Refresh();
      }
    }

    public void Delete(string config_name) {
      System.IO.File.Delete(file_path + "/" + config_name + ".json");
      AssetDatabase.Refresh();
    }
     
    private string file_path;
  }
}
