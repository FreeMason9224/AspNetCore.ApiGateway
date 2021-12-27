﻿using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;

namespace Hub.Client
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var conn = new HubConnectionBuilder()
                            .WithUrl("https://localhost:5001/GatewayHub")
                            .AddNewtonsoftJsonProtocol()
                            .Build();

            await conn.StartAsync();

            await conn.InvokeAsync("SubscribeToRoute", new GatewayHubUser
            {
                Api = "chatservice",
                Key = "room",                
                ReceiveKey = "2f85e3c6-66d2-48a3-8ff7-31a65073558b",
                UserId = "JohnD"
            });

            conn.On("ReceiveMessage", new Type[] { typeof(object), typeof(object) }, (arg1, arg2) =>
            {
                return WriteToConsole(arg1);
            }, new object());

            Console.ReadLine();
        }

        static Task WriteToConsole(object[] arg1)
        {
            var jArray = JArray.Parse(arg1[0].ToString());

            Console.WriteLine($"{jArray[0]} says {jArray[1]}");
            return Task.CompletedTask;
        }

    }
}
