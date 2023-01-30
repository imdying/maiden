using System.Diagnostics;

namespace Raiden.Runtime
{
    public sealed class InvokeProcess
    {
        private readonly Process _process;

        public InvokeProcess(string exec)
        {
            if (string.IsNullOrWhiteSpace(exec))
            {
                throw new ArgumentNullException(exec);
            }

            Executable = exec;
            _process = new();
        }

        public string Executable 
        { 
            get;
            private set;
        }

        public int Invoke(string args, bool isVisible = false, bool newWindow = false)
        {
            int result;

            _process.StartInfo = new ProcessStartInfo()
            {
                Arguments = args,
                FileName = Executable,
                UseShellExecute = newWindow,
                CreateNoWindow = !isVisible
            };

            _process.Start();
            _process.WaitForExit();
            result = _process.ExitCode;
            _process.Dispose();

            return result;
        }
    }
}