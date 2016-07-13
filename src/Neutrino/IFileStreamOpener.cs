﻿using System.IO;

namespace Neutrino {
    public interface IFileStreamOpener {
        Stream OpenWithoutLock(string path);
        Stream OpenForCreation(string path);
    }
}