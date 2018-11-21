# mdbx.NET

.NET bindings(.NET Standard 2.0) of [libmdbx](https://github.com/leo-yuriev/libmdbx), succeeder of LMDB(Lightning Memory-Mapped Database).

[![Build status](https://ci.appveyor.com/api/projects/status/7nyn3s6fspk8j6o2/branch/master?svg=true)](https://ci.appveyor.com/project/wangjia184/mdbx-net/branch/master) [![NuGet version](https://img.shields.io/nuget/v/mdbx.NET.svg)](https://www.nuget.org/packages/mdbx.NET/) 

### License

This project is licensed under the [Apache License, Version 2.0](http://www.apache.org/licenses/LICENSE-2.0.html).

The `libmdbx` library is not shipped with this assembly. And the assembly will load `libmdbx` from the location below according to your platform and OS.
```
/mdbx.NET.dll
/native
  ├──/windows
  │   ├──/x86/mdbx.dll
  │   ├──/x64/mdbx.dll
  │   ├──/arm/mdbx.dll
  │   └──/arm64/mdbx.dll
  ├──/linux
  │   ├──/x86/libmdbx.so
  │   ├──/x64/libmdbx.so
  │   ├──/arm/libmdbx.so
  │   └──/arm64/libmdbx.so
  └──/osx
      ├──/x86/libmdbx.so
      ├──/x64/libmdbx.so
      ├──/arm/libmdbx.so
      └──/arm64/libmdbx.so
```


## How to Use

NuGet package  is available, first install.
```
Install-Package mdbx.NET
```

Here is an example of basic operations.
```csharp
using MDBX;

using (MdbxEnvironment env = new MdbxEnvironment())
{
    env.SetMaxDatabases(10) /* allow us to use a different db for testing */
        .Open(path, EnvironmentFlag.NoTLS/* flags */, Convert.ToInt32("666", 8)/* permission */ );

    DatabaseOption option = DatabaseOption.Create /* needed to create a new db if not exists */
        | DatabaseOption.IntegerKey/* opitimized for fixed-size int/long key */;

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
}
```

Here is an example of using cursor

```csharp
using (MdbxTransaction tran = env.BeginTransaction(TransactionOption.ReadOnly))
{
    MdbxDatabase db = tran.OpenDatabase("cursor_test1");
    using (MdbxCursor cursor = db.OpenCursor())
    {
        string key = null, value = null;
        cursor.Get(ref key, ref value, CursorOp.First);
        // ...
        
        while(cursor.Get(ref key, ref value, CursorOp.Next))
        {
            // ...
        }
    }
}
```

Please check unit test for more examples
