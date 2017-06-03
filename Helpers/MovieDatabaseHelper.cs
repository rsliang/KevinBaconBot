using AdaptiveCards;
using DM.MovieApi;
using DM.MovieApi.ApiResponse;
using DM.MovieApi.MovieDb.Configuration;
using DM.MovieApi.MovieDb.Movies;
using DM.MovieApi.MovieDb.People;
using Microsoft.Bot.Connector;
using Microsoft.IdentityModel.Protocols;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace KevinBaconBot.Helpers
{
    public static class MovieDatabaseHelper
    {
        public static async Task<Attachment> GetActorCard(string actorName)
        {
            try
            {
                var configApi = MovieDbFactory.Create<IApiConfigurationRequest>().Value;
                var c = await configApi.GetAsync();
                var movieApi = MovieDbFactory.Create<IApiPeopleRequest>().Value;
                ApiSearchResponse<PersonInfo> apiResponse = await movieApi.SearchByNameAsync(actorName);
                Attachment movieCard = null;
                
                // Check we got something back
                if (apiResponse.Results.Any())
                {
                    // Take the first one
                    PersonInfo info = apiResponse.Results.FirstOrDefault();
                    List<CardImage> cardImages = new List<CardImage>();

                    string actorImage = $"{c}w500{info.ProfilePath}";
                    Debug.WriteLine($"Poster path: {actorImage}");


                    /*
                    cardImages.Add(new CardImage(url: actorImage));

                    CardAction plButton = new CardAction()
                    {
                        
                        Value = actorName,
                        Type = ActionTypes.ImBack,
                        Title = "Answer"
                    };

                    List<CardAction> cardButtons = new List<CardAction>();
                    cardButtons.Add(plButton);

                    HeroCard plCard = new HeroCard()
                    {
                        //Text = info.KnownFor.ToString(),
                        Title = info.Name,
                        Images = cardImages,
                        Buttons = cardButtons,
                        Tap = new CardAction(type: ActionTypes.ImBack, title: "Answer", value: actorName)
                    };
                    movieCard = plCard.ToAttachment();
                    */

                    // Using AdaptiveCard
                    AdaptiveCard card = new AdaptiveCard();
                
                    // Add Image  to the card.
                    card.Body.Add(new Image()
                    {
                        Url = actorImage,
                        Size = ImageSize.Large
                    });

                    // Specify speech for the card.
                    card.Speak = "<s>Your  meeting about \"Adaptive Card design session\"<break strength='weak'/> is starting at 12:30pm</s><s>Do you want to snooze <break strength='weak'/> or do you want to send a late notification to the attendees?</s>";

                    // Add Title text to the card.
                    card.Body.Add(new TextBlock()
                    {
                        Text = info.Name,
                        Size = TextSize.Large,
                        Weight = TextWeight.Bolder
                    });

                    // Add buttons to the card.
                    card.Actions.Add(new SubmitAction()
                    {
                        Title = actorName,
                        Data = actorName

                    });                

                    // Create the attachment.
                    movieCard = new Attachment()
                    {
                        ContentType = AdaptiveCard.ContentType,
                        Content = card
                    };
                   
                }
                return movieCard;

            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        public static async Task<Attachment> GetMovieCard(string movieName)
        {
            try
            {
                var configApi = MovieDbFactory.Create<IApiConfigurationRequest>().Value;
                var c = await configApi.GetAsync();
                var movieApi = MovieDbFactory.Create<IApiMovieRequest>().Value;
                ApiSearchResponse<MovieInfo> apiResponse = await movieApi.SearchByTitleAsync(movieName);
                Attachment movieCard = null;
                
                // Check we got something back
                if (apiResponse.Results.Any())
                {
                    // Take the first one
                    MovieInfo info = apiResponse.Results.FirstOrDefault();
                    List<CardImage> cardImages = new List<CardImage>();

                    // Check we found an image - otherwise default
                    string movieImage = $"{c}w500{info.BackdropPath}";
                    if (info.BackdropPath == null || info.BackdropPath.Length == 0)
                    {
                        movieImage = "http://2.bp.blogspot.com/_SrngIbj3Pt8/TN21ACfvLiI/AAAAAAAAAzQ/IrzZvHTR4ag/s1600/Movie+Clapper+Board.png";
                    }

                    Debug.WriteLine($"Poster path: {movieImage}");

                    // Using AdaptiveCard
                    AdaptiveCard card = new AdaptiveCard();

                    // Add Image  to the card.
                    card.Body.Add(new Image()
                    {
                        Url = movieImage,
                        Size = ImageSize.Large
                    });

                    // Specify speech for the card.
                    card.Speak = "<s>Your  meeting about \"Adaptive Card design session\"<break strength='weak'/> is starting at 12:30pm</s><s>Do you want to snooze <break strength='weak'/> or do you want to send a late notification to the attendees?</s>";

                    // Add Overview text to the card.
                    card.Body.Add(new TextBlock()
                    {
                        Text = info.Overview,
                        Size = TextSize.Large,
                        Weight = TextWeight.Bolder
                    });

                    // Add Title text to the card.
                    card.Body.Add(new TextBlock()
                    {
                        Text = info.Title,
                        Size = TextSize.Large,
                        Weight = TextWeight.Bolder
                    });

                    // Add buttons to the card.
                    card.Actions.Add(new SubmitAction()
                    {
                        Title = movieName,
                        Data = movieName

                    });

                    // Create the attachment.
                    movieCard = new Attachment()
                    {
                        ContentType = AdaptiveCard.ContentType,
                        Content = card
                    };

                    /* commented out for old HeroCard
                    cardImages.Add(new CardImage(url: movieImage));

                    CardAction plButton = new CardAction()
                    {
                        Value = movieName,
                        Type = "imBack",
                        Title = "Answer"
                    };

                    List<CardAction> cardButtons = new List<CardAction>();
                    cardButtons.Add(plButton);

                    HeroCard plCard = new HeroCard()
                    {
                        Text = info.Overview,
                        Title = info.Title,
                        Images = cardImages,
                        Buttons = cardButtons,
                        Tap = new CardAction(type: ActionTypes.ImBack, title: "Answer", value: movieName)
                    };
                    movieCard = plCard.ToAttachment();
                    */



                }
                return movieCard;
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

    }
}


//string news = string.Format("[{0}]({1})", item.Title, item.Url);
//Activity newsItem = activity.CreateReply();
//newsItem.Attachments = new List<Attachment>();

//                        List<CardImage> cardImages = new List<CardImage>();
//cardImages.Add(new CardImage(url: item.ThumbnailUrl));

//                        CardAction plButton = new CardAction()
//                        {
//                            Value = item.Url,
//                            Type = "openUrl",
//                            Title = "More"
//                        };

//List<CardAction> cardButtons = new List<CardAction>();
//cardButtons.Add(plButton);

//                        HeroCard plCard = new HeroCard()
//                        {
//                            Text = item.Description,
//                            Title = item.Title,
//                            Images = cardImages,
//                            Buttons = cardButtons
//                        };
//Attachment plAttachment = plCard.ToAttachment();
//newsItem.Attachments.Add(plAttachment);
//                        await connector.Conversations.ReplyToActivityAsync(newsItem);
