using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace ConsoleApp
{
    class Program
    {
        public static readonly TelegramBotClient bot = new TelegramBotClient("1529839504:AAFJcEFZWe5HN_MSRq647WivrTKvAxzUPNg");
        public static readonly string path = Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.FullName, @"phones.txt");
        public static readonly string[] roles={"Godfather","Mozakere Konande","Mafiasade1","Mafiasade2","Shahrvand"};


        static void Main(string[] args)
        {
            //https://core.telegram.org/bots/api
            //http://t.me/MafSiaBot
            //1529839504:AAFJcEFZWe5HN_MSRq647WivrTKvAxzUPNg
            bot.OnMessage += Bot_OnMessage;
            bot.OnMessageEdited += Bot_OnMessage;
            bot.StartReceiving();
            Console.ReadLine();
            bot.StopReceiving();
            String line;
        }
        private static void clearFile()
        {
            try
            {
                File.WriteAllText(path, String.Empty);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
            finally
            {
                Console.WriteLine("Executing finally block.");
            }
        }
        protected static bool AddPhoneIfNotExist( string chatid)
        {
            if (!File.Exists(path)) File.Create(path).Close();

            if (!File.ReadLines(path).Any(line => line.Contains(chatid)))
            {
                File.AppendAllText(path, chatid);
                return true;
            }

            return false;
        }
        private static void writeNewId( string chatid)
        {
            try
            {
                File.AppendAllText(path,
                           chatid + Environment.NewLine);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
            finally
            {
                Console.WriteLine("Executing finally block.");
            }
        }

        private async static void Bot_OnMessage(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            try
            {


                List<string> roles = new List<string>();
                List<string> players = new List<string>();
                if (e.Message.Type == Telegram.Bot.Types.Enums.MessageType.Text)
                {
             
                    if (e.Message.Text.ToUpper() == "PLAY")
                    {
                        AddPhoneIfNotExist(e.Message.Chat.Id.ToString());
                        await bot.SendTextMessageAsync(e.Message.Chat.Id, "thanks");
                    }
                    if (e.Message.Text.ToUpper() == "ROLE")
                    {
                        await bot.SendTextMessageAsync(e.Message.Chat.Id, "Test Bot: Godfather");
                    }
                    if (e.Message.Text.ToUpper() == "RESET")
                    {
                        await bot.SendTextMessageAsync(e.Message.Chat.Id, "thanks");
                    }
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

    }
}
