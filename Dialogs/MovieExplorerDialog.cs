using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using KevinBaconBot.Services;
using Microsoft.Bot.Connector;
using System.Diagnostics;
using KevinBaconBot.Model;
using System.Collections.Generic;
using KevinBaconBot.Util;

namespace KevinBaconBot.Dialogs
{
    [Serializable]
    public class MovieExplorerDialog : IDialog<object>
    {
        private readonly AzureSearchService searchService = new AzureSearchService();
        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync("Type in the name of the movie you are exploring for six degrees of Kevin Bacon:");
            context.Wait(MessageRecievedAsync);
        }

        public virtual async Task MessageRecievedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;
            int tries = 0;

            try
            {

                SearchResult searchResult = await searchService.SearchByMovie(message.Text);
                if (searchResult.value.Length != 0)
                {
                    List<string> actors = new List<string>();

                    foreach (Value actor in searchResult.value)
                    {
                        actors.Add($"{actor.Name}");
                    }
                    PromptDialog.Choice(context, AfterMenuSelection, actors, "Which actor is next in six degree of Kevin Bacon?");
                }

            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error when searching by name: {e}");
            }
        }

        private async Task AfterMenuSelection(IDialogContext context, IAwaitable<string> result)
        {
            var optionSelected = await result;
            //string selectedActor = optionSelected.Split('*')[0];
            //string selectedMovie = optionSelected.Split('*')[1];
            string selectedActor = optionSelected;

            try
            {

                SearchResult searchResult = await searchService.SearchByName(selectedActor);
                if (searchResult.value.Length != 0)
                {
                    if (selectedActor.Equals("Kevin Bacon"))
                    {
                        try
                        {
                            //SearchResult searchResult = await searchService.SearchByName(message.Text);

                            if (searchResult.value.Length != 0)
                            {
                                CardUtil.showHeroCard((IMessageActivity)context.Activity, searchResult);
                            }
                            else
                            {
                                await context.PostAsync($"I couldn't find any musicians in that era :0");
                            }
                        }
                        catch (Exception e)
                        {
                            Debug.WriteLine($"Error when searching for actor: {e.Message}");
                        }
                        context.Done<object>(null);
                    }
                    else
                    {
                        List<string> movies = new List<string>();
                        foreach (Value actor in searchResult.value)
                        {
                            //movies.Add($"{actor.Name}" + "*" + $"{actor.Description}");
                            movies.Add($"{actor.Era}");
                        }
                        PromptDialog.Choice(context, AfterMenuSelectionMovies, movies, "Which movie is next in six degree of Kevin Bacon?");

                    }
                }

            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error when searching by era: {e}");
            }
        }

        private async Task AfterMenuSelectionMovies(IDialogContext context, IAwaitable<string> result)
        {
            var optionSelected = await result;
            //string selectedActor = optionSelected.Split('*')[0];
            //string selectedMovie = optionSelected.Split('*')[1];
            string selectedMovie = optionSelected;

            try
            {

                SearchResult searchResult = await searchService.SearchByMovie(selectedMovie);
                if (searchResult.value.Length != 0)
                {
                    List<string> actors = new List<string>();

                    foreach (Value actor in searchResult.value)
                    {
                        actors.Add($"{actor.Name}");
                    }
                    PromptDialog.Choice(context, AfterMenuSelection, actors, "Which actor is next in six degree of Kevin Bacon?");
                }

            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error when searching by name: {e}");
            }
        }
    }
}