using System.Collections.Generic;

namespace LibUnity.DataStore {
  /**
   * \class Query
   *
   * \brief 패스 기반 방식의 데이터 읽기/쓰기 인터페이스를 제공한다. 
   *
   * \author Lee, Hyeon-gi
   */
  public class Query {
    /**
     * Constructor
     * 
     * \param io 저장 대상 
     */
    public Query(StoreBase store) {
      caches = new Dictionary<string, object>();
      this.store = store;
    }

    /**
     * 설정 정보를 변경한다.
     * 
     * \param path 저장 위치
     * \param value 저장 정보
     * 
     * \throw Exception
     */
    public void Set<T>(string path, T value, bool persistent = false) {
      string[] tokens = path.Split('.');
      string table_name = tokens[0];
      if (!caches.ContainsKey(table_name)) {
        Load<object>(table_name);
      }
      Dictionary<string, object> current = caches[table_name] as Dictionary<string, object>;
      for (int i = 1; i < tokens.Length; ++i) {
        string name = tokens[i];
        if (i == (tokens.Length - 1))
          current[name] = (T)value;
        else {
          if (!current.ContainsKey(name))
            current[name] = new Dictionary<string, object>();
          current = current[name] as Dictionary<string, object>;
        }
      }
      if (persistent) {
        Apply(table_name);
      }
    }

    /**
     * 설정값을 조회한다.
     * 
     * \todo
        현재 json 포맺의 최상위가 배열인 경우 처리하지 못한다. 배열관련 문법을 지원하도록 해야 한다.
        query.Get<object[]>("items");
        query.Get<Dictinary<string,object>>("items[0]");
     *
     * \param path .(dot) 단위로 정의된 값 path.
     * 
     * \retrun 조회하려는 데이터 타입
     * \throw Exception
     */
    public ConvertType Get<ConvertType>(string path) {
      this.path = path;
      string[] tokens = path.Split('.');
      if (!caches.ContainsKey(tokens[0])) {
        Load<object>(tokens[0]);
      }
      object current = caches;
      ConvertType result = default(ConvertType);
      for (int i = 0; i < tokens.Length; ++i) {
        string name = tokens[i];
        if (i == (tokens.Length - 1))
          result = GetVar<ConvertType>(current as Dictionary<string, object>, name);
        else
          current = NextToken(current as Dictionary<string, object>, name);
      }
      return result;
    }

    private T GetVar<T>(Dictionary<string, object> current, string name) {
      if (!current.ContainsKey(name))
        throw new System.Exception("variable is not exist. path : " + path);
      try {
        return (T)current[name];
      }
      catch (System.InvalidCastException e) {
        throw new System.Exception("cast failed." + e.StackTrace);
      }
    }

    private Dictionary<string, object> NextToken(Dictionary<string, object> current, string name) {
      if (!current.ContainsKey(name))
        throw new System.Exception("variable is not exist. path : " + path);
      return current[name] as Dictionary<string, object>;
    }

    public void Clear(string table_name, bool persistent = false) {
      caches.Remove(table_name);
      if (persistent) {
        Apply(table_name);
      }
    }

    public void ClearAll() {
      caches.Clear();
    }

    /**
     * 설정 정보를 저장소에서 로드한다.
     *
     * \param table_name 설정 정보명 
     */
    public void Load<Type>(string table_name) {
      if (store.Exist(table_name)) {
        caches[table_name] = store.Load<Type>(table_name);
      }
      else
        caches[table_name] = new Dictionary<string, object>();
    }

    /**
     * 데이터를 를 저장소에 반영한다.
     *
     * \param table_name 설정 정보명 
     */
    public void Apply(string table_name) {
      if (caches.ContainsKey(table_name)) {
        store.Save(table_name, caches[table_name]);
      }
      else {
        store.Delete(table_name);
      }
    }

    private string path;
    private StoreBase store;
    private Dictionary<string, object> caches;
  }
}
