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
using System.Runtime.InteropServices;

namespace AmiBrokerPlugin
{
    public enum Date
    {
        EodTicks = 15,
        EodMinutes = 63,
        EodHours = 31
    }

    [StructLayoutAttribute(LayoutKind.Sequential, Size = 8)]
    public struct PackedDate
    {
        public int Time;
        public int Date;
    }

    [StructLayoutAttribute(LayoutKind.Explicit, Size = 8)]
    public struct AmiDate
    {
        [FieldOffset(0)]
        public UInt64 Date;

        [FieldOffset(0)]
        public PackedDate PackDate;
    }
}
