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
    public class Program
    {
        public static readonly TelegramBotClient bot = new TelegramBotClient("1529839504:AAFJcEFZWe5HN_MSRq647WivrTKvAxzUPNg");
        public static readonly string path = Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.FullName, @"phones.txt");
        public static List<string> roles=new List<string>{"Godfather","Mozakere Konande","Mafiasade1","Mafiasade2","Shahrvand","Karagah","doktor","Khabarnegar Oskol"};
        public static List<string> mafias = new List<string>();
        public static List<int> MafiaRole = new List<int>();
        public static int mafiaCount = 4;
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
                    string chatid = e.Message.Chat.Id.ToString();
                    if (e.Message.Text.Contains("@"))
                    {
                        AddPhoneIfNotExist(e.Message.Chat.Id.ToString());
                        await bot.SendTextMessageAsync(chatid, "thanks");
                    }
                    if (e.Message.Text.ToUpper() == "PLAY")
                    {
                        AddPhoneIfNotExist(e.Message.Chat.Id.ToString());
                        await bot.SendTextMessageAsync(chatid, "thanks");
                    }
                    if (e.Message.Text.ToUpper() == "ROLE")
                    {
                        int randindex = getRandomIndex();
                        if (randindex >=0 && randindex <= (mafiaCount-1))
                        {
                            mafias.Add(chatid);
                            MafiaRole.Add(randindex);
                            if (mafias.Count == mafiaCount)
                            {
                                foreach (var item in mafias)
                                {
                                    await bot.SendTextMessageAsync(chatid, item);
                                }
                            }
                        }
                        string role =GetRole(chatid, randindex);
                      
                        await bot.SendTextMessageAsync(e.Message.Chat.Id, "Lets GO!");
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

        private static string GetRole(string chatid,int randindex)
        {
            string role = roles[randindex].ToString();
          roles.RemoveAt(randindex);
            return role;
        }

        private static int getRandomIndex( )
        {
            Random random = new Random();
            return random.Next(0, roles.Count);
        }

        public  static T[] RemoveAt<T>(this T[] source, int index)
        {
            T[] dest = new T[source.Length - 1];
            if (index > 0)
                Array.Copy(source, 0, dest, 0, index);

            if (index < source.Length - 1)
                Array.Copy(source, index + 1, dest, index, source.Length - index - 1);

            return dest;
        }
    }
}
