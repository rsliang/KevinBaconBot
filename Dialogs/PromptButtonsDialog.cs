using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System.Collections.Generic;

namespace KevinBaconBot.Dialogs
{
    [Serializable]
    //This is actually Root Dialog of this bot, but I named PromptButtons Dialog becuase I want to set similar name in node.js sample.
    public class PromptButtonsDialog : IDialog<object>
    {
        private const string MovieOption = "Movie";
        private const string ActorOption =  "Actor";

        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(this.MessageRecievedAsync);
        }

        public virtual async Task MessageRecievedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            //Show options whatever users chat
           PromptDialog.Choice(context, this.AfterMenuSelection, new List<string>() {MovieOption , ActorOption}, "How would you like to begin the 6 Degrees of Kevin Bacon game?  Start by naming either a movie or an actor.");
         }

        //After users select option, Bot call other dialogs
        private async Task AfterMenuSelection(IDialogContext context, IAwaitable<string> result)
        {
            var optionSelected = await result;
            switch(optionSelected)
            {
                case MovieOption:
                    context.Call(new MovieExplorerDialog(), ResumeAfterOptionDialog);
                    break;
                case ActorOption:
                    context.Call(new ActorExplorerDialog(), ResumeAfterOptionDialog);
                    break;
            }
        }

        //This function is called after each dialog process is done
        private async Task ResumeAfterOptionDialog(IDialogContext context,IAwaitable<object> result)
        {
            //This means  MessageRecievedAsync function of this dialog (PromptButtonsDialog) will receive users' messeges
            context.Wait(MessageRecievedAsync);
        }
    }
}