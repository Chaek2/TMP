using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public static void SetMessage(string title,string number)
        {
            client.SendTextMessageAsync("1125395821", $"Новый проект: {title}\nКоличесвто баллов за участие : {number}").Wait();
        }
    }
}

