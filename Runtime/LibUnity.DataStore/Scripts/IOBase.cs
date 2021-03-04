using System.Collections.Generic;

namespace LibUnity.DataStore {
  public interface IOBase {
    bool Exist(string table);
    string Read(string table);
    void Write(string table, string data);
    void Delete(string table);
  }
}
