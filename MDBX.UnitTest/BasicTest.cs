using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Runtime.Serialization;
using System.Threading.Tasks;

using Xunit;


namespace MDBX.UnitTest
{
    public class BasicTest
    {
        [Fact(DisplayName = "put / set / delete single key (strong type)")]
        public void Test1()
        {
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "mdbx");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            using (MdbxEnvironment env = new MdbxEnvironment())
            {
                env.SetMaxDatabases(10) /* allow us to use a different db for testing */
                   .Open(path, EnvironmentFlag.NoTLS, Convert.ToInt32("666", 8));

                DatabaseOption option = DatabaseOption.Create /* needed to create a new db if not exists */
                    | DatabaseOption.IntegerKey/* opitimized for fixed key */;

                // mdbx_put
                using (MdbxTransaction tran = env.BeginTransaction())
                {
                    MdbxDatabase db = tran.OpenDatabase("basic_op_test", option);
                    db.Put(10L, "ten");
                    db.Put(1000L, "thousand");
                    db.Put(1000000000L, "billion");
                    db.Put(1000000L, "million");
                    db.Put(100L, "hundred");
                    db.Put(1L, "one");
                    tran.Commit();
                }


                // mdbx_get
                using (MdbxTransaction tran = env.BeginTransaction(TransactionOption.ReadOnly))
                {
                    MdbxDatabase db = tran.OpenDatabase("basic_op_test", option);

                    string text = db.Get<long, string>(1000000L);
                    Assert.NotNull(text);
                    Assert.Equal("million", text);
                }

                // mdbx_del
                using (MdbxTransaction tran = env.BeginTransaction())
                {
                    MdbxDatabase db = tran.OpenDatabase("basic_op_test", option);
                    bool deleted = db.Del(100L);
                    Assert.True(deleted);
                    deleted = db.Del(100L);
                    Assert.False(deleted);
                    tran.Commit();
                }


                // mdbx_get
                using (MdbxTransaction tran = env.BeginTransaction(TransactionOption.ReadOnly))
                {
                    MdbxDatabase db = tran.OpenDatabase("basic_op_test", option);

                    string text = db.Get<long, string>(100L);
                    Assert.Null(text);
                }

                env.Close();
            }
        }

        [Fact(DisplayName = "put / set / delete single key (raw key)")]
        public void Test2()
        {
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "mdbx");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            using (MdbxEnvironment env = new MdbxEnvironment())
            {
                env.Open(path, EnvironmentFlag.NoTLS, Convert.ToInt32("666", 8));

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

        

        [Fact(DisplayName = "put / set / delete single key (custom serializer)")]
        public void Test3()
        {
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "mdbx");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            // register serializer for our custom type
            SerializerRegistry.Register(new BasicTest3PayloadSerializer());

            using (MdbxEnvironment env = new MdbxEnvironment())
            {
                env.Open(path, EnvironmentFlag.NoTLS, Convert.ToInt32("666", 8));


                // mdbx_put
                using (MdbxTransaction tran = env.BeginTransaction())
                {
                    MdbxDatabase db = tran.OpenDatabase();
                    db.Put("ana_key", new BasicTest3Payload() { Person = "Ana", Age = 50 } );
                    tran.Commit();
                }


                // mdbx_get
                using (MdbxTransaction tran = env.BeginTransaction(TransactionOption.ReadOnly))
                {
                    MdbxDatabase db = tran.OpenDatabase();

                    BasicTest3Payload payload = db.Get<string, BasicTest3Payload>("ana_key");
                    Assert.NotNull(payload);
                    Assert.Equal("Ana", payload.Person);
                    Assert.Equal(50, payload.Age);
                }



                env.Close();
            }
        }



        [Fact(DisplayName = "put / set single key (raw value)")]
        public void Test4()
        {
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "mdbx");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            using (MdbxEnvironment env = new MdbxEnvironment())
            {
                env.Open(path, EnvironmentFlag.NoTLS, Convert.ToInt32("666", 8));

                string key = Guid.NewGuid().ToString("N"); // some key
                byte[] value = Encoding.UTF8.GetBytes(Guid.NewGuid().ToString()); // some value in bytes


                // mdbx_get
                using (MdbxTransaction tran = env.BeginTransaction(TransactionOption.ReadOnly))
                {
                    MdbxDatabase db = tran.OpenDatabase();

                    byte[] getBytes = db.Get(key);
                    Assert.Null(getBytes);
                }


                // mdbx_put
                using (MdbxTransaction tran = env.BeginTransaction())
                {
                    MdbxDatabase db = tran.OpenDatabase();
                    db.Put(key, value);
                    tran.Commit();
                }


                // mdbx_get
                using (MdbxTransaction tran = env.BeginTransaction(TransactionOption.ReadOnly))
                {
                    MdbxDatabase db = tran.OpenDatabase();

                    byte[] getBytes = db.Get(key);
                    Assert.NotNull(getBytes);
                    Assert.Equal(value.Length, getBytes.Length);
                    Assert.Equal(value, getBytes);
                }


                env.Close();
            }
        }

    }
}
