using System;

namespace Sharpen.IO
{
    public interface IFilenameFilter
    {
        bool Accept(Sharpen.IO.File dir, String name);
    }
}

