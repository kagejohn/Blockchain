using System;
using System.Collections.Generic;
using System.Text;

namespace Blockchain
{
    class Peer
    {
        public string Ip { get; set; }
        public DateTime LastSignOfLife { get; set; }
    }
}
