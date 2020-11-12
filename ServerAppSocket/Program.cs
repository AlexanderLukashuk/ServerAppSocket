using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ServerAppSocket
{
    class Program
    {
        static void Main(string[] args)
        {
            // адрес интранет сети, адрес локальный и адрес localhost (127.0.0.1)
            // 2 тип адреса узнаем через командную строку и команду ipconfig
            //var ipAddress = IPAddress.Parse("172.27.61.79");
            var ipAddress = IPAddress.Parse("127.0.0.1"); //создали ip адрес

            // создаем сокет для ip сети с транспортным протоколов TCP, работающий через потоки(постоянные соединения)
            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp); // сокет создан, но еще не работает

            try
            {
                // привязываем наш сокет к конечной точки, для будущего прослушивания
                socket.Bind(new IPEndPoint(ipAddress, 62227)); // привязали сокет к ip адресу и порту
                
                // начинаем прослушивать конечную точку на входящее соединение, при этом может держать в ожидании 10 следующих соединений
                socket.Listen(10); // очередь из 10 соединений возможна

                Console.WriteLine("Сервер запущен...");

                // бесконечно принимаем соединение и считываем из них данные
                while (true)
                {
                    Console.WriteLine("Ждем входящих соединений...");
                    var incomeSocket = socket.Accept(); // принимаем входящее соединение

                    Console.WriteLine("Новое входящее соединение...");
   
                    // начиная с буффера и заканчивая закрытием соединения будет отличаться код
                    var buffer = new byte[256];

                    do
                    {
                        // работает как Read у Stream
                        incomeSocket.Receive(buffer);

                        Console.WriteLine($"Новое сообщение: {Encoding.UTF8.GetString(buffer)}");
                    }
                    while (incomeSocket.Available > 0);


                    incomeSocket.Shutdown(SocketShutdown.Both);
                    incomeSocket.Close();

                    Console.WriteLine("Закрытие соединения...");
                }

                //socket.Close(); // закрыли соединение
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
        }
    }
}
