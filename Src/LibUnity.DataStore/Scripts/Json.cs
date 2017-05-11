using System;
using UnityEngine;
using JsonFx.Json;

namespace LibUnity.DataStore {
  public class Json {
    public static string Encode(object data) {
      JsonWriterSettings settings = new JsonWriterSettings();
      settings.PrettyPrint = true;

      System.Text.StringBuilder result = new System.Text.StringBuilder();
      JsonWriter writer = new JsonWriter(result, settings);
      writer.Write(data);
      if (0 == result.Length)
        throw new System.Exception("encode result is empty string");
      return result.ToString();
    }

    public static T Decode<T>(string json_string) {
      T result = JsonFx.Json.JsonReader.Deserialize<T>(json_string);
      if (null == result)
        throw new System.Exception("decode result is null");
      return result;
    }
  }
}
