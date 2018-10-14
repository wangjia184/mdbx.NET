using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Xunit;


namespace MDBX.UnitTest
{
    public class BasicTest
    {
        [Fact(DisplayName = "put / set / delete single key (raw key)")]
        public void Test1()
        {
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "mdbx");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            using (MdbxEnvironment env = new MdbxEnvironment())
            {
                env.Open(path, EnvironmentFlag.NoTLS, 0644);

                var putBytes = Encoding.UTF8.GetBytes(Guid.NewGuid().ToString());

                // mdbx_put
                using (MdbxTransaction tran = env.BeginTransaction())
                {
                    MdbxDatabase db = tran.OpenDatabase();
                    db.Put(putBytes, putBytes);
                    tran.Commit();
                }


                // mdbx_get
                using (MdbxTransaction tran = env.BeginTransaction(TransactionOption.ReadOnly))
                {
                    MdbxDatabase db = tran.OpenDatabase();

                    byte[] getBytes = db.Get(putBytes);
                    Assert.NotNull(getBytes);
                    Assert.Equal(putBytes.Length, getBytes.Length);
                    Assert.Equal(putBytes, getBytes);
                }

                // mdbx_del
                using (MdbxTransaction tran = env.BeginTransaction())
                {
                    MdbxDatabase db = tran.OpenDatabase();
                    bool deleted = db.Del(putBytes);
                    Assert.True(deleted);
                    deleted = db.Del(putBytes);
                    Assert.False(deleted);
                    tran.Commit();
                }


                env.Close();
            }
        }

        [Fact(DisplayName = "basic cursor operation")]
        public void Test2()
        {
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "mdbx");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            using (MdbxEnvironment env = new MdbxEnvironment())
            {
                env.SetMaxDatabases(2)
                    .Open(path, EnvironmentFlag.NoTLS, 0644);

                
                using (MdbxTransaction tran = env.BeginTransaction())
                {
                    MdbxDatabase db = tran.OpenDatabase("cursor_test", DatabaseOption.Create);
                    db.Empty(); // clean this data table for test

                    string[] keys = new string[] 
                    {
                        "Q", "W", "E", "R", "T", "Y", "U", "I", "O", "P",
                       "A", "S", "D", "F", "G", "H", "J", "K", "L",
                        "Z", "X", "C", "V", "B", "N", "M"
                    };

                    // add some keys
                    foreach (string key in keys)
                        db.Put(key, key);

                    tran.Commit();
                }

                using (MdbxTransaction tran = env.BeginTransaction())
                {
                    MdbxDatabase db = tran.OpenDatabase("cursor_test");
                    using (MdbxCursor cursor = db.OpenCursor())
                    {

                    }
                }

                env.Close();
            }
        }


    }
}
