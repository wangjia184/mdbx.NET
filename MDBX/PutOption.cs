using System;
using System.Collections.Generic;
using System.Text;

namespace MDBX
{
    using Interop;

    [Flags]
    public enum PutOption : int
    {
        Unspecific = 0,

        /// <summary>
        /// Enter the new key/data pair only if it does not already appear in the
        /// database. This flag may only be specified if the database was opened
        /// with MDBX_DUPSORT. The function will return MDBX_KEYEXIST if the
        /// key/data pair already appears in the database.
        /// </summary>
        NoDupData = Constant.MDBX_NODUPDATA,

        /// <summary>
        /// Enter the new key/data pair only if the key does not already appear
        /// in the database. The function will return MDBX_KEYEXIST if the key
        /// already appears in the database, even if the database supports
        /// duplicates (MDBX_DUPSORT).
        /// </summary>
        NoOverwrite = Constant.MDBX_NOOVERWRITE,

        /// <summary>
        /// Reserve space for data of the given size, but don't copy the given
        /// data. Instead, return a pointer to the reserved space, which the
        /// caller can fill in later - before the next update operation or the
        /// transaction ends. This saves an extra memcpy if the data is being
        /// generated later. This flag must not be specified if the database
        /// was opened with MDBX_DUPSORT.
        /// </summary>
        Reserve = Constant.MDBX_RESERVE,

        /// <summary>
        /// Append the given key/data pair to the end of the database. No key
        /// comparisons are performed. This option allows fast bulk loading when
        /// keys are already known to be in the correct order. Loading unsorted
        /// keys with this flag will cause a MDBX_KEYEXIST error.
        /// </summary>
        Append = Constant.MDBX_APPEND,

        /// <summary>
        /// Same as Append, but for sorted dup data.
        /// </summary>
        AppendDup = Constant.MDBX_APPENDDUP,

        /// <summary>
        /// Store multiple contiguous data elements in a single request. This flag
        /// may only be specified if the database was opened with MDBX_DUPFIXED.
        /// The data argument must be an array of two MDBX_vals. The iov_len of the
        /// first MDBX_val must be the size of a single data element. The iov_base
        /// of the first MDBX_val must point to the beginning of the array of
        /// contiguous data elements. The iov_len of the second MDBX_val must be
        /// the count of the number of data elements to store. On return this
        /// field will be set to the count of the number of elements actually
        /// written. The iov_base of the second MDBX_val is unused.
        /// </summary>
        Multiple = Constant.MDBX_MULTIPLE,
    }
}
