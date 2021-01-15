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
    public static class Program
    {
        public static readonly TelegramBotClient bot = new TelegramBotClient("1529839504:AAFJcEFZWe5HN_MSRq647WivrTKvAxzUPNg");
        public static readonly string path = Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.FullName, @"phones.txt");

        public static List<string> roles = new List<string> { "Godfather", "Mozakere Konande", "Mafiasade1", "Mafiasade2", "Shahrvand", "Karagah", "doktor", "Khabarnegar Oskol" };

        public static Dictionary<string, string> dicmafias = new Dictionary<string, string>();
        public static int mafiaCount = 4;
        public static int UserCounter = 0;
        public static int UserLimited = 12;
        public static List<string> UserRoles = new List<string>();
        public static Dictionary<string, string> dicroles = new Dictionary<string, string>();
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
        public static bool AddChatIdIfNotExist(string chatid)
        {
            if (!File.Exists(path)) File.Create(path).Close();

            if (!File.ReadLines(path).Any(line => line.Contains(chatid)))
            {
                File.AppendAllText(path, chatid);
                return true;
            }

            return false;
        }
        private static void writeNewId(string chatid)
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

                if (e.Message.Type == Telegram.Bot.Types.Enums.MessageType.Text)
                {
                    string chatid = e.Message.Chat.Id.ToString();
                    //if (e.Message.Text.ToUpper() == "Start")
                    //{
                    //    await bot.SendTextMessageAsync(chatid, string.Format("Khoshamadid!Siamak:Please enter your username with '@' like @sampleuserXX  " + e.Message.Text));
                    //}
                    if (e.Message.Text.Contains("@"))
                    {
                        UserCounter++;
                        if (UserCounter > UserLimited)
                        {
                            await bot.SendTextMessageAsync(chatid, string.Format("Sorry Refigh {0} : try for next match .we have no free place for you .. " + e.Message.Text));

                        }
                        else
                        {
                            AddChatIdIfNotExist(e.Message.Chat.Id.ToString() + ";" + e.Message.Text);
                            await bot.SendTextMessageAsync(chatid, string.Format("user name {0} is submitted", e.Message.Text));
                        }
                    }

                    if (e.Message.Text.ToUpper() == "ROLE")
                    {
                        bool isDuplicate = UserRoles.Contains(chatid);

                        if (!isDuplicate)
                        {
                            int randindex = getRandomIndex();
                            string role = GetRole(chatid, randindex);
                            await bot.SendTextMessageAsync(e.Message.Chat.Id, string.Format("role:{0} Lets GO!", role));
                            if (randindex >= 0 && randindex <= (mafiaCount - 1))
                            {
                                dicmafias.Add(chatid, role);
                                if (dicmafias.Count == mafiaCount)
                                {
                                    foreach (var mafiaChat in dicmafias)
                                    {
                                        var mafiaChatId = mafiaChat.Key;
                                        if (mafiaChatId != chatid)
                                        {

                                            foreach (var mafiaitem in dicmafias)
                                            {
                                                var mafiakey = mafiaitem.Key;
                                                string mafiastr = dicroles.Where(c => c.Key == mafiakey).FirstOrDefault().Value;
                                                await bot.SendTextMessageAsync(mafiaChatId, mafiastr + ":" + " in your team as");
                                            }
                                        }
                                    }
                                }
                            }
                            UserRoles.Add(chatid);
                            dicroles.Add(chatid, role);
                            if (randindex != 4)
                            {
                                roles.RemoveAt(randindex);
                            }

                        }
                        else
                        {
                            string doubleRole = dicroles.Where(c => c.Key == chatid).FirstOrDefault().Value;
                            await bot.SendTextMessageAsync(e.Message.Chat.Id, string.Format(e.Message.Chat.Id + "you have got already one role as {0}! ", doubleRole));
                        }

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

        private static string GetRole(string chatid, int randindex)
        {
            string role = roles[randindex].ToString();
            roles.RemoveAt(randindex);
            return role;
        }

        private static int getRandomIndex()
        {
            Random random = new Random();
            return random.Next(0, roles.Count);
        }

        public static T[] RemoveAt<T>(this T[] source, int index)
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
