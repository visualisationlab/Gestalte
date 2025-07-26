using System;

namespace Examples.Factory101.Scripts
{
    public struct OracleResponse
    {
        public string guid;
        public Action<string> callback;
    }
}