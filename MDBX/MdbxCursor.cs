using System;
using System.Runtime.InteropServices;
using System.Text;

namespace MDBX
{
    using Interop;

    public class MdbxCursor : IDisposable
    {
        private bool closed = false;

        public void Dispose()
        {
            Close();
        }

        private readonly MdbxEnvironment _env;
        private readonly MdbxTransaction _tran;
        private readonly MdbxDatabase _db;
        private readonly IntPtr _cursorPtr;

        internal MdbxCursor(MdbxEnvironment env, MdbxTransaction tran, MdbxDatabase db, IntPtr cursorPtr)
        {
            string a = Environment.StackTrace;
            _env = env;
            _tran = tran;
            _db = db;
            _cursorPtr = cursorPtr;
        }

        /// <summary>
        /// Close a cursor handle.
        /// 
        /// The cursor handle will be freed and must not be used again after this call.
        /// Its transaction must still be live if it is a write-transaction.
        /// </summary>
        void Close()
        {
            if(!closed)
            {
                closed = true;
                Cursor.Close(_cursorPtr);
            }
        }


        /// <summary>
        /// Get items from a database.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="op"></param>
        public bool Get(ref byte[] key,ref byte[] value, CursorOp op)
        {
            IntPtr keyPtr = IntPtr.Zero;
            IntPtr valuePtr = IntPtr.Zero;
            if (key != null)
                keyPtr = Marshal.AllocHGlobal(key.Length);
            if( value != null )
                valuePtr = Marshal.AllocHGlobal(key.Length);

            try
            {
                DbValue dbKey = new DbValue(keyPtr, key == null ? 0 : key.Length);
                DbValue dbValue = new DbValue(valuePtr, value == null ? 0 : value.Length);

                Cursor.Get(_cursorPtr, ref dbKey, ref dbValue, op);

                if( dbKey.Address != IntPtr.Zero)
                {
                    if (key == null || key.Length != dbKey.Length)
                        key = new byte[dbKey.Length];
                    Marshal.Copy(dbKey.Address, key, 0, key.Length);
                }
                else
                {
                    key = null;
                }
                if ( dbValue.Address != IntPtr.Zero)
                {
                    if (value == null || value.Length != dbValue.Length)
                        value = new byte[dbValue.Length];
                    Marshal.Copy(dbValue.Address, value, 0, value.Length);
                }
                else
                {
                    value = null;
                }
            }
            catch(MdbxException ex)
            {
                if (ex.ErrorNumber == MdbxCode.MDBX_NOTFOUND)
                    return false;
                throw;
            }
            finally
            {
                if (keyPtr != IntPtr.Zero)
                    Marshal.FreeHGlobal(keyPtr);
                if (valuePtr != IntPtr.Zero)
                    Marshal.FreeHGlobal(valuePtr);
            }
            return true;
        }

        /// <summary>
        /// Get items from a database.
        /// </summary>
        /// <typeparam name="K"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="op"></param>
        /// <returns>false if not found.</returns>
        public bool Get<K, V>(ref K key, ref V value, CursorOp op)
        {
            ISerializer<K> keySerializer = SerializerRegistry.Get<K>();
            ISerializer<V> valueSerializer = SerializerRegistry.Get<V>();

            byte[] keyBytes = keySerializer.Serialize(key);
            byte[] valueBytes = valueSerializer.Serialize(value);

            bool found = Get(ref keyBytes, ref valueBytes, op);
            if (found)
            {
                if (keyBytes != null)
                    key = keySerializer.Deserialize(keyBytes);
                else
                    key = default(K);

                if (valueBytes != null)
                    value = valueSerializer.Deserialize(valueBytes);
                else
                    value = default(V);
            }
            return found;
        }
    }
}
