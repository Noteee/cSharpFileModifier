using System.Diagnostics;

namespace SanityArchiver
{
    public interface ICompress
    {
        void doCompression(string source, string destination);
    }
}