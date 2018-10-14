using System;
using System.Collections.Generic;
using System.Text;

namespace MDBX.Interop
{
    internal class Constant
    {
        /* no environment directory */
        public const int MDBX_NOSUBDIR = 0x4000;
        /* don't fsync after commit */
        public const int MDBX_NOSYNC = 0x10000;
        /* read only */
        public const int MDBX_RDONLY = 0x20000;
        /* don't fsync metapage after commit */
        public const int MDBX_NOMETASYNC = 0x40000;
        /* use writable mmap */
        public const int MDBX_WRITEMAP = 0x80000;
        /* use asynchronous msync when MDBX_WRITEMAP is used */
        public const int MDBX_MAPASYNC = 0x100000;
        /* tie reader locktable slots to MDBX_txn objects instead of to threads */
        public const int MDBX_NOTLS = 0x200000;
        /* open DB in exclusive/monopolistic mode. */
        public const int MDBX_EXCLUSIVE = 0x400000;
        /* don't do readahead */
        public const int MDBX_NORDAHEAD = 0x800000;
        /* don't initialize malloc'd memory before writing to datafile */
        public const int MDBX_NOMEMINIT = 0x1000000;
        /* aim to coalesce FreeDB records */
        public const int MDBX_COALESCE = 0x2000000;
        /* LIFO policy for reclaiming FreeDB records */
        public const int MDBX_LIFORECLAIM = 0x4000000;
        /* make a steady-sync only on close and explicit env-sync */
        public const int MDBX_UTTERLY_NOSYNC = (MDBX_NOSYNC | MDBX_MAPASYNC);
        /* debuging option; fill/perturb released pages */
        public const int MDBX_PAGEPERTURB = 0x8000000;
        /* Do not block when starting a write transaction */
        public const int MDBX_TRYTXN = 0x10000000;

        /* use reverse string keys */
        public const int MDBX_REVERSEKEY = 0x02;
        /* use sorted duplicates */
        public const int MDBX_DUPSORT = 0x04;
        /* numeric keys in native byte order, either uint32_t or uint64_t.
         * The keys must all be of the same size. */
        public const int MDBX_INTEGERKEY = 0x08;
        /* with MDBX_DUPSORT, sorted dup items have fixed size */
        public const int MDBX_DUPFIXED = 0x10;
        /* with MDBX_DUPSORT, dups are MDBX_INTEGERKEY-style integers */
        public const int MDBX_INTEGERDUP = 0x20;
        /* with MDBX_DUPSORT, use reverse string dups */
        public const int MDBX_REVERSEDUP = 0x40;
        /* create DB if not already existing */
        public const int MDBX_CREATE = 0x40000;


        /* For put: Don't write if the key already exists. */
        public const int MDBX_NOOVERWRITE = 0x10;
        /* Only for MDBX_DUPSORT
         * For put: don't write if the key and data pair already exist.
         * For mdbx_cursor_del: remove all duplicate data items. */
        public const int MDBX_NODUPDATA = 0x20;
        /* For mdbx_cursor_put: overwrite the current key/data pair
         * MDBX allows this flag for mdbx_put() for explicit overwrite/update without
         * insertion. */
        public const int MDBX_CURRENT = 0x40;
        /* For put: Just reserve space for data, don't copy it. Return a
         * pointer to the reserved space. */
        public const int MDBX_RESERVE = 0x10000;
        /* Data is being appended, don't split full pages. */
        public const int MDBX_APPEND = 0x20000;
        /* Duplicate data is being appended, don't split full pages. */
        public const int MDBX_APPENDDUP = 0x40000;
        /* Store multiple data items in one call. Only for MDBX_DUPFIXED. */
        public const int MDBX_MULTIPLE = 0x80000;



    }
}
