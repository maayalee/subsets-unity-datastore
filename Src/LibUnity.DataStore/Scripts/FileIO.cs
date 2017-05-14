using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace LibUnity.DataStore {
  public class FileIO : IOBase {
    /**
     * Constructor
     */
    public FileIO(string store_path, string extension = "json") {
      this.store_path = store_path;
      this.extension = extension;
    }

    public bool Exist(string table) {
      return System.IO.File.Exists(store_path + "/" + table + "." + extension);
    }

    public string Read(string table) {
      return System.IO.File.ReadAllText(store_path + "/" + table + "." + extension);
    }

    public void Write(string table, string data) {
      System.IO.File.WriteAllText(store_path + "/" + table + "." + extension, data);
    }

    public void Delete(string table) {
      System.IO.File.Delete(store_path + "/" + table + "." + extension);
    }
     
    private string store_path;
    private string extension;
  }
}
