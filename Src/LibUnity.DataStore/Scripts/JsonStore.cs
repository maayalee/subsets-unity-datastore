using System.Collections.Generic;
using JsonFx.Json;

namespace LibUnity.DataStore {
  public class JsonStore : StoreBase {
    public JsonStore(IOBase io) {
      this.io = io;
    }

    public bool Exist(string table_name) {
      return io.Exist(table_name);
    }

    public Dictionary<string, object> Load(string table_name) {
      string text = io.Read(table_name);
      return Decode<Dictionary<string, object>>(text);
    }

    public void Save(string table_name, object data) {
      io.Write(table_name, Encode(data, true));
    }

    public void Delete(string table_name) {
      io.Delete(table_name);
    }

    private string Encode(object data, bool pretty_print = false) {
      JsonWriterSettings settings = new JsonWriterSettings();
      settings.PrettyPrint = pretty_print;

      System.Text.StringBuilder result = new System.Text.StringBuilder();
      JsonWriter writer = new JsonWriter(result, settings);
      writer.Write(data);
      if (0 == result.Length)
        throw new System.Exception("encode result is empty string");
      return result.ToString();
    }

    private T Decode<T>(string json_string) {
      T result = JsonFx.Json.JsonReader.Deserialize<T>(json_string);
      if (null == result)
        throw new System.Exception("decode result is null");
      return result;
    }

    public static Query Create(IOBase io) {
      return new Query(new JsonStore(io));
    }

    private IOBase io;
  }
}
