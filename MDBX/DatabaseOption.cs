using System;

namespace MDBX
{
    using Interop;

    [Flags]
    public enum DatabaseOption : int
    {
        Unspecific = 0,

        /// <summary>
        /// Keys are strings to be compared in reverse order, from the end
        /// of the strings to the beginning. By default, Keys are treated as
        /// strings and compared from beginning to end.
        /// </summary>
        ReverseKey = Constant.MDBX_REVERSEKEY,

        /// <summary>
        /// Duplicate keys may be used in the table. Or, from another point of
        /// view, keys may have multiple data items, stored in sorted order. By
        /// default keys must be unique and may have only a single data item.
        /// </summary>
        DupSort = Constant.MDBX_DUPSORT,

        /// <summary>
        /// Keys are binary integers in native byte order, either uin32_t or
        /// uint64_t, and will be sorted as such. The keys must all be of the
        /// same size
        /// </summary>
        IntegerKey = Constant.MDBX_INTEGERKEY,

        /// <summary>
        ///  This flag may only be used in combination with MDBX_DUPSORT. This
        ///  option tells the library that the data items for this database are
        ///  all the same size, which allows further optimizations in storage and
        ///  retrieval. When all data items are the same size, the MDBX_GET_MULTIPLE,
        ///  MDBX_NEXT_MULTIPLE and MDBX_PREV_MULTIPLE cursor operations may be used
        ///  to retrieve multiple items at once.
        /// </summary>
        DupFixed = Constant.MDBX_DUPFIXED,

        /// <summary>
        ///  This option specifies that duplicate data items are binary integers,
        ///  similar to MDBX_INTEGERKEY keys.
        /// </summary>
        IntegerDup = Constant.MDBX_INTEGERDUP,

        /// <summary>
        /// This option specifies that duplicate data items should be compared as
        /// strings in reverse order (the comparison is performed in the direction
        /// from the last byte to the first).
        /// </summary>
        ReverseDup = Constant.MDBX_REVERSEDUP,

        /// <summary>
        /// Create the named database if it doesn't exist. This option is not
        /// allowed in a read-only transaction or a read-only environment.
        /// </summary>
        Create = Constant.MDBX_CREATE,
    }
}
