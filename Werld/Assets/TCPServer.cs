using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class TCPServer : MonoBehaviour
{
    private int port = 30120;
    RobertCommandProcessor commandProcessor = null;

    // Start is called before the first frame update
    async void Start()
    {
        commandProcessor = GetComponent<RobertCommandProcessor>();
        await StartServer();
    }

    void Update() { }

    private async Task StartServer()
    {
        TcpListener listener = null;
        try
        {
            IPAddress localhost = new IPAddress(new byte[4] { 127, 0, 0, 1 });
            listener = new TcpListener(localhost, port);
            listener.Start();
            Debug.Log($"Server listening on port {port}...");

            while (true)
            {
                // Accept a new client connection asynchronously
                TcpClient client = await listener.AcceptTcpClientAsync();
                Debug.Log("Client conected!");

                _ = HandleClientAsync(client);
            }
        }
        catch (SocketException e)
        {
            Debug.Log($"SocketException: {e.Message}");
        }
        finally
        {
            listener?.Stop();
        }
    }

    private async Task HandleClientAsync(TcpClient client)
    {
        NetworkStream stream = null;
        try
        {
            stream = client.GetStream();
            byte[] buffer = new byte[1024];
            int bytesRead;

            while ((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length)) != 0)
            {
                string recievedData = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                string response = commandProcessor.OnCommandRecieved(recievedData);
                byte[] response_bytes = Encoding.UTF8.GetBytes(response);
                await stream.WriteAsync(response_bytes, 0, response_bytes.Length);  
            }
        }
        catch (Exception e)
        {
            Debug.Log($"Error handling client: {e.Message}");
        }
        finally
        {
            stream?.Close();
            client?.Close();
        }
    }

    
}
