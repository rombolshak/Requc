using System;
using System.IO;

namespace Cascade.Model
{
    public class FileLogger : IDisposable
    {
        private readonly string _filename;
        private readonly FileStream _stream;
        private readonly StreamWriter _writer;

        public FileLogger(string filename)
        {
            _filename = filename;
            _stream = new FileStream(_filename, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
            _writer = new StreamWriter(_stream) {AutoFlush = true};
        }

        public void Write(string message)
        {
            _writer.Write("{0}: {1}{2}", DateTime.Now, message, Environment.NewLine);
        }

        public void Dispose()
        {
            _writer.Dispose();
        }
    }
}
