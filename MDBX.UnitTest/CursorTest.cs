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
                env.SetMaxDatabases(20)
                    .SetMaxReaders(128)
                    .Open(path, EnvironmentFlag.NoTLS, Convert.ToInt32("666", 8));


                using (MdbxTransaction tran = env.BeginTransaction())
                {
                    MdbxDatabase db = tran.OpenDatabase("cursor_test1", DatabaseOption.Create);
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
                    MdbxDatabase db = tran.OpenDatabase("cursor_test1");
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


        [Fact(DisplayName = "update by cursor")]
        public void Test2()
        {
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "mdbx");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            using (MdbxEnvironment env = new MdbxEnvironment())
            {
                env.SetMaxDatabases(20)
                    .SetMaxReaders(128)
                    .Open(path, EnvironmentFlag.NoTLS, Convert.ToInt32("666", 8));


                using (MdbxTransaction tran = env.BeginTransaction())
                {
                    MdbxDatabase db = tran.OpenDatabase("cursor_test2", DatabaseOption.Create 
                        | DatabaseOption.IntegerKey /*opitimized for fixed size int or long key*/
                        );
                    db.Empty(); // clean this data table for test

                    // add some keys
                    for (int i = 0; i < 5; i++)
                        db.Put(i+1, (i+1).ToString());

                    tran.Commit();
                }

                using (MdbxTransaction tran = env.BeginTransaction())
                {
                    MdbxDatabase db = tran.OpenDatabase("cursor_test2");
                    using (MdbxCursor cursor = db.OpenCursor())
                    {
                        cursor.Put(2, "2a"); // update by key

                        int key = 0;
                        string value = null;
                        cursor.Get(ref key, ref value, CursorOp.Next); // move to next

                        Assert.Equal(3, key);

                        cursor.Del();  // delete current one

                        key = 0;
                        value = null;
                        cursor.Get(ref key, ref value, CursorOp.GetCurrent);
                        Assert.Equal(4, key);

                        key = 0;
                        value = null;
                        cursor.Get(ref key, ref value, CursorOp.Prev);
                        Assert.Equal(2, key);
                        Assert.Equal("2a", value);
                    }

                    tran.Commit();
                }

                env.Close();
            }
        }


        [Fact(DisplayName = "enumerate all (raw value)")]
        public void Test3()
        {
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "mdbx2");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            using (MdbxEnvironment env = new MdbxEnvironment())
            {
                EnvironmentFlag flags = EnvironmentFlag.NoTLS |
                    EnvironmentFlag.NoMetaSync |
                    EnvironmentFlag.Coalesce |
                    EnvironmentFlag.LifoReclaim;
                env.SetMaxDatabases(20)
                    .SetMaxReaders(128)
                    .SetMapSize(10485760*10)
                    .Open(path, flags, Convert.ToInt32("666", 8));


                DatabaseOption option = DatabaseOption.Create | DatabaseOption.IntegerKey;
                // add some values
                using (MdbxTransaction tran = env.BeginTransaction())
                {
                    MdbxDatabase db = tran.OpenDatabase("cursor_test3", option);

                    for ( long i = 0; i < 1000000; i++)
                    {
                        db.Put(i, Guid.NewGuid().ToByteArray());
                    }

                    tran.Commit();
                }

                using (MdbxTransaction tran = env.BeginTransaction(TransactionOption.ReadOnly))
                {
                    MdbxDatabase db = tran.OpenDatabase("cursor_test3", option);
                    using (MdbxCursor cursor = db.OpenCursor())
                    {
                        long key = 0;
                        byte[] value = null;
                        cursor.Get(ref key, ref value, CursorOp.First);

                        long index = 0;
                        Assert.Equal(index, key);

                        key = 0;
                        value = null;
                        while (cursor.Get(ref key, ref value, CursorOp.Next))
                        {
                            index++;
                            Assert.Equal(index, key);
                        }
                    }
                }

                env.Close();
            }
        }


    }
}
