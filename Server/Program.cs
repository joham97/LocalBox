using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            Directory.CreateDirectory(currentDirectory + "/tmp");

            TcpListener listener = new TcpListener(8080);
            listener.Start();


            bool running = true;
            while (running)
            {
                TcpClient c = listener.AcceptTcpClient();
                Stream inOut = c.GetStream();

                StreamReader sr = new StreamReader(inOut);
                String line = sr.ReadLine();
                //sr.Close();

                Console.WriteLine("Got: " + line);

                if (line.Equals("list"))
                { 
                    StreamWriter sw = new StreamWriter(inOut);
                    foreach (String s in Directory.GetFiles(currentDirectory + "/tmp/"))
                    {
                        sw.WriteLine(s.Split('/').Last());
                    }
                    sw.Flush();
                    sw.Close();
                }
                else if (line.Equals("file"))
                {
                    String fileName = sr.ReadLine();
                    Console.WriteLine("New File: " + fileName);
                    FileStream fs = new FileStream(currentDirectory + "/tmp/" + fileName, FileMode.Create);
                    inOut.CopyTo(fs);
                    fs.Close();
                    inOut.Close();
                }
                else if (line.Equals("download"))
                {
                    String fileName = sr.ReadLine();
                    foreach (String s in Directory.GetFiles(currentDirectory + "/tmp/"))
                    {
                        if (s.Split('/').Last().Equals(fileName))
                        {
                            Console.WriteLine("\t" + fileName);
                            FileStream fs = new FileStream(s, FileMode.Open);
                            fs.CopyTo(inOut);
                            fs.Close();
                        }
                    }
                    inOut.Close();
                }

                c.Close();
            }

            listener.Stop();
        }
    }
}
