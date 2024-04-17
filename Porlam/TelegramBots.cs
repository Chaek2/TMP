using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Porlam
{
    public class TelegramBots
    {
        private static TelegramBotClient client;
        public static void init()
        {
            client = new TelegramBotClient("5856168872:AAFk_2WS5dz5Yi2mqHJmwDAYh-7Pdy9uXZw");
        }

        public static void SetMessage(string title, string number)
        {
            try
            {
                client.SendTextMessageAsync("-1001829968059", $"Новый проект: {title}\nКоличесвто баллов за участие : {number}").Wait();
            }
            catch { }
        }

        public static void SetMessageDocument(string doc, string name)
        {
            try
            {
                Stream stream = System.IO.File.OpenRead(doc);
                client.SendDocumentAsync("-1001829968059",
                    InputFile.FromStream(stream: stream, fileName: name + ".doc")).Wait();
            }
            catch { }
        }
    }
}

