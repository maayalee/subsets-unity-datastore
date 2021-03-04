using System.Collections.Generic;

namespace LibUnity.DataStore {
  public interface StoreBase {
    bool Exist(string table_name);
    T Load<T>(string table_name);
    void Save(string table_name, object data);
    void Delete(string table_name);
  }
}
