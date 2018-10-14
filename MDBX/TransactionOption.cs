using System;

namespace MDBX
{
    using Interop;

    [Flags]
    public enum TransactionOption : int
    {
        Unspecific = 0,

        /// <summary>
        /// Flush system buffers to disk only once per transaction, omit the metadata flush.
        /// Defer that until the system flushes files to disk,
        /// or next non-MDBX_RDONLY commit or mdbx_env_sync().
        /// 
        /// This optimization maintains database integrity, 
        /// but a system crash may undo the last committed transaction.
        /// I.e. it preserves the ACI (atomicity,consistency, isolation) but not D (durability) database property.
        /// This flag may be changed at any time using mdbx_env_set_flags().
        /// </summary>
        NoMetaSync = Constant.MDBX_NOMETASYNC,

        /// <summary>
        /// Don't flush system buffers to disk when committing a transaction.
        /// This optimization means a system crash can corrupt the database or 
        /// lose the last transactions if buffers are not yet flushed to disk.
        /// 
        /// The risk is governed by how often the system flushes dirty buffers
        /// to disk and how often mdbx_env_sync() is called.  However, if the
        /// filesystem preserves write order and the MDBX_WRITEMAP and/or
        /// LIFORECLAIM flags are not used, transactions exhibit ACI(atomicity, consistency, isolation)
        /// properties and only lose D(durability) 
        /// I.e. database integrity is maintained, but a system crash may undo the final transactions.
        /// 
        /// Note that (MDBX_NOSYNC | MDBX_WRITEMAP) leaves the system with no hint for when to write transactions to disk.
        /// Therefore the (MDBX_MAPASYNC | MDBX_WRITEMAP) may be preferable.
        /// This flag may be changed at any time using mdbx_env_set_flags().
        /// </summary>
        NoSync = Constant.MDBX_NOSYNC,

        /// <summary>
        /// This transaction will not perform any write operations.
        /// </summary>
        ReadOnly = Constant.MDBX_RDONLY,

        /// <summary>
        /// Do not block when starting a write transaction
        /// </summary>
        Try = Constant.MDBX_TRYTXN,
    }
}
