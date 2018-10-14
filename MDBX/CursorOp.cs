using System;
using System.Collections.Generic;
using System.Text;

namespace MDBX
{
    public enum CursorOp
    {
        /// <summary>
        /// Position at first key/data item
        /// </summary>
        First,

        /// <summary>
        /// MDBX_DUPSORT-only: Position at first data item of current key.
        /// </summary>
        FirstDup,

        /// <summary>
        /// MDBX_DUPSORT-only: Position at key/data pair.
        /// </summary>
        GetBoth,

        /// <summary>
        /// MDBX_DUPSORT-only: position at key, nearest data.
        /// </summary>
        GetBothRange,

        /// <summary>
        /// Return key/data at current cursor position
        /// </summary>
        GetCurrent,

        /// <summary>
        /// MDBX_DUPFIXED-only: Return up to a page of duplicate
        /// data items from current cursor position.
        /// Move cursor to prepare for MDBX_NEXT_MULTIPLE.
        /// </summary>
        GetMultiple,

        /// <summary>
        /// Position at last key/data item
        /// </summary>
        Last,

        /// <summary>
        /// MDBX_DUPSORT-only: Position at last data item of current key.
        /// </summary>
        LastDup,

        /// <summary>
        /// Position at next data item 
        /// </summary>
        Next,

        /// <summary>
        /// MDBX_DUPSORT-only: Position at next data item of current key.
        /// </summary>
        NextDup,

        /// <summary>
        /// MDBX_DUPFIXED-only: Return up to a page of duplicate 
        /// data items from next cursor position.
        /// Move cursor to prepare for MDBX_NEXT_MULTIPLE.
        /// </summary>
        NextMultiple,

        /// <summary>
        /// Position at first data item of next key
        /// </summary>
        NextNoDup,

        /// <summary>
        /// Position at previous data item
        /// </summary>
        Prev,

        /// <summary>
        /// MDBX_DUPSORT-only: Position at previous data item of current key.
        /// </summary>
        PrevDup,

        /// <summary>
        /// Position at last data item of previous key
        /// </summary>
        PrevNoDup,

        /// <summary>
        /// Position at specified key
        /// </summary>
        Set,

        /// <summary>
        /// Position at specified key, return both key and data
        /// </summary>
        SetKey,

        /// <summary>
        /// Position at first key greater than or equal to specified key
        /// </summary>
        SetRange,

        /// <summary>
        /// MDBX_DUPFIXED-only: Position at previous page and return up to a page of duplicate data items.
        /// </summary>
        PrevMultiple,
    }
}
