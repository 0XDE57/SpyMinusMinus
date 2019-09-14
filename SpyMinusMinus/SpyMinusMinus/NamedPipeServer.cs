using System;
using System.IO;
using System.IO.Pipes;
using System.Text;
using System.Threading;

namespace SpyMinusMinus {
    class NamedPipeServer {

        private Thread serverThread;

        static MessageLogForm messageLog = new MessageLogForm();

        public NamedPipeServer() {
            
            messageLog.Show();

            serverThread = new Thread(ServerThread);
            serverThread.Start();
        }

        private static void ServerThread() {
            NamedPipeServerStream pipeServer = new NamedPipeServerStream("spyminuspipe", PipeDirection.InOut, 254, PipeTransmissionMode.Message, PipeOptions.Asynchronous);
            messageLog.Log("Waiting for connections....");
            pipeServer.WaitForConnection();
            messageLog.Log(string.Format("Client connected on thread[{0}].", Thread.CurrentThread.ManagedThreadId));

            //ReadMessageAsync(pipeServer);
            do {
                string msg = ReadMessage(pipeServer); 
                messageLog.Log(msg);
            } while (pipeServer.IsConnected);


            pipeServer.Close();
            messageLog.Log("pipe closed");
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
