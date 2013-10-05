///////////////////////////////////////////////////////////////////////////////////////////////////
// AmiBroker Plug-in SDK | Copyright © 2010 by Koistya `Navin | http://code.google.com/p/amibroker/
// ------------------------------------------------------------------------------------------------
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file
// except in compliance with the License. You may obtain a copy of the License at:
// http://www.apache.org/licenses/LICENSE-2.0
// Unless required by applicable law or agreed to in writing, software distributed under the
// License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND,
// either express or implied. See the License for the specific language governing permissions and
// limitations under the License.
///////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace AmiBrokerPlugin
{
    unsafe public delegate AmiVar FunctionDelegate(int NumArgs, AmiVar* ArgsTable);

    /// <summary>
    /// FunDesc structure holds the pointer to actual user-defined function
    /// that can be called by AmiBroker. It holds also the number of array,
    /// string, float and default arguments for the function and the default values
    /// </summary>
    [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    unsafe public struct FunDesc
    {
        public FunctionDelegate Function;

        /// <summary>
        /// The number of Array arguments required
        /// </summary>
        public byte ArrayQty;

        /// <summary>
        /// The number of String arguments required
        /// </summary>
        public byte StringQty;

        /// <summary>
        /// The number of float args
        /// </summary>
        public byte FloatQty;

        /// <summary>
        /// The number of default float args
        /// </summary>
        public byte DefaultQty;

        /// <summary>
        /// The pointer to defaults table
        /// </summary>
        public float* DefaultValues;
    }
}
