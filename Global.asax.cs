using Autofac;
using DM.MovieApi;
using KevinBaconBot.Helpers;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Internals.Fibers;
using Microsoft.IdentityModel.Protocols;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

namespace KevinBaconBot
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            this.RegisterBotModules();
            GlobalConfiguration.Configure(WebApiConfig.Register);

            MovieDbFactory.ResetFactory();
            MovieDbFactory.RegisterSettings(ConfigurationManager.AppSettings["TheMovieApiKey"],
                ConfigurationManager.AppSettings["TheMovieApiUri"]);

            //MovieDbFactory.RegisterSettings(new MovieDBSettings());
        }

        private void RegisterBotModules()
        {
            var builder = new ContainerBuilder();

            builder.RegisterModule(new ReflectionSurrogateModule());

            builder.RegisterModule<GlobalMessageHandlersBotModule>();

            builder.Update(Conversation.Container);
        }
    }
}
