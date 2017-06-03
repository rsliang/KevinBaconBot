using System;
using System.Collections.Generic;
using Microsoft.Bot.Connector;
using KevinBaconBot.Model;

namespace KevinBaconBot.Util
{
    public static class CardUtil
    {
        public static async void showHeroCard(IMessageActivity message, SearchResult searchResult)
        {
            //Make reply activity and set layout
            Activity reply = ((Activity)message).CreateReply();
            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;

            //Make each Card for each musician
            foreach (Value actor in searchResult.value)
            {
                List<CardImage> cardImages = new List<CardImage>();
                cardImages.Add(new CardImage(url: actor.imageURL));
                HeroCard card = new HeroCard()
                {
                    Title = actor.Name,
                    Subtitle = $"Movie: {actor.Era } | Search Score: {actor.searchscore}",
                    Text = actor.Description,
                    Images = cardImages
                };
                reply.Attachments.Add(card.ToAttachment());
            }

            //make connector and reply message
            ConnectorClient connector = new ConnectorClient(new Uri(reply.ServiceUrl));
            await connector.Conversations.SendToConversationAsync(reply);
        }
    }
}