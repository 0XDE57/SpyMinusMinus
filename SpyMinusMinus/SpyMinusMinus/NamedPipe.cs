using System;
using System.IO;
using System.IO.Pipes;
using System.Text;
using System.Threading;

namespace SpyMinusMinus {
    class NamedPipe {

        Thread serverThread;

        public NamedPipe() {
            serverThread = new Thread(ServerThread);
            serverThread.Start();
        }

        private static void ServerThread() {
            NamedPipeServerStream pipeServer = new NamedPipeServerStream("testpipe", PipeDirection.InOut);

            pipeServer.WaitForConnection();
            Console.WriteLine("Client connected on thread[{0}].", Thread.CurrentThread.ManagedThreadId);

            using (StreamReader sr = new StreamReader(pipeServer)) {
                string temp;
                while ((temp = sr.ReadLine()) != null) {
                    Console.WriteLine("Received from server: {0}", temp);
                }
            }
            pipeServer.Close();
            Console.WriteLine("ended?");
        }
    }
}
