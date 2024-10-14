using System.Net.Sockets;
using System.Windows.Forms;
using System.Net;
using System.Drawing;
using System.Drawing.Imaging;


var ip = IPAddress.Parse("192.168.100.115");
var port = 27001;
var endPoint = new IPEndPoint(ip, port);

using var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

try
{
    socket.Connect(endPoint);
    if (socket.Connected)
    {
        while (true)
        {
            byte[] screenData = Screenshot();
            socket.Send(screenData);
            Thread.Sleep(10000);
        }
    }
}
catch(Exception ex)
{
    Console.WriteLine(ex.Message);
}

static byte[] Screenshot()
{
    using Bitmap map = new Bitmap(ScreenWidth(), ScreenHeight());
    using Graphics g = Graphics.FromImage(map);
    g.CopyFromScreen(0, 0, 0, 0, map.Size);

    using MemoryStream memory = new MemoryStream();
    map.Save(memory, ImageFormat.Jpeg);

    return memory.ToArray();
}

static int ScreenWidth() => Screen.PrimaryScreen.Bounds.Width;
static int ScreenHeight() => Screen.PrimaryScreen.Bounds.Height;




