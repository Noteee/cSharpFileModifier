using System.Diagnostics;

namespace SanityArchiver
{
    public interface ICrypt
    {
        void doCrypt(string source, string destination);
    }
}