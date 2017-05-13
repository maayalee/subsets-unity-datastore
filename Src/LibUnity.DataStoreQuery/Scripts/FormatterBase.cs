namespace LibUnity.DataStore {
  public interface FormatterBase {
    string Encode(object data, bool pretty_print = false);
    T Decode<T>(string json_string);
  }
}
