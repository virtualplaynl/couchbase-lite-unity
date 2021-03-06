﻿// 
// AndroidDefaultLogger.cs
// 
// Copyright (c) 2017 Couchbase, Inc All rights reserved.
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
// http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// 
using Couchbase.Lite.DI;
using Couchbase.Lite.Logging;

using UnityEngine;

namespace Couchbase.Lite.Support
{
    internal sealed class AndroidDefaultLogger : DI.ILogger
    {
        #region ILogger

        public void Log(LogLevel logLevel, string category, string message)
        {
            switch(logLevel) {
                case LogLevel.Error:
                    Debug.LogError($"{category} {message}");
                    break;
                case LogLevel.Warning:
                    Debug.LogWarning($"{category} {message}");
                    break;
                default:
                    Debug.Log($"{category} {message}");
                    break;
            }
        }

        #endregion
    }
}
