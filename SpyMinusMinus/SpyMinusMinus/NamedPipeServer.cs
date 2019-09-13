using System;
using System.IO;
using System.IO.Pipes;
using System.Text;
using System.Threading;

namespace SpyMinusMinus {
    class NamedPipeServer {

        Thread serverThread;

        public NamedPipeServer() {
            serverThread = new Thread(ServerThread);
            serverThread.Start();
        }

        private static void ServerThread() {
            NamedPipeServerStream pipeServer = new NamedPipeServerStream("testpipe", PipeDirection.InOut, 1, PipeTransmissionMode.Message, PipeOptions.Asynchronous);
            Console.WriteLine("Waiting for connections....");
            pipeServer.WaitForConnection();
            Console.WriteLine("Client connected on thread[{0}].", Thread.CurrentThread.ManagedThreadId);

            //ReadMessageAsync(pipeServer);
            do {
                string thing = ReadMessage(pipeServer);
                Console.WriteLine(thing);
            } while (pipeServer.IsConnected);


            pipeServer.Close();
            Console.WriteLine("ended?");
        }

        private static string ReadMessage(NamedPipeServerStream namedPipeServer) {
            StringBuilder messageBuilder = new StringBuilder();
            byte[] messageBuffer = new byte[256];
            int bytesRead = 0;
            do {
                bytesRead = namedPipeServer.Read(messageBuffer, 0, messageBuffer.Length);
                messageBuilder.Append(Encoding.UTF8.GetString(messageBuffer, 0, bytesRead));
            } while (!namedPipeServer.IsMessageComplete && bytesRead > 0);
            return messageBuilder.ToString();
        }

        private static async void ReadMessageAsync(NamedPipeServerStream namedPipeServer) {
            StringBuilder messageBuilder = new StringBuilder();
            byte[] messageBuffer = new byte[256];
            int bytesRead = 0;
            do {
                bytesRead = await namedPipeServer.ReadAsync(messageBuffer, 0, messageBuffer.Length);
                messageBuilder.Append(Encoding.UTF8.GetString(messageBuffer, 0, bytesRead));
            } while (!namedPipeServer.IsMessageComplete && bytesRead > 0);
            Console.WriteLine(messageBuilder.ToString());
        }
    }
}
