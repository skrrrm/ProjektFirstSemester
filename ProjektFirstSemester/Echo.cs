using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;

namespace ProjektFirstSemester
{
    class Echo
    {
        public static void Tracert()
        {
            // Variables
            string hostname = ""; // Can be url: www.dr.dk. Orr an ip address: 1.1.1.1 
            int timeout = 1000; // 1000ms or 1 second
            int maxTTL = 30; //max number of hops
            int currentTTL = 0; //used for tracking how many hops have been found.
            Stopwatch s1 = new Stopwatch(); // used for tracking delay between hops
            Stopwatch s2 = new Stopwatch(); // used for tracking the delay to end point
            byte[] buffer = new byte[32]; // tracert uses a buffer size, change accordingly to how big you want it 
            new Random().NextBytes(buffer); // using a random buffersize - max 32 bytes
            Ping ping = new Ping();

            hostname = Console.ReadLine();
            Console.WriteLine();

            {
                Console.WriteLine($"Started ICMP Trace route on {hostname}");
                for (int ttl = 1; ttl <= maxTTL; ttl++)
                {
                    currentTTL++;
                    s1.Start();
                    s2.Start();
                    PingOptions options = new PingOptions(ttl, true);
                    PingReply reply = null;
                    try
                    {
                        reply = ping.Send(hostname, timeout, buffer, options);
                    }
                    catch
                    {
                        Console.WriteLine("Error");
                        break; //the rest of the code relies on reply not being null so...
                    }
                    if (reply != null) //dont need this but anyway...
                    {
                        //the traceroute bits :)
                        if (reply.Status == IPStatus.TtlExpired)
                        {
                            //address found after yours on the way to the destination
                            Console.WriteLine($"[{ttl}] - Route: {reply.Address} - Time: {s1.ElapsedMilliseconds} ms - Total Time: {s2.ElapsedMilliseconds} ms");
                            s1.Reset();
                            continue; //continue to the other bits to find more servers
                        }
                        if (reply.Status == IPStatus.TimedOut)
                        {
                            //this would occour if it takes too long for the server to reply or if a server has the ICMP port closed (quite common for this).
                            Console.WriteLine($"Timeout on {hostname}. Continuing.");
                            continue;
                        }
                        if (reply.Status == IPStatus.Success)
                        {
                            //the ICMP packet has reached the destination (the hostname)
                            Console.WriteLine($"\nSuccessful Trace route to {hostname} in {s1.ElapsedMilliseconds} ms - Total Time: {s2.ElapsedMilliseconds} ms");
                            s1.Stop();
                            s2.Stop();
                        }
                    }
                    break;
                }
            }
            Console.ReadLine();
        }
        public static void Ding()
        {
            // variables
            string hostname = ""; // Can be url: www.dr.dk. Orr an ip address: 1.1.1.1 
            int maxPing = 4; // amount of times we want to ping the target. With more work this could be user determined.
            int currentPing = 0; // used for tracking how many times we pinged the target
            int avgTime = 0; // calculating the average delay to target
            int totalTime = 0; // used calculating the average delay to target
            Ping ping = new Ping(); // ping object
            bool error = false;
            
            
            hostname = Console.ReadLine();

            Console.WriteLine($"\n\nStarted ICMP ping on {hostname}");
            for (int i = 1; i <= maxPing; i++)
            {
                currentPing++;
                PingReply reply = null; // pingreply object
                try
                {
                    reply = ping.Send(hostname);
                    error = false;
                }
                catch
                {
                    Console.WriteLine("Error");
                    error = true;
                    break; //the rest of the code relies on reply not being null so...
                }

                if (reply != null)
                {
                    if (reply.Status == IPStatus.BadDestination || reply.Status == IPStatus.DestinationHostUnreachable || reply.Status == IPStatus.DestinationNetworkUnreachable)
                    {
                        Console.WriteLine("\tSomething went wrong.");
                        error = true;
                        break;
                    }
                    if (reply.Status == IPStatus.TimedOut)
                    {
                        //this would occour if it takes too long for the server to reply or if a server has the ICMP port closed (quite common for this).
                        Console.WriteLine($"\tTimeout on {hostname}. Trying again.");
                        error = false;
                        continue;
                    }
                    if (reply.Status == IPStatus.Success)
                    {
                        //the ICMP packet has reached the destination (the hostname)
                        Console.WriteLine($"\tSuccessful ping to {hostname} in {reply.RoundtripTime} ms");
                        totalTime += Convert.ToInt32(reply.RoundtripTime);
                        error = false;
                    }
                }
            }

            avgTime = totalTime / maxPing;

            if (error == false)
            {
                Console.WriteLine($"You average delay to desired {hostname} is {avgTime} ms.\n");
                if (avgTime == 0)
                    Console.WriteLine("Something went wrong.");
                else if (avgTime < 25)
                    Console.WriteLine($"That is quite good.");
                else if (avgTime > 25 && avgTime < 50)
                    Console.WriteLine($"That is good, but could be better.");
                else if (avgTime > 50 && avgTime < 100)
                    Console.WriteLine($"That is a little bit of delay, but could be worse.");
                else if (avgTime > 100)
                    Console.WriteLine($"The connection is a bit slow.");
                else if (avgTime > 350)
                    Console.WriteLine($"Holy moly, that is a slow connection!.");
            }
            Console.ReadLine();
        }
        public static void NetworkInfo()
        {
            Console.Clear();
            int i = 1;

            foreach (NetworkInterface networkinterface in NetworkInterface.GetAllNetworkInterfaces())
            {
                Console.WriteLine($"{i})");
                Console.WriteLine($"Hostname: ........................ {Dns.GetHostName()}");
                Console.WriteLine($"Networkinterface(name): .......... {networkinterface.Name}");
                Console.WriteLine($"MAC-address:  .................... {networkinterface.GetPhysicalAddress()}");

                foreach (UnicastIPAddressInformation IPs in networkinterface.GetIPProperties().UnicastAddresses)
                {
                    Console.WriteLine($"IP-address: ...................... {IPs.Address} / {IPs.IPv4Mask}");
                }

                foreach (GatewayIPAddressInformation gatewayaddresses in networkinterface.GetIPProperties().GatewayAddresses)
                {
                    Console.WriteLine($"Gateway: ......................... {gatewayaddresses.Address}");
                }

                IPInterfaceProperties adapterProperties = networkinterface.GetIPProperties();
                IPAddressCollection dnsServers = adapterProperties.DnsAddresses;
                if (dnsServers.Count > 0)
                {
                    foreach (IPAddress dns in dnsServers)
                    {
                        Console.WriteLine($"DNS Servers: ..................... {dns.ToString()}");
                    }
                }
                i++;
                Console.WriteLine();
            }
            Console.ReadLine();
        }
    }
}
