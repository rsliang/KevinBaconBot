namespace KevinBaconBot.Dialogs
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Bot.Builder.Dialogs;
    using Microsoft.Bot.Connector;
    using System.Threading;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using System.Linq;
    using System.Diagnostics;

    [Serializable]
    public class RootDialog : IDialog<object>
    {

        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var msg = await result;

            if (IsQuestion(msg.Text))
            {
                Debug.WriteLine("Initiate QnA Maker");
                var qna = new QnADialog();
                await context.Forward(qna, AfterQnA, msg, CancellationToken.None);
            }
            else
            {
                // Begin the game!
                var gameDialog = new PromptButtonsDialog();
                await context.Forward(gameDialog, AfterQnA, msg, CancellationToken.None);
                //context.Done<string>(null);
            }
        }


        //Callback, after the QnAMaker Dialog returns a result.
        public async Task AfterQnA(IDialogContext context, IAwaitable<object> argument)
        {
            context.Done<string>(null);
        }

        // Simple check if the message is a potential question.
        private bool IsQuestion(string message)
        {
            //List of common question words
            List<string> questionWords = new List<string>() { "who", "what", "why", "how", "when" };

            //Question word present in the message
            Regex questionPattern = new Regex(@"\b(" + string.Join("|", questionWords.Select(Regex.Escape).ToArray()) + @"\b)", RegexOptions.IgnoreCase);

            //Return true if a question word present, or the message ends with "?"
            if (questionPattern.IsMatch(message) || message.EndsWith("?"))
                return true;
            else
                return false;
        }
    }
}