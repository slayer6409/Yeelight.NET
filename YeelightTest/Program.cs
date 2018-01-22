﻿using System;
using System.Threading.Tasks;

using YeelightNET;

namespace YeelightTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var Devices = Yeelight.DiscoverDevices().Result;
            foreach (var device in Devices)
            {
                Console.WriteLine("Device: {0}({1}), State: {2}", device[Yeelight.DeviceProperty.Id],device[Yeelight.DeviceProperty.Name], device[Yeelight.DeviceProperty.Power]);
            }

            if (Devices.Count == 0)
            {
                Console.WriteLine("Couldn't find any devices. Make sure to enable lan control over yeelight app");
                return;
            }

            while (true)
            {
                Console.Write("\nCommand: ");
                var command = Console.ReadLine();

                string function = command.Split(' ')[0];
                int mIndex = 0;

                try
                {
                    mIndex = int.Parse(command.Split(' ')[1]);

                    if (mIndex < 0 || mIndex > Devices.Count)
                        mIndex = 0;
                }
                catch
                {
                }

                Yeelight.Device mDevice = Devices[mIndex];

                if (function == "toggle")
                {
                    mDevice.Toggle();
                }
                else if (function == "blink")
                {
                    Task.Run(() => mDevice.Blink());
                }
                else if (function == "ct")
                {
                    Console.Write("\nNew Temperature: ");
                    int temp = int.Parse(Console.ReadLine());
                    mDevice.SetColorTemperature(temp);
                }
                else if (function == "rgb")
                {
                    Console.Write("\nNew Color(R,G,B): ");
                    string[] rgb = Console.ReadLine().Split(',');
                    mDevice.SetRgbColor(int.Parse(rgb[0]), int.Parse(rgb[1]), int.Parse(rgb[2]));
                }
                else if (function == "bright")
                {
                    Console.Write("\nNew Brightness: ");
                    int bright = int.Parse(Console.ReadLine());
                    mDevice.SetBrightness(bright);
                }
                else if (function == "test")
                {
                    Console.Write("\nTesting...");
                    mDevice.Toggle().Wait().Result.SetBrightness(10).Wait().Result.SetRgbColor(0, 255, 0).Wait().Result.Blink().Result.Wait().Result.Toggle();
                }
                else if (function == "name")
                {
                    Console.Write("\nCurrent name:{0}", mDevice[Yeelight.DeviceProperty.Name]);
                    Console.Write("\nNew name: ");
                    var newName = Console.ReadLine();
                    mDevice.SetName(newName);

                }
                else if (function == "status")
                {
                    Console.WriteLine("\nStatus:");

                    Console.WriteLine("Id: {0}", mDevice[Yeelight.DeviceProperty.Id]);
                    Console.WriteLine("Location: {0}", mDevice[Yeelight.DeviceProperty.Location]);
                    Console.WriteLine("Name: {0}", mDevice[Yeelight.DeviceProperty.Name]);
                    Console.WriteLine("State: {0}", mDevice[Yeelight.DeviceProperty.Power]);
                    Console.WriteLine("Brightness: {0}", mDevice[Yeelight.DeviceProperty.Brightness]);
                    Console.WriteLine("Color Mode: {0}", mDevice[Yeelight.DeviceProperty.ColorMode]);
                    Console.WriteLine("RGB: {0},{1},{2}", mDevice[Yeelight.DeviceProperty.RGB] >> 16, (Devices[0][Yeelight.DeviceProperty.RGB] >> 8) & 255, Devices[0][Yeelight.DeviceProperty.RGB] & 255);
                    Console.WriteLine("Color Temperature: {0} K", mDevice[Yeelight.DeviceProperty.ColorTemperature]);
                }
                else if (function == "quit")
                {
                    break;
                }
            }

        }
    }
}