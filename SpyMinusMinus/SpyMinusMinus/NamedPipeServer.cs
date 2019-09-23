﻿using System;
using System.IO;
using System.IO.Pipes;
using System.Runtime.InteropServices;
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
                //string msg = ReadString(pipeServer);
                NativeMethods.CWPSTRUCT cwp = ReadCWPStruct(pipeServer);
                messageLog.Log($"h:{cwp.hwnd, -10} m:{cwp.message, -10} w:{cwp.wParam, -10} l:{cwp.lParam, -10}");
            } while (pipeServer.IsConnected);


            pipeServer.Close();
            messageLog.Log("pipe closed");
        }


        private static string ReadString(NamedPipeServerStream namedPipeServer) {
            StringBuilder messageBuilder = new StringBuilder();
            byte[] messageBuffer = new byte[256];
            int bytesRead = 0;
            
            do {
                bytesRead = namedPipeServer.Read(messageBuffer, 0, messageBuffer.Length);             
                messageBuilder.Append(Encoding.UTF8.GetString(messageBuffer, 0, bytesRead));
            } while (!namedPipeServer.IsMessageComplete && bytesRead > 0);
            return messageBuilder.ToString();
        }


        private static NativeMethods.CWPSTRUCT ReadCWPStruct(NamedPipeServerStream namedPipeServer) {
            byte[] messageBuffer = new byte[256];
            int bytesRead;
            byte[] cwpStruct;
            NativeMethods.CWPSTRUCT cwp;
            do {
                bytesRead = namedPipeServer.Read(messageBuffer, 0, messageBuffer.Length);
                cwpStruct = new byte[bytesRead];
                Array.Copy(messageBuffer, 0, cwpStruct, 0, bytesRead);
                cwp = BufferToStruct<NativeMethods.CWPSTRUCT>(cwpStruct);
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


        private static async void ReadStringAsync(NamedPipeServerStream namedPipeServer) {
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
