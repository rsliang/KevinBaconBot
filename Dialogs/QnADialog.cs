namespace KevinBaconBot.Dialogs
{
    using Microsoft.Bot.Builder.CognitiveServices.QnAMaker;
    using System;
    using System.Configuration;

    [Serializable]
    public class QnADialog : QnAMakerDialog
    {
        //Parameters to QnAMakerService are:
        //Compulsory: subscriptionKey, knowledgebaseId, 
        //Optional: defaultMessage, scoreThreshold[Range 0.0 – 1.0]
        public QnADialog() : base(new QnAMakerService
                                    (new QnAMakerAttribute(ConfigurationManager.AppSettings["QnASubscriptionKey"], 
                                        ConfigurationManager.AppSettings["QnAKnowledgebaseId"], 
                                        "Sorry I, Kevin Bacon don't understand, please ask me something else.", 0.5)))
        { }
    }
}