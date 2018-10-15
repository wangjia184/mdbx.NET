using System;
using System.Collections.Generic;
using System.Text;

namespace MDBX
{
    public enum SetOption
    {
        /// <summary>
        /// Add the flag if non-existed
        /// </summary>
        Add,

        /// <summary>
        /// Clear the flag if exists
        /// </summary>
        Clear,
    }
}
