using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace comment_analyzer
{
    interface IFileService
    {
        string ReadFile(string filePath);
        void SaveFile(string filePath, string data);
    }
}
