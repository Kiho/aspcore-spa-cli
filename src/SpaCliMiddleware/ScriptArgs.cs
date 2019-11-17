using System;
using System.Collections.Generic;
using System.Text;

namespace SpaCliMiddleware
{
    public class ScriptArgs
    {
        public ScriptArgs(string name, int port, string regex)
        {
            NpmScriptName = name;
            PortNumber = port;
            Regex = regex;
        }

        public string NpmScriptName { get; private set; }
        public int PortNumber { get; private set; }
        public string Regex { get; private set; }
    }
}
