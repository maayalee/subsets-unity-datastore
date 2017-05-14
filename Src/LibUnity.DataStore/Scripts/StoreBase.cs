using System.Collections.Generic;

namespace LibUnity.DataStore {
  public interface StoreBase {
    bool Exist(string table_name);
    Dictionary<string, object> Load(string table_name);
    void Save(string table_name, object data);
    void Delete(string table_name);
  }
}
