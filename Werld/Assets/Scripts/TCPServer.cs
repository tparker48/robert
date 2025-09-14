using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using Newtonsoft.Json;
using Unity.VisualScripting;

public class TCPServer : MonoBehaviour
{
    private Dictionary<int, Robert> bots;

    private int port = 3000;
    private CancellationTokenSource _cancellationTokenSource;

    public static TCPServer Instance = null;

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    async void Start()
    {
        Debug.Log("Starting Server...");
        _cancellationTokenSource = new CancellationTokenSource();
        await StartServer(_cancellationTokenSource.Token);
    }

    private void OnDestroy()
    {
        Debug.Log("Sending shutdown to server task");
        _cancellationTokenSource?.Cancel();
        _cancellationTokenSource?.Dispose();
    }

    private async Task StartServer(CancellationToken cancellationToken)
    {
        TcpListener listener = null;
        try
        {
            IPAddress localhost = new IPAddress(new byte[4] { 127, 0, 0, 1 });
            listener = new TcpListener(localhost, port);
            listener.Start();
            Debug.Log($"Server listening on port {port}...");

            while (!cancellationToken.IsCancellationRequested)
            {
                // Accept a new client connection asynchronously
                using (cancellationToken.Register(listener.Stop))
                {
                    try
                    {
                        TcpClient client = await listener.AcceptTcpClientAsync();
                        Debug.Log("Client conected!");
                        _ = HandleClientAsync(client, cancellationToken);
                    }
                    catch (ObjectDisposedException) when (cancellationToken.IsCancellationRequested)
                    {
                        Debug.Log("Server listener cancelled");
                    }
                }
            }
            Debug.Log("Stopping Server!");
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

    private async Task HandleClientAsync(TcpClient client, CancellationToken cancellationToken)
    {
        NetworkStream stream = null;
        try
        {
            stream = client.GetStream();
            byte[] buffer = new byte[1024];
            int bytesRead;
            Response response;

            while ((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length, cancellationToken)) != 0)
            {
                string recievedData = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                Command obj = JsonConvert.DeserializeObject<Command>(recievedData);

                if (obj.ship_command)
                {
                    // handle it
                    ShipCommand shipCmd = JsonConvert.DeserializeObject<ShipCommand>(recievedData);
                    response = ShipTerminal.Instance.OnCommandRecieved(recievedData);
                }
                else
                {
                    BotCommand botCmd = JsonConvert.DeserializeObject<BotCommand>(recievedData);

                    if (bots.ContainsKey(botCmd.bot_id) && bots[botCmd.bot_id] != null)
                    {
                        response = bots[botCmd.bot_id].OnCommandRecieved(recievedData);
                    }
                    else
                    {
                        response = Response.ErrorResponse($"ERROR: Could not find bot with bot_id: {botCmd.bot_id}");
                    }
                }
                string responseString = JsonConvert.SerializeObject(response);
                byte[] responseBytes = Encoding.UTF8.GetBytes(responseString);
                await stream.WriteAsync(responseBytes, 0, responseBytes.Length);
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
        finally
        {
            stream?.Close();
            client?.Close();
        }
    }

    public void RegisterNewBot(Robert bot)
    {
        if (bots == null)
        {
            bots = new Dictionary<int, Robert>();
        }

        bots.Add(bot.id, bot);
    }

    public void RemoveBot(Robert bot)
    {
        if (bots != null)
        {
            bots.Remove(bot.id);
        }
    }
}
