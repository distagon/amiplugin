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

using System.Runtime.InteropServices;

namespace AmiBrokerPlugin
{
    [StructLayoutAttribute(LayoutKind.Sequential)]
    public class OptimizeParams
    {
        /// <summary>
        /// 0 - gets defaults, 1 - retrieves settings from formula (setup phase), 2 - optimization phase
        /// </summary>
        public int Mode;

        /// <summary>
        /// 0 - none (regular optimization), 1-in-sample, 2 - out of sample
        /// </summary>
        public int WalkForwardMode;

        /// <summary>
        /// Optimization engine selected - 0 means - built-in exhaustive search
        /// </summary>
        public int Engine;

        /// <summary>
        /// Number of variables to optimize
        /// </summary>
        public int Qty;

        public int LastQty;

        /// <summary>
        /// Boolean flag 1 - means optimization can continue, 0 - means aborted by pressing "Cancel" in progress dialog or other error
        /// </summary>
        public int CanContinue;

        /// <summary>
        /// Boolean flag 1 - means that AmiBroker will first check if same param set wasn't used already
        /// </summary>
        public int DuplicateCheck;

        /// <summary>
        /// And if duplicate is found it won't run backtest, instead will return previously stored value
        /// </summary>
        public int Reserved;

        /// <summary>
        /// Pointer to info text buffer (providing text display in the progress dialog)
        /// </summary>
        public string InfoText;

        /// <summary>
        /// The size (in bytes) of info text buffer
        /// </summary>
        public int InfoTextSize;

        /// <summary>
        /// Current optimization step (used for progress indicator) - automatically increased with each iteration
        /// </summary>
        public long Step;

        /// <summary>
        /// Total number of optimization steps (used for progress indicator)
        /// </summary>
        public long NumSteps;

        public double TargetCurrent;

        public double TargetBest;

        /// <summary>
        /// Optimization step in which best was achieved
        /// </summary>
        public int TargetBestStep;

        /// <summary>
        /// Parameters to optimize
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 100)]
        public OptimizeItem[] Items = new OptimizeItem[100];
    }
}
