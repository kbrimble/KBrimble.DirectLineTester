﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Bot.Connector.DirectLine.Models;

namespace BotSpec.Client
{
    public interface IBotClient
    {
        Task StartConversation();
        Task SendMessage(string messageText, object channelData = null);
        Task SendMessage(Message message);
        Task<IEnumerable<Message>> GetMessagesFromHigherWatermark();
        Task<IEnumerable<Message>> GetMessagesFromLowerWatermark();
    }
}