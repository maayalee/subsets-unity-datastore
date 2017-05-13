using System.Collections.Generic;

namespace LibUnity.DataStore {
  public interface IOBase {
    bool Has(string config_name);
    Dictionary<string, object> Load(FormatterBase formmater, string config_name);
    void Save(FormatterBase formatter, string config_name, object data);
    void Delete(string config_name);
  }
}
