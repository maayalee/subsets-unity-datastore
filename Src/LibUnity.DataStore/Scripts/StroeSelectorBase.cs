using System.Collections.Generic;

namespace LibUnity.DataStore {
  public interface StoreSelectorBase {
    bool Has(string config_name);
    Dictionary<string, object> Load(string config_name);
    void Save(string config_name, object data);
    void Delete(string config_name);
  }
}
