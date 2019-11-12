using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace SpyMinusMinus {

    public class NamedPipeServer {

        private static List<Thread> serverThreads = new List<Thread>();

        public void SpawnThread(MessageLogForm messageForm) {
            Thread thread = new Thread(() => ServerThread(messageForm));
            serverThreads.Add(thread);

            thread.Start();
        }

        private void ServerThread(object messageForm) {
            MessageLogForm messageLog = (MessageLogForm)messageForm;
            NamedPipeServerStream pipeServer = new NamedPipeServerStream("spyminuspipe", PipeDirection.InOut, 254, PipeTransmissionMode.Message, PipeOptions.Asynchronous);
   
            messageLog.Log("Waiting for connections....");
            pipeServer.WaitForConnection();
            messageLog.Log(string.Format("Client connected on thread[{0}].", Thread.CurrentThread.ManagedThreadId));

            do {
                NativeMethods.CWPSTRUCT cwp = ReadCWPStruct(pipeServer);
                messageLog.Log(cwp);
            } while (pipeServer.IsConnected);


            pipeServer.Close();
            messageLog.Log("===pipe closed===");
        }


        private static NativeMethods.CWPSTRUCT ReadCWPStruct(NamedPipeServerStream namedPipeServer) {
            NativeMethods.CWPSTRUCT cwp;
            byte[] cwpStruct;
            byte[] messageBuffer = new byte[256];
            int bytesRead = 0;       
            
            do {
                try {
                    //get a message from the server
                    bytesRead = namedPipeServer.Read(messageBuffer, 0, messageBuffer.Length);

                    //copy message contents, parse bytes into struct
                    cwpStruct = new byte[bytesRead];
                    Array.Copy(messageBuffer, 0, cwpStruct, 0, bytesRead);
                    cwp = BufferToStruct<NativeMethods.CWPSTRUCT>(cwpStruct);
                } catch (Exception ex) {
                    Console.WriteLine("[ERROR] Could not read message:" + ex.Message);
                    Console.WriteLine(ex.StackTrace);
                    return new NativeMethods.CWPSTRUCT();
                }
            } while (!namedPipeServer.IsMessageComplete && bytesRead > 0);

            return cwp;
        }


        public static T BufferToStruct<T>(byte[] buffer) {
            //allocate handle for buffer
            GCHandle gHandle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            //arrange data from unmanaged block of memory to structure of type T
            T data = (T)Marshal.PtrToStructure(gHandle.AddrOfPinnedObject(), typeof(T));
            gHandle.Free(); //release handle

            return data;
        }


        private static string ReadString(NamedPipeServerStream namedPipeServer) {
            StringBuilder messageBuilder = new StringBuilder();
            byte[] messageBuffer = new byte[256];
            int bytesRead;

            do {
                bytesRead = namedPipeServer.Read(messageBuffer, 0, messageBuffer.Length);
                messageBuilder.Append(Encoding.UTF8.GetString(messageBuffer, 0, bytesRead));
            } while (!namedPipeServer.IsMessageComplete && bytesRead > 0);

            return messageBuilder.ToString();
        }


        private static async void ReadStringAsync(NamedPipeServerStream namedPipeServer) {
            StringBuilder messageBuilder = new StringBuilder();
            byte[] messageBuffer = new byte[256];
            int bytesRead;
            do {
                bytesRead = await namedPipeServer.ReadAsync(messageBuffer, 0, messageBuffer.Length);
                messageBuilder.Append(Encoding.UTF8.GetString(messageBuffer, 0, bytesRead));
            } while (!namedPipeServer.IsMessageComplete && bytesRead > 0);

            Console.WriteLine(messageBuilder.ToString());
        }

    }
}
