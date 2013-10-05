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
    /// <summary>
    /// FunctionTag struct holds the Name of the function and the corresponding
    /// FunDesc structure. This structure is used to define function table that is retrieved
    /// by AmiBroker via GetFunctionTable() call when AFL engine is initialized. That way
    /// new function names are added to the AFL symbol table and they become accessible.
    /// </summary>
    [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public class FunctionTag
    {
        [MarshalAs(UnmanagedType.LPStr)]
        public string Name;

        public FunDesc Description;
    }
}
