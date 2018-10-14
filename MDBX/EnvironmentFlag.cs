using System;
using System.Collections.Generic;
using System.Text;

namespace MDBX
{
    using Interop;

    [Flags]
    public enum EnvironmentFlag : int
    {
        Unspecific = 0,

        /// <summary>
        /// By default, MDBX creates its environment in a directory whose
        /// pathname is given in path, and creates its data and lock files
        /// under that directory. With this option, path is used as-is for
        /// the database main data file. The database lock file is the path
        /// with "-lock" appended.
        /// </summary>
        NoSubDir = Constant.MDBX_NOSUBDIR,

        /// <summary>
        /// Open the environment in read-only mode. No write operations will
        /// be allowed. MDBX will still modify the lock file - except on
        /// read-only filesystems, where MDBX does not use locks.
        /// </summary>
        ReadOnly = Constant.MDBX_RDONLY,

        /// <summary>
        /// Use a writeable memory map unless MDBX_RDONLY is set. This uses fewer
        ///  mallocs but loses protection from application bugs like wild pointer
        ///  writes and other bad updates into the database.
        ///  This may be slightly faster for DBs that fit entirely in RAM,
        ///  but is slower for DBs larger than RAM.
        ///  Incompatible with nested transactions.
        ///  Do not mix processes with and without MDBX_WRITEMAP on the same
        ///  environment.  This can defeat durability (mdbx_env_sync etc).
        /// </summary>
        WriteMap = Constant.MDBX_WRITEMAP,

        /// <summary>
        ///  Flush system buffers to disk only once per transaction, omit the
        ///  metadata flush. Defer that until the system flushes files to disk,
        ///  or next non-MDBX_RDONLY commit or mdbx_env_sync(). This optimization
        ///  maintains database integrity, but a system crash may undo the last
        ///  committed transaction. I.e. it preserves the ACI (atomicity,
        ///  consistency, isolation) but not D (durability) database property.
        ///  This flag may be changed at any time using mdbx_env_set_flags().
        /// </summary>
        NoMetaSync = Constant.MDBX_NOMETASYNC,

        /// <summary>
        ///  Don't flush system buffers to disk when committing a transaction.
        ///  This optimization means a system crash can corrupt the database or
        ///  lose the last transactions if buffers are not yet flushed to disk.
        ///  The risk is governed by how often the system flushes dirty buffers
        ///  to disk and how often mdbx_env_sync() is called.  However, if the
        ///  filesystem preserves write order and the MDBX_WRITEMAP and/or
        ///  MDBX_LIFORECLAIM flags are not used, transactions exhibit ACI
        ///  (atomicity, consistency, isolation) properties and only lose D
        ///  (durability).  I.e. database integrity is maintained, but a system
        ///  crash may undo the final transactions.
        ///  
        ///  Note that (MDBX_NOSYNC | MDBX_WRITEMAP) leaves the system with no
        ///  hint for when to write transactions to disk.
        ///  Therefore the (MDBX_MAPASYNC | MDBX_WRITEMAP) may be preferable.
        ///  This flag may be changed at any time using mdbx_env_set_flags().
        /// </summary>
        NoSync = Constant.MDBX_NOSYNC,

        /// <summary>
        /// When using MDBX_WRITEMAP, use asynchronous flushes to disk. As with
        /// MDBX_NOSYNC, a system crash can then corrupt the database or lose
        /// the last transactions. Calling mdbx_env_sync() ensures on-disk
        /// database integrity until next commit. This flag may be changed at
        /// any time using mdbx_env_set_flags().
        /// </summary>
        MapAsync = Constant.MDBX_MAPASYNC,

        /// <summary>
        /// Don't use Thread-Local Storage. Tie reader locktable slots to
        /// MDBX_txn objects instead of to threads. I.e. mdbx_txn_reset() keeps
        /// the slot reseved for the MDBX_txn object. A thread may use parallel
        /// read-only transactions. A read-only transaction may span threads if
        /// the user synchronizes its use. Applications that multiplex many
        /// user threads over individual OS threads need this option. Such an
        /// application must also serialize the write transactions in an OS
        /// thread, since MDBX's write locking is unaware of the user threads.
        /// </summary>
        NoTLS = Constant.MDBX_NOTLS,

        /// <summary>
        /// Turn off readahead. Most operating systems perform readahead on 
        /// read requests by default. This option turns it off if the OS
        /// supports it. Turning it off may help random read performance
        /// when the DB is larger than RAM and system RAM is full.
        /// </summary>
        NoReadAhead = Constant.MDBX_NORDAHEAD,

        /// <summary>
        /// Don't initialize malloc'd memory before writing to unused spaces
        /// in the data file. By default, memory for pages written to the data
        /// file is obtained using malloc. While these pages may be reused in
        /// subsequent transactions, freshly malloc'd pages will be initialized
        /// to zeroes before use.This avoids persisting leftover data from other
        /// code(that used the heap and subsequently freed the memory) into the
        /// data file.Note that many other system libraries may allocate and free
        /// memory from the heap for arbitrary uses.E.g., stdio may use the heap
        /// for file I/O buffers. This initialization step has a modest performance
        /// cost so some applications may want to disable it using this flag.This
        /// option can be a problem for applications which handle sensitive data
        /// like passwords, and it makes memory checkers like Valgrind noisy. This
        /// flag is not needed with MDBX_WRITEMAP, which writes directly to the
        /// mmap instead of using malloc for pages.The initialization is also
        /// skipped if MDBX_RESERVE is used; the caller is expected to overwrite
        /// all of the memory that was reserved in that case. This flag may be
        /// changed at any time using mdbx_env_set_flags().
        /// </summary>
        NoMemInit = Constant.MDBX_NOMEMINIT,

        /// <summary>
        /// Aim to coalesce records while reclaiming FreeDB. This flag may be
        /// changed at any time using mdbx_env_set_flags().
        /// </summary>
        Coalesce = Constant.MDBX_COALESCE,

        /// <summary>
        /// LIFO policy for reclaiming FreeDB records. This significantly reduce
        /// write IPOs in case MDBX_NOSYNC with periodically checkpoints.
        /// </summary>
        LifoReclaim = Constant.MDBX_LIFORECLAIM,
    }
    
}
