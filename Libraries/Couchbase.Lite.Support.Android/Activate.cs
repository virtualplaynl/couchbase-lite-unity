﻿// 
//  Activate.cs
// 
//  Copyright (c) 2017 Couchbase, Inc All rights reserved.
// 
//  Licensed under the Apache License, Version 2.0 (the "License");
//  you may not use this file except in compliance with the License.
//  You may obtain a copy of the License at
// 
//  http://www.apache.org/licenses/LICENSE-2.0
// 
//  Unless required by applicable law or agreed to in writing, software
//  distributed under the License is distributed on an "AS IS" BASIS,
//  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//  See the License for the specific language governing permissions and
//  limitations under the License.
// 

using System;

using Couchbase.Lite.DI;
using Couchbase.Lite.Logging;
using Couchbase.Lite.Util;

using LiteCore.Interop;

namespace Couchbase.Lite.Support
{
    /// <summary>
    /// Android specific support logic
    /// </summary>
    public static class Unity
    {
        #region Variables

        private static AtomicBool _Activated = false;

        #endregion

        #region Public Methods

        /// <summary>
        /// Activates the support classes for Android
        /// </summary>
        /// <param name="context">The main context of the Android application</param>
        public static void Activate()
        {
			if(_Activated.Set(true)) {
				return;
			}

            Console.WriteLine("Loading support items for Android");
            Service.Register<IDefaultDirectoryResolver>(new DefaultDirectoryResolver());
            Service.Register<IMainThreadTaskScheduler>(new MainThreadTaskScheduler());
            Service.Register<IRuntimePlatform>(new AndroidRuntimePlatform());
            Service.Register<ILiteCore>(new LiteCoreImpl());
            Service.Register<ILiteCoreRaw>(new LiteCoreRawImpl());
            Service.Register<IProxy>(new UnityAndroidProxy());
        }

        /// <summary>
		/// Enables text based logging for debugging purposes.  Log statements will
		/// be printed to logcat under the CouchbaseLite tag.
		/// </summary>
		public static void EnableTextLogging()
		{
			Log.EnableTextLogging(new AndroidDefaultLogger());
		}

        #endregion
    }
}
