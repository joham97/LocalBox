using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace LocalBox
{
    class Client
    {

        //private readonly IPAddress localAddr = IPAddress.Parse("172.20.45.4");
        private readonly IPAddress localAddr = IPAddress.Parse("127.0.0.1");
        private readonly int PORT = 8080;
       
        public List<String> GetList()
        {
            List<String> list = new List<String>();

            TcpClient client = new TcpClient();
            client.Connect(localAddr.MapToIPv4(), PORT);

            StreamWriter sw = new StreamWriter(client.GetStream());
            sw.WriteLine("list");
            sw.Flush();
            //sw.Close();

            StreamReader sr = new StreamReader(client.GetStream());
            String line = null;
            while ((line = sr.ReadLine()) != null)
            {
                list.Add(line);
            }
            sr.Close();

            client.Close();

            return list;
        }

        public void SendFile(String path)
        {
            TcpClient client = new TcpClient();
            client.Connect(localAddr.MapToIPv4(), PORT);

            StreamWriter sw = new StreamWriter(client.GetStream());
            sw.WriteLine("file");
            sw.WriteLine(path.Split('\\').Last());
            sw.Flush();
            FileStream fs = new FileStream(path, FileMode.Open);
            fs.CopyTo(client.GetStream());
            fs.Close();
            sw.Close();

            client.Close();
        }

        public void LoadFile(String fileName, String path)
        {
            TcpClient client = new TcpClient();
            client.Connect(localAddr.MapToIPv4(), PORT);

            StreamWriter sw = new StreamWriter(client.GetStream());
            sw.WriteLine("download");
            sw.WriteLine(fileName);
            sw.Flush();
            FileStream fs = new FileStream(path + "/" + fileName, FileMode.Create);
            client.GetStream().CopyTo(fs);
            fs.Close();
            sw.Close();

            client.Close();
        }


    }
}
