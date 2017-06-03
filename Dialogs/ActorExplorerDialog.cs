using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using KevinBaconBot.Services;
using KevinBaconBot.Model;
using System.Diagnostics;
using KevinBaconBot.Util;
using System.Collections.Generic;
using KevinBaconBot.Helpers;
using System.Text.RegularExpressions;

namespace KevinBaconBot.Dialogs
{
    [Serializable]
    public class ActorExplorerDialog : IDialog<object>
    {
        private readonly AzureSearchService searchService = new AzureSearchService();
        private int tries = 0;
        public async Task StartAsync(IDialogContext context)
        {
            var msg = context.MakeMessage();

            // Set beginning game state
            GameState currentGame = new GameState() { IsMovieAnswer = false, NumberOfTurns = 0, PreviousAnswer = "", StartingActor = "" };
            context.ConversationData.SetValue<GameState>("GameState", currentGame);
            await context.PostAsync("Type in the name of the actor you want to link to me, Kevin Bacon, in six degrees of separation or less.  Hint: F.A. Turner");
            context.Wait(MessageRecievedAsync);
        }

        public virtual async Task MessageRecievedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;
            tries++;

            try
            {
                GameState currentGame = context.ConversationData.GetValue<GameState>("GameState");

                // Persist the users very first selection
                if (currentGame.StartingActor.Length == 0)
                {
                    currentGame.StartingActor = message.Text;
                }

                // Check if we've hit our limits for separation
                if (currentGame.NumberOfTurns > 6)
                {
                    await context.PostAsync($"You failed to link {currentGame.StartingActor} with me, Kevin Bacon, in 6 degress of separation");
                    await GameOver(context);
                    return;
                }

                if (message.Text.Equals("Kevin Bacon"))
                {
                    await context.PostAsync($"Congratulations!!!  You've link {currentGame.StartingActor} with me, Kevin Bacon, in 6 degress of separation");
                    await GameOver(context);
                    return;
                }


                if (!currentGame.IsMovieAnswer)
                {
                    SearchResult searchResult = await searchService.SearchByName(message.Text);
                    if (searchResult.value.Length != 0)
                    {
                        // Generate movie cards for each result
                        var msg = context.MakeMessage();
                        msg.AttachmentLayout = AttachmentLayoutTypes.Carousel;
                        foreach (var movie in searchResult.value)
                        {
                            if (!currentGame.PreviousAnswer.Equals(movie.Era))
                            {
                                var movieCard = await MovieDatabaseHelper.GetMovieCard(movie.Era);
                                msg.Attachments.Add(movieCard);
                            }
                        }

                        // Check if we have filtered all the results from the search out - if so game over!
                        if (msg.Attachments.Count == 0)
                        {
                            await context.PostAsync($"{message.Text} has only appeared in {currentGame.PreviousAnswer}, you can't have the same answer twice!");
                            await GameOver(context);
                            return;
                        }
                        else
                        {
                            //context.Post("changeBackground");
                            currentGame.IsMovieAnswer = true;
                            await context.PostAsync(msg);
                        }
                    }
                    // TODO: Don't think this is required
                    //else
                    //{
                    //    await context.PostAsync($"There are no further movies staring in: {message.Text}");
                    //    await GameOver(context);
                    //    return;
                    //}
                }
                else
                {
                    SearchResult searchResult = await searchService.SearchByMovie(message.Text);
                    if (searchResult.value.Length != 0)
                    {
                         // Generate actor cards for each result
                        var msg = context.MakeMessage();
                        msg.AttachmentLayout = AttachmentLayoutTypes.Carousel;
                        foreach (var actor in searchResult.value)
                        {
                            if (!currentGame.PreviousAnswer.Equals(actor.Name))
                            {
                                var actorCard = await MovieDatabaseHelper.GetActorCard(actor.Name);
                                msg.Attachments.Add(actorCard);
                            }
                        }

                        // Check if we have filtered all the results from the search out - if so game over!
                        if (msg.Attachments.Count == 0)
                        {
                            await context.PostAsync($"The movie {message.Text} only stars {currentGame.PreviousAnswer}, you can't have the same answer twice!");
                            await GameOver(context);
                            return;
                        }
                        else
                        {
                            currentGame.IsMovieAnswer = false;
                            await context.PostAsync(msg);
                        }
                    }
                    // TODO: Don't think this is required
                    else //if (searchResult.value.Length == 0)
                    {
                        await context.PostAsync($"There are no further actors found staring in: {message.Text}");
                        await GameOver(context);
                        return;
                    }
                }
                currentGame.NumberOfTurns++;
                currentGame.PreviousAnswer = message.Text;
                context.ConversationData.SetValue<GameState>("GameState", currentGame);

            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error: {e}");
            }
        }

        //private async void GameOver(IDialogContext context)
        public async Task GameOver(IDialogContext context)
        {
            await context.PostAsync("Game over!!");
            PromptDialog.Confirm(context, this.PlayAgain, "Play again?");

        }

        private async Task PlayAgain(IDialogContext context, IAwaitable<bool> result)
        {
            var response = await result;
            if (response)
            {
                await StartAsync(context);
            }
            else
            {
                context.Done<object>(null);
            }
        }
    }
}