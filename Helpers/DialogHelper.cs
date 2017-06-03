using AdaptiveCards;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KevinBaconBot.Helpers
{
    public static class DialogHelper
    {
        public static Attachment GetHeroCard(string title, string subtitle, string text, CardImage cardImage, CardAction cardAction)
        {
            //Activity replyToConversation = message.CreateReply("Should go to conversation");
            //replyToConversation.Attachments = new List<Attachment>();
            AdaptiveCard card = new AdaptiveCard();

            //var card = new AdaptiveCard();
            card.Body.Add(new Image()
            {
                Url = "http://someUrl.com/example.png"
            });

            // Specify speech for the card.
            card.Speak = "<s>Your  meeting about \"Adaptive Card design session\"<break strength='weak'/> is starting at 12:30pm</s><s>Do you want to snooze <break strength='weak'/> or do you want to send a late notification to the attendees?</s>";

            // Add Title text to the card.
            card.Body.Add(new TextBlock()
            {
                Text = title,
                Size = TextSize.Large,
                Weight = TextWeight.Bolder
            });

            // Add Subtitle text to the card.
            card.Body.Add(new TextBlock()
            {
                Text = subtitle,
            });

            // Add text to the card.
            card.Body.Add(new TextBlock()
            {
                Text = text
            });

            /*
            // Add list of choices to the card.
            card.Body.Add(new ChoiceSet()
            {
                Id = "snooze",
                Style = ChoiceInputStyle.Compact,
                Choices = new List<Choice>()
                {
                    new Choice() { Title = "5 minutes", Value = "5", IsSelected = true },
                    new Choice() { Title = "15 minutes", Value = "15" },
                    new Choice() { Title = "30 minutes", Value = "30" }
                }
            });
            */


            // Add buttons to the card.
            card.Actions.Add(new HttpAction()
            {
                Url = "http://foo.com",
                Title = "Snooze"
            });

            card.Actions.Add(new HttpAction()
            {
                Url = "http://foo.com",
                Title = "I'll be late"
            });

            card.Actions.Add(new HttpAction()
            {
                Url = "http://foo.com",
                Title = "Dismiss"
            });

            // Create the attachment.
            Attachment attachment = new Attachment()
            {
                ContentType = AdaptiveCard.ContentType,
                Content = card
            };

            //replyToConversation.Attachments.Add(attachment);

            //var reply = await connector.Conversations.SendToConversationAsync(replyToConversation);
            return attachment;

            /*
            card.Body.Add(new Image()
            {
                Url = "http://someUrl.com/example.png"
            });
            
            var heroCard = new HeroCard
            {
                Title = title,
                Subtitle = subtitle,
                Text = text,
                Images = new List<CardImage>() { cardImage },
                Buttons = new List<CardAction>() { cardAction },
            };
            return heroCard.ToAttachment();
            */
        }

        public static Attachment GetThumbnailCard(string title, string subtitle, string text, CardImage cardImage, CardAction cardAction)
        {

            /*
            AdaptiveCard card = new AdaptiveCard();

            //var card = new AdaptiveCard();
            card.Body.Add(new Image()
            {
                Url = "http://someUrl.com/example.png"
            });

            // Specify speech for the card.
            card.Speak = "<s>Your  meeting about \"Adaptive Card design session\"<break strength='weak'/> is starting at 12:30pm</s><s>Do you want to snooze <break strength='weak'/> or do you want to send a late notification to the attendees?</s>";

            // Add Title text to the card.
            card.Body.Add(new TextBlock()
            {
                Text = title,
                Size = TextSize.Large,
                Weight = TextWeight.Bolder
            });

            // Add Subtitle text to the card.
            card.Body.Add(new TextBlock()
            {
                Text = subtitle,
            });

            // Add text to the card.
            card.Body.Add(new TextBlock()
            {
                Text = text
            });
           
            // Add list of choices to the card.
            card.Body.Add(new ChoiceSet()
            {
                Id = "snooze",
                Style = ChoiceInputStyle.Compact,
                Choices = new List<Choice>()
                {
                    new Choice() { Title = "5 minutes", Value = "5", IsSelected = true },
                    new Choice() { Title = "15 minutes", Value = "15" },
                    new Choice() { Title = "30 minutes", Value = "30" }
                }
            });

            // Add buttons to the card.
            card.Actions.Add(new HttpAction()
            {
                Url = "http://foo.com",
                Title = "Snooze"
            });

            card.Actions.Add(new HttpAction()
            {
                Url = "http://foo.com",
                Title = "I'll be late"
            });

            card.Actions.Add(new HttpAction()
            {
                Url = "http://foo.com",
                Title = "Dismiss"
            });

            // Create the attachment.
            Attachment attachment = new Attachment()
            {
                ContentType = AdaptiveCard.ContentType,
                Content = card
            };

            //replyToConversation.Attachments.Add(attachment);

            //var reply = await connector.Conversations.SendToConversationAsync(replyToConversation);
            return attachment;
            */

            
            var heroCard = new ThumbnailCard
            {
                Title = title,
                Subtitle = subtitle,
                Text = text,
                Images = new List<CardImage>() { cardImage },
                Buttons = new List<CardAction>() { cardAction },
            };
            return heroCard.ToAttachment();
            
        }

        public static void SetUserDataValue<T>(Activity message, string key, T value)
        {
            var stateClient = message.GetStateClient();
            var userData = stateClient.BotState.GetUserData(message.ChannelId, message.From.Id);
            userData.SetProperty(key, value);
            stateClient.BotState.SetUserData(message.ChannelId, message.From.Id, userData);
        }

        public static T GetUserDataValue<T>(Activity message, string key)
        {
            var stateClient = message.GetStateClient();
            var userData = stateClient.BotState.GetUserData(message.ChannelId, message.From.Id);
            return userData.GetProperty<T>(key);
        }

        public static IList<Attachment> GetCardsAttachments()
        {
            return new List<Attachment>()
            {
                GetHeroCard(

                    "Azure Storage",

                    "Offload the heavy lifting of data center management",

                    "Store and help protect your data. Get durable, highly available data storage across the globe and pay only for what you use.",

                    new CardImage(url: "https://docs.microsoft.com/en-us/azure/storage/media/storage-introduction/storage-concepts.png"),

                    new CardAction(ActionTypes.OpenUrl, "Learn more", value: "https://azure.microsoft.com/en-us/services/storage/")),

                GetThumbnailCard(

                    "DocumentDB",

                    "Blazing fast, planet-scale NoSQL",

                    "NoSQL service for highly available, globally distributed apps—take full advantage of SQL and JavaScript over document and key-value data without the hassles of on-premises or virtual machine-based cloud database options.",

                    new CardImage(url: "https://docs.microsoft.com/en-us/azure/documentdb/media/documentdb-introduction/json-database-resources1.png"),

                    new CardAction(ActionTypes.OpenUrl, "Learn more", value: "https://azure.microsoft.com/en-us/services/documentdb/")),

                GetHeroCard(

                    "Azure Functions",

                    "Process events with a serverless code architecture",

                    "An event-based serverless compute experience to accelerate your development. It can scale based on demand and you pay only for the resources you consume.",

                    new CardImage(url: "https://azurecomcdn.azureedge.net/cvt-5daae9212bb433ad0510fbfbff44121ac7c759adc284d7a43d60dbbf2358a07a/images/page/services/functions/01-develop.png"),

                    new CardAction(ActionTypes.OpenUrl, "Learn more", value: "https://azure.microsoft.com/en-us/services/functions/")),

                GetThumbnailCard(

                    "Cognitive Services",

                    "Build powerful intelligence into your applications to enable natural and contextual interactions",

                    "Enable natural and contextual interaction with tools that augment users' experiences using the power of machine-based intelligence. Tap into an ever-growing collection of powerful artificial intelligence algorithms for vision, speech, language, and knowledge.",

                    new CardImage(url: "https://azurecomcdn.azureedge.net/cvt-68b530dac63f0ccae8466a2610289af04bdc67ee0bfbc2d5e526b8efd10af05a/images/page/services/cognitive-services/cognitive-services.png"),

                    new CardAction(ActionTypes.OpenUrl, "Learn more", value: "https://azure.microsoft.com/en-us/services/cognitive-services/")),

            };

        }
    }
}