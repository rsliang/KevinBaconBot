using KevinBaconBot.Helpers;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace KevinBaconBot.Dialogs
{
    public class KBGameDialog : IDialog<object>
    {
        // TODO: Implement this!!
        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var msg = context.MakeMessage();
            msg.AttachmentLayout = AttachmentLayoutTypes.Carousel;

            // TODO: Get random results & the real answer
            List<string> answers = new List<string>() { "footloose", "big", "alien", "terminator", "forest gump" };

            foreach (var answer in answers)
            {
                var movieCard = await MovieDatabaseHelper.GetMovieCard(answer);
                msg.Attachments.Add(movieCard);
            }

            await context.PostAsync(msg);
            context.Done<object>(null);
        }

    }
}