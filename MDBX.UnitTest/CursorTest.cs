using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Xunit;


namespace MDBX.UnitTest
{
    public class CursorTest
    {
        [Fact(DisplayName = "basic cursor operation")]
        public void Test1()
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

                using (MdbxTransaction tran = env.BeginTransaction(TransactionOption.ReadOnly))
                {
                    MdbxDatabase db = tran.OpenDatabase("cursor_test");
                    using (MdbxCursor cursor = db.OpenCursor())
                    {
                        string key = null, value = null;
                        cursor.Get(ref key, ref value, CursorOp.First);

                        char c = 'A';
                        Assert.Equal(c.ToString(), key);
                        Assert.Equal(c.ToString(), value);

                        while(cursor.Get(ref key, ref value, CursorOp.Next))
                        {
                            c = (char)((int)c + 1);
                            Assert.Equal(c.ToString(), key);
                            Assert.Equal(c.ToString(), value);
                        }
                    }
                }

                env.Close();
            }
        }




    }
}
