﻿using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace LibUnity.DataStore {
  public class ResourcesIO : IOBase {
    /**
     * Constructor
     */
    public ResourcesIO(string store_path) {
      this.store_path = store_path;
    }

    public bool Exist(string table) {
      TextAsset asset = Resources.Load<TextAsset>(store_path + "/" + table);
      return null == asset ? false : true;
    }

    public string Read(string table) {
      TextAsset asset = Resources.Load<TextAsset>(store_path + "/" + table);
      if (null == asset)
        throw new System.Exception("load failed. \"" + table +
          "\" asset is not exist in Resources folder");
      return asset.text;
    }

    public void Write(string table, string data) {
      throw new System.Exception("Resources unable write");
    }

    public void Delete(string table) {
      throw new System.Exception("Resources unable write");
    }

    private string store_path;
  }
}
