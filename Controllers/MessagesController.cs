using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System.Collections.Generic;
using KevinBaconBot.Dialogs;
using System.Linq;
using System;

namespace KevinBaconBot
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));
            if (activity.Type == ActivityTypes.Message)
            {
                await Conversation.SendAsync(activity, () => new Dialogs.RootDialog());
            }
            else
            {
                var reply = HandleSystemMessage(activity);
                if (reply != null)
                {
                    await connector.Conversations.ReplyToActivityAsync(reply);
                }
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        private Activity HandleSystemMessage(Activity message)
        {
            if (message.Type == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == ActivityTypes.ConversationUpdate)
            {
                // Handle conversation state changes, like members being added and removed
                // Use Activity.MembersAdded and Activity.MembersRemoved and Activity.Action for info
                // Not available in all channels

                if (message.MembersAdded.Any(m => m.Id == message.Recipient.Id))
                {
                    return WelcomeMessageReplyCreation(message);
                }

            }
            else if (message.Type == ActivityTypes.ContactRelationUpdate)
            {
                if (message.Action == "add")
                {
                    return WelcomeMessageReplyCreation(message);
                }
                else
                {
                    // delete data
                    var stateClient = message.GetStateClient();
                    stateClient.BotState.DeleteStateForUser(message.ChannelId, message.From.Id);
                }
            }
            else if (message.Type == ActivityTypes.Typing)
            {
                // Handle knowing tha the user is typing
            }
            else if (message.Type == ActivityTypes.Ping)
            {
            }
            else if (message.Type == ActivityTypes.Event)
            {
                if (message.Name == "relationId")
                {
                    //var encryptedRelationId = message.Value.ToString();
                    //var policy = DataAccess.GetPolicyData(encryptedRelationId);

                    ////string msg = string.Format("Hi {0} {1} {2}", policy.MotorPolicies[0].Title, policy.MotorPolicies[0].FirstName, policy.MotorPolicies[0].Surname);
                    ////var reply = message.CreateReply(msg);

                    //var reply = this.GenerateGreetingHeroCard(message, policy.MotorPolicies[0].FirstName, policy.MotorPolicies[0].CarMake);

                    //try
                    //{
                    //    StateClient stateClient = message.GetStateClient();
                    //    BotData bd = new BotData();
                    //    bd.SetProperty<Policies>(message.Conversation.Id, policy);
                    //    bd.ETag = "*";
                    //    stateClient.BotState.SetConversationData(message.ChannelId, message.Conversation.Id, bd);
                    //    Debug.WriteLine($"Policy data saved to state for ConversationId: {message.Conversation.Id}");
                    //}
                    //catch (Exception exp)
                    //{
                    //    Debug.WriteLine("Error storing state: " + exp);
                    //    throw;
                    //}

                    //var connector = new ConnectorClient(new Uri(message.ServiceUrl));

                    //await connector.Conversations.ReplyToActivityAsync(reply);
                }
            }

            return null;
        }

        private Activity WelcomeMessageReplyCreation(Activity activity)
        {
            var stateClient = activity.GetStateClient();
            stateClient.BotState.DeleteStateForUser(activity.ChannelId, activity.From.Id);
            setUserDataValue(activity, "WelcomeMessageSent", true);

            var reply = activity.CreateReply();
            WelcomeDialog.ApplyWelcomeMessageAsync(activity, reply);
            return reply;
        }

        private void setUserDataValue<T>(Activity message, string key, T value)
        {
            var stateClient = message.GetStateClient();
            var userData = stateClient.BotState.GetUserData(message.ChannelId, message.From.Id);
            userData.SetProperty(key, value);
            stateClient.BotState.SetUserData(message.ChannelId, message.From.Id, userData);
        }

        private T getUserDataValue<T>(Activity message, string key)
        {
            var stateClient = message.GetStateClient();
            var userData = stateClient.BotState.GetUserData(message.ChannelId, message.From.Id);
            return userData.GetProperty<T>(key);
        }
    }
}