using System;
using System.Collections.Generic;
using LibUnity.Core;
using LibUnity.Test;
using UnityEngine;

namespace LibUnity.CoreTest.Core {
  /**
   * \class ConfigTest
   *
   * \brief config 모듈 테스트 케이스
   *
   * \author Lee, Hyeon-gi
   */
  class ConfigTest : TestCase {
    [TestMethod]
    public void TestGet() {
      con.SetConfigSelector(new ResourceConfig("Resources/config"));
      Assert(con.Get<int>("test_config.var1") == 0, "get test_config.var1 is 0");
      Assert(con.Get<long>("test_config.var2") == 533157870896947200, "get test_config.var1 is 0");
      //Assert(con.Get<int>("test_config.var1") == 0, "get test_config.var1 is 0");
    }

    //[TestMethod]
    public void test_set() {
      con.SetConfigSelector(new ResourceConfig("config"));
      con.Set<long>("test_config.var1", 10000);
      Assert(con.Get<long>("test_config.var1") == 10000, "get test_config.var1 is 10000");

      con.Set<long>("test_config.var2", 10);
      Assert(con.Get<long>("test_config.var2") == 10, "get test_config.var1 is 10000");
    }

    //[TestMethod]
    public void TestGet_Failed() {
      con.SetConfigSelector(new ResourceConfig("config"));
      Assert(!IsGetSuccess<string>(con, "test_config2.var"), "exception when wrong config name");
      Assert(IsGetSuccess<long>(con, "test_config.var1"), "success when valid type");
      Assert(!IsGetSuccess<string>(con, "test_config.var1"), "exception when wrong type");
      Assert(IsGetSuccess<double>(con, "test_config.var_float"), "success when cast enable type");
      Assert(!IsGetSuccess<float>(con, "test_config.var_float"), "exception when wrong type");
      Assert(!IsGetSuccess<string>(con, "test_config.var3333"), "exception when not exist path");
    }

    override protected void SetUp() {
      con = ScriptableObject.CreateInstance<Config>();
    }

    override protected void TearDown() {
    }

    private Config con = null;

    private bool IsGetSuccess<T>(Config config, string path) {
#pragma warning disable 0219, 0168
      bool rise_exception = false;
      try {
        T var = config.Get<T>(path);
      }
      catch (System.Exception e) {
        rise_exception = true;
      }
      return !rise_exception;
#pragma warning restore 0219, 0168
    }
  }
}
