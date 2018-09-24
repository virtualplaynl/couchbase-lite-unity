using System;
using System.Net;
using Couchbase.Lite.DI;

using UnityEngine;

namespace Couchbase.Lite.Support
{
    public sealed class UnityAndroidProxy : IProxy
    {
        public IWebProxy CreateProxy(Uri destination)
        {
            //This doesn't work anymore, so proxy support (currently untested but) probably not working

            //AndroidJavaObject jo = new AndroidJavaObject("java.lang.System");
            //string proxyHost = jo.CallStatic<string>("getProperty", "http.proxyHost");
            //string proxyPort = jo.CallStatic<string>("getProperty", "http.proxyPort");

            //if(string.IsNullOrEmpty(proxyHost) == false && string.IsNullOrEmpty(proxyPort) == false) {
            //    return new WebProxy(proxyHost, int.Parse(proxyPort));
            //}

            return null;
        }
    }
}