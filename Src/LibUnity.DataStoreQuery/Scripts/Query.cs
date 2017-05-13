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
    public Query(IOBase io, FormatterBase formatter) {
      caches = new Dictionary<string, object>();
      this.io = io;
      this.formatter = formatter;
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
      string config_name = tokens[0];
      if (!caches.ContainsKey(config_name)) {
        Load(config_name);
      }
      Dictionary<string, object> current = caches[config_name] as Dictionary<string, object>;
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
        Apply(config_name);
      }
    }
 
    /**
     * 설정값을 조회한다.
     *
     * \param path .(dot) 단위로 정의된 값 path.
     * 
     * \retrun 조회하려는 데이터 타입
     * \throw Exception
     */
    public T Get<T>(string path) {
      this.path = path;
      string[] tokens = path.Split('.');
      string config_name = tokens[0];
      if (!caches.ContainsKey(config_name)) {
        Load(config_name);
      }
      Dictionary<string, object> current = caches[config_name] as Dictionary<string, object>;
      T result = default(T);
      for (int i = 1; i < tokens.Length; ++i) {
        string name = tokens[i];
        if (i == (tokens.Length - 1))
          result = GetVar<T>(current, name);
        else
          current = NextToken(current, name);
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

    public void Clear(string config_name, bool persistent = false) {
      caches.Remove(config_name);
      if (persistent) {
        Apply(config_name);
      }
    }

    public void ClearAll() {
      caches.Clear();
    }

    /**
     * 설정 정보를 저장소에서 로드한다.
     *
     * \param config_name 설정 정보명 
     */
    public void Load(string config_name) {
      if (io.Has(config_name)) {
        caches[config_name] = io.Load(formatter, config_name);
      }
      else
        caches[config_name] = new Dictionary<string, object>();
    }

    /**
     * 데이터를 를 저장소에 반영한다.
     *
     * \param config_name 설정 정보명 
     */
    public void Apply(string config_name) {
      if (caches.ContainsKey(config_name)) {
        io.Save(formatter, config_name, caches[config_name]);
      }
      else {
        io.Delete(config_name);
      }
    }

    private string path;
    private IOBase io;
    private FormatterBase formatter;
    private Dictionary<string, object> caches;
  }
}
