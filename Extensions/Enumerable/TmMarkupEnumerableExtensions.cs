﻿using System.Collections.Generic;
using System.Linq;
using ImportShopBot.Extensions.String;
using Telegram.Bot.Types.ReplyMarkups;

namespace ImportShopBot.Extensions.Enumerable
{
    public static partial class EnumerableExtensions
    {
        public static ReplyKeyboardMarkup ToKeyboardColumn(this IEnumerable<string> items)
            => new ReplyKeyboardMarkup {Keyboard = new[] {items.ToKeyboardButtons()}};

        private static IEnumerable<KeyboardButton> ToKeyboardButtons(this IEnumerable<string> items)
            => items.Select(item => item.ToKeyboardButton());
    }
}