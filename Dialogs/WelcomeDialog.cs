namespace KevinBaconBot.Dialogs
{
    using AdaptiveCards;
    using Microsoft.Bot.Builder.Dialogs;
    using Microsoft.Bot.Connector;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public class WelcomeDialog : IDialog<object>
    {

        public async Task StartAsync(IDialogContext context)
        {
            var msg = context.MakeMessage();
            var gameDialog = new PromptButtonsDialog();
            await context.Forward(gameDialog, AfterWelcome, msg, CancellationToken.None);
        }

        public async Task AfterWelcome(IDialogContext context, IAwaitable<object> argument)
        {
            context.Done<string>(null);
        }

        private Activity WelcomeMessageReplyCreation(Activity activity)
        {
            var stateClient = activity.GetStateClient();
            stateClient.BotState.DeleteStateForUser(activity.ChannelId, activity.From.Id);
            //setUserDataValue(activity, "WelcomeMessageSent", true);

            var reply = activity.CreateReply();
            ApplyWelcomeMessageAsync(activity, reply);
            return reply;
        }

        public static async Task ApplyWelcomeMessageAsync(Activity activity, IMessageActivity reply)
        {
            string name = string.Empty;
            var namesToIgnore = new[] { "user", "you" };

            if (activity != null && activity.From != null && !string.IsNullOrWhiteSpace(activity.From.Name)
                && !namesToIgnore.Contains(activity.From.Name.ToLower()))
            {
                name = string.Concat(" ", activity.From.Name.Split(' ')[0]);
            }

            // Testing Adaptive Cards
            var connector = new ConnectorClient(new Uri(activity.ServiceUrl));

            Activity replyToConversation = activity.CreateReply("Should go to conversation");
            replyToConversation.Attachments = new List<Attachment>();

            AdaptiveCard card = new AdaptiveCard();

            // Specify speech for the card.
            card.Speak = "<s>Your  meeting about \"Adaptive Card design session\"<break strength='weak'/> is starting at 12:30pm</s><s>Do you want to snooze <break strength='weak'/> or do you want to send a late notification to the attendees?</s>";

            // Add text to the card.
            card.Body.Add(new TextBlock()
            {
                Text = $"Hello {name}",
                Size = TextSize.Large,
                Weight = TextWeight.Bolder
            });

            // Add text to the card.
            card.Body.Add(new TextBlock()
            {
                Text = "Welcome to the 6 degrees of Kevin Bacon Bot game.  I'll start by giving you an actor - you need to select the answer that will get you close to me, Kevin Bacon.  Game on!",
                Wrap = true
            });

            card.Body.Add(new Image()
            {
                Url = "https://upload.wikimedia.org/wikipedia/commons/3/33/Kevin_Bacon_2_SDCC_2014.jpg",
                Size = ImageSize.Large
            });

            // Add buttons to the card.
            card.Actions.Add(new SubmitAction()
            {
                Title = "Game On",
                Data = "Game On"
            });


            // Create the attachment.
            Attachment attachment = new Attachment()
            {
                ContentType = AdaptiveCard.ContentType,
                Content = card
            };

            replyToConversation.Attachments.Add(attachment);
            reply.Attachments.Add(attachment);

        }

        private IMessageActivity createReplyWithButtons(string title, IList<CardAction> buttons, IDialogContext context)
        {
            var card = new HeroCard
            {
                Text = title,
                Buttons = buttons
            };

            var reply = context.MakeMessage();
            reply.AttachmentLayout = AttachmentLayoutTypes.List;
            reply.Attachments = new List<Attachment> { card.ToAttachment() };

            return reply;
        }
    }
}
