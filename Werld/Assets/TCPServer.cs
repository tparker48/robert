using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Newtonsoft.Json;

public class TCPServer : MonoBehaviour
{
    private Dictionary<int, RobertCommandProcessor> bots;

    private int port = 30120;

    async void Start()
    {
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
            string response;

            while ((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length)) != 0)
            {
                string recievedData = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                Command obj = JsonConvert.DeserializeObject<Command>(recievedData);

                if (bots.ContainsKey(obj.bot_id) && bots[obj.bot_id] != null)
                {
                    response = bots[obj.bot_id].OnCommandRecieved(recievedData);
                }
                else
                {
                    response = $"ERROR: Could not find bot with bot_id: {obj.bot_id}";
                }

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

    public void RegisterNewBot(RobertCommandProcessor bot)
    {
        if (bots == null)
        {
            bots = new Dictionary<int, RobertCommandProcessor>();
        }

        bots.Add(bot.id, bot);
    }
}
