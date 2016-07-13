using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Neutrino.Core.Util {
    public static class FileStreamEx {
        public static FileStream OpenWithoutLock(string path) {
            return new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite, 4096, true);
        }
    }
}
