using System;
using System.Collections.Generic;
using System.Text;

namespace MDBX
{

    /// <summary>
    /// Copy the definitions from https://github.com/leo-yuriev/libmdbx/blob/stable/0.1/mdbx.h
    /// Then reg-replace `\s\(\-(?<num>\d+)\)` with `= -$1;`
    /// </summary>
    public static class MdbxCode
    {
        public const int MDBX_SUCCESS = 0;
        public const int MDBX_RESULT_FALSE = 0;
        public const int MDBX_RESULT_TRUE = -1;

        /* key/data pair already exists */
        public const int MDBX_KEYEXIST = -30799;
        /* key/data pair not found (EOF) */
        public const int MDBX_NOTFOUND = -30798;
        /* Requested page not found - this usually indicates corruption */
        public const int MDBX_PAGE_NOTFOUND = -30797;
        /* Located page was wrong type */
        public const int MDBX_CORRUPTED = -30796;
        /* Update of meta page failed or environment had fatal error */
        public const int MDBX_PANIC = -30795;
        /* DB file version mismatch with libmdbx */
        public const int MDBX_VERSION_MISMATCH = -30794;
        /* File is not a valid MDBX file */
        public const int MDBX_INVALID = -30793;
        /* Environment mapsize reached */
        public const int MDBX_MAP_FULL = -30792;
        /* Environment maxdbs reached */
        public const int MDBX_DBS_FULL = -30791;
        /* Environment maxreaders reached */
        public const int MDBX_READERS_FULL = -30790;
        /* Txn has too many dirty pages */
        public const int MDBX_TXN_FULL = -30788;
        /* Cursor stack too deep - internal error */
        public const int MDBX_CURSOR_FULL = -30787;
        /* Page has not enough space - internal error */
        public const int MDBX_PAGE_FULL = -30786;
        /* Database contents grew beyond environment mapsize */
        public const int MDBX_MAP_RESIZED = -30785;
        /* Operation and DB incompatible, or DB type changed. This can mean:
         *  - The operation expects an MDBX_DUPSORT / MDBX_DUPFIXED database.
         *  - Opening a named DB when the unnamed DB has MDBX_DUPSORT/MDBX_INTEGERKEY.
         *  - Accessing a data record as a database, or vice versa.
         *  - The database was dropped and recreated with different flags. */
        public const int MDBX_INCOMPATIBLE = -30784;
        /* Invalid reuse of reader locktable slot */
        public const int MDBX_BAD_RSLOT = -30783;
        /* Transaction must abort, has a child, or is invalid */
        public const int MDBX_BAD_TXN = -30782;
        /* Unsupported size of key/DB name/data, or wrong DUPFIXED size */
        public const int MDBX_BAD_VALSIZE = -30781;
        /* The specified DBI was changed unexpectedly */
        public const int MDBX_BAD_DBI = -30780;
        /* Unexpected problem - txn should abort */
        public const int MDBX_PROBLEM = -30779;
        /* Another write transaction is running */
        public const int MDBX_BUSY = -30778;
        /* The last defined error code */
        public const int MDBX_LAST_ERRCODE = -30778;

        /* The mdbx_put() or mdbx_replace() was called for key,
            that has more that one associated value. */
        public const int MDBX_EMULTIVAL = -30421;

        /* Bad signature of a runtime object(s), this can mean:
         *  - memory corruption or double-free;
         *  - ABI version mismatch (rare case); */
        public const int MDBX_EBADSIGN = -30420;

        /* Database should be recovered, but this could NOT be done automatically
         * right now (e.g. in readonly mode and so forth). */
        public const int MDBX_WANNA_RECOVERY = -30419;

        /* The given key value is mismatched to the current cursor position,
         * when mdbx_cursor_put() called with MDBX_CURRENT option. */
        public const int MDBX_EKEYMISMATCH = -30418;

        /* Database is too large for current system,
         * e.g. could NOT be mapped into RAM. */
        public const int MDBX_TOO_LARGE = -30417;

        /* A thread has attempted to use a not owned object,
         * e.g. a transaction that started by another thread. */
        public const int MDBX_THREAD_MISMATCH = -30416;
    }
}
