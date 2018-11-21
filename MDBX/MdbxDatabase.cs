using System;
using System.Runtime.InteropServices;
using System.Text;

namespace MDBX
{
    using Interop;

    /// <summary>
    /// A table handle denotes the name and parameters of a table, independently
    /// of whether such a table exists.The table handle may be discarded by
    /// calling mdbx_dbi_close(). The old table handle is returned if the table
    /// was already open.The handle may only be closed once.
    /// 
    /// The table handle will be private to the current transaction until
    /// the transaction is successfully committed.If the transaction is
    /// aborted the handle will be closed automatically.
    /// After a successful commit the handle will reside in the shared
    /// environment, and may be used by other transactions.
    /// </summary>
    public class MdbxDatabase
    {

        private readonly MdbxEnvironment _env;
        private readonly MdbxTransaction _tran;
        private readonly uint _dbi;

        internal MdbxDatabase(MdbxEnvironment env, MdbxTransaction tran, uint dbi)
        {
            _env = env;
            _tran = tran;
            _dbi = dbi;
        }

        /// <summary>
        /// Close a database handle. Normally unnecessary.
        /// Closing a database handle is not necessary, but lets mdbx_dbi_open()
        /// reuse the handle value.  Usually it's better to set a bigger
        /// mdbx_env_set_maxdbs(), unless that value would be large.
        /// </summary>
        public void Close()
        {
            Dbi.Close(_env._envPtr, _dbi);
        }


        /// <summary>
        /// Drop this database
        /// </summary>
        public void Drop()
        {
            Dbi.Drop(_tran._txnPtr, _dbi, true);
        }

        /// <summary>
        /// delete all keys in this database to empty it
        /// </summary>
        public void Empty()
        {
            Dbi.Drop(_tran._txnPtr, _dbi, false);
        }

        public void Put(byte[] key, byte[] value, PutOption option = PutOption.Unspecific)
        {
            IntPtr keyPtr = Marshal.AllocHGlobal(key.Length);
            IntPtr valuePtr = Marshal.AllocHGlobal(value.Length);
            
            try
            {
                Marshal.Copy(key, 0, keyPtr, key.Length);
                Marshal.Copy(value, 0, valuePtr, value.Length);


                DbValue dbKey = new DbValue(keyPtr, key.Length);
                DbValue dbValue = new DbValue(valuePtr, value.Length);
                Dbi.Put(_tran._txnPtr, _dbi, dbKey, dbValue, option);
            }
            finally
            {
                Marshal.FreeHGlobal(keyPtr);
                Marshal.FreeHGlobal(valuePtr);
            }
        }

        public void Put<K, V>(K key, V value, PutOption option = PutOption.Unspecific)
        {
            ISerializer<K> keySerializer = SerializerRegistry.Get<K>();
            ISerializer<V> valueSerializer = SerializerRegistry.Get<V>();
            Put(keySerializer.Serialize(key), valueSerializer.Serialize(value), option);
        }


        /// <summary>
        /// Get a single key
        /// </summary>
        /// <param name="key"></param>
        /// <returns>null if key is not found</returns>
        public byte[] Get(byte[] key)
        {
            IntPtr keyPtr = Marshal.AllocHGlobal(key.Length);

            try
            {
                Marshal.Copy(key, 0, keyPtr, key.Length);
                DbValue dbKey = new DbValue(keyPtr, key.Length);
                DbValue dbValue = Dbi.Get(_tran._txnPtr, _dbi, dbKey);

                byte[] buffer = null;
                if (dbValue.Address != IntPtr.Zero && dbValue.Length >= 0)
                {
                    buffer = new byte[dbValue.Length];
                    if(dbValue.Length > 0)
                    {
                        Marshal.Copy(dbValue.Address, buffer, 0, buffer.Length);
                    }
                }

                return buffer;
            }
            catch(MdbxException ex)
            {
                if (ex.ErrorNumber == MdbxCode.MDBX_NOTFOUND)
                    return null; // key not found
                throw;
            }
            finally
            {
                Marshal.FreeHGlobal(keyPtr);
            }
        }

        /// <summary>
        /// Get a single key
        /// </summary>
        /// <typeparam name="K"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public V Get<K, V>(K key)
        {
            ISerializer<K> keySerializer = SerializerRegistry.Get<K>();
            ISerializer<V> valueSerializer = SerializerRegistry.Get<V>();
            byte[] buffer = Get(keySerializer.Serialize(key));
            if (buffer == null)
                return default(V);
            return valueSerializer.Deserialize(buffer);
        }

        /// <summary>
        /// Get a single key
        /// </summary>
        /// <typeparam name="K"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public byte[] Get<K>(K key)
        {
            return Get<K, byte[]>(key);
        }

        /// <summary>
        /// Delete a specific key
        /// </summary>
        /// <param name="key"></param>
        /// <returns>true if deleted successfully; false means not-found</returns>
        public bool Del(byte[] key)
        {
            IntPtr keyPtr = Marshal.AllocHGlobal(key.Length);

            try
            {
                Marshal.Copy(key, 0, keyPtr, key.Length);

                DbValue dbKey = new DbValue(keyPtr, key.Length);
                Dbi.Del(_tran._txnPtr, _dbi, dbKey, IntPtr.Zero);

                return true;
            }
            catch (MdbxException ex)
            {
                if (ex.ErrorNumber == MdbxCode.MDBX_NOTFOUND)
                    return false; // key not found
                throw;
            }
            finally
            {
                Marshal.FreeHGlobal(keyPtr);
            }
        }

        /// <summary>
        /// Delete a specific key
        /// </summary>
        /// <param name="key"></param>
        /// <returns>true if deleted successfully; false means not-found</returns>
        public bool Del<K>(K key)
        {
            ISerializer<K> keySerializer = SerializerRegistry.Get<K>();
            return Del(keySerializer.Serialize(key));
        }


        /// <summary>
        /// Create a cursor handle.
        /// 
        /// A cursor is associated with a specific transaction and database.
        /// A cursor cannot be used when its database handle is closed.  Nor
        /// when its transaction has ended, except with mdbx_cursor_renew().
        /// It can be discarded with mdbx_cursor_close().
        /// 
        /// A cursor must be closed explicitly always, before
        /// or after its transaction ends. It can be reused with
        /// mdbx_cursor_renew() before finally closing it.
        /// </summary>
        /// <returns></returns>
        public MdbxCursor OpenCursor()
        {
            IntPtr ptr = Cursor.Open(_tran._txnPtr, _dbi);
            return new MdbxCursor(_env, _tran, this, ptr);
        }

    }
}
