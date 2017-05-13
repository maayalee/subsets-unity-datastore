using JsonFx.Json;

namespace LibUnity.DataStore {
  public class JsonFormatter : FormatterBase {
    public string Encode(object data, bool pretty_print = false) {
      JsonWriterSettings settings = new JsonWriterSettings();
      settings.PrettyPrint = pretty_print;

      System.Text.StringBuilder result = new System.Text.StringBuilder();
      JsonWriter writer = new JsonWriter(result, settings);
      writer.Write(data);
      if (0 == result.Length)
        throw new System.Exception("encode result is empty string");
      return result.ToString();
    }

    public T Decode<T>(string json_string) {
      T result = JsonFx.Json.JsonReader.Deserialize<T>(json_string);
      if (null == result)
        throw new System.Exception("decode result is null");
      return result;
    }
  }
}
