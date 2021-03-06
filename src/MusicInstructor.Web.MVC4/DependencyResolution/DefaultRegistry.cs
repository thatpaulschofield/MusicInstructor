using System;
using System.Linq;
using System.Web;
using Google.YouTube;
using MusicInstructor.Web.Controllers;
using MusicInstructor.Web.Domain;
using MusicInstructor.Web.Models;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Linq;
using StructureMap;
using StructureMap.Configuration.DSL;

namespace MusicInstructor.Web
{
    public class DefaultRegistry : Registry
    {
        private static DocumentStore Store;
        public DefaultRegistry()
        {
            
            For<IDocumentSession>().HybridHttpOrThreadLocalScoped().Use(x =>
                                                                            { 
                                                                                if (Store == null)
                                                                                {
                                                                                    Store = new DocumentStore { ConnectionStringName = "RavenDB" };
                                                                                    Store.Initialize();
                                                                                    Glimpse.RavenDb.Profiler.AttachTo(Store);
                                                                                    Glimpse.RavenDb.Profiler.HideFields("PasswordHash", "PasswordSalt");
                                                                                    try
                                                                                    {
                                                                                        Raven.Client.Indexes.IndexCreation.CreateIndexes(System.Reflection.Assembly.GetExecutingAssembly(), Store);
                                                                                    }
                                                                                    catch (Exception)
                                                                                    {
                                                                                        throw;
                                                                                    }
                                                                                }
                                                                                return Store.OpenSession();
                                                                            }
                );
            For<IAuthenticationSession>().HybridHttpOrThreadLocalScoped().Use<CookieAuthenticationSession>();
            For<UserProfile>().Use(c =>
                                            {
                                                var uid = c.GetInstance<IAuthenticationSession>();
                                                if (uid.Id == Guid.Empty) 
                                                    return UserProfile.NullProfile;

                                                var authenticatedUserId = uid.Id;
                                                return c.GetInstance<IDocumentSession>()
                                                           .Load<UserProfile>(authenticatedUserId)
                                                       ?? UserProfile.NullProfile
                                                    ;
                                            });
            For<Login>().Use(ResolveEntityByUserId<Login>);
            For<VideoBrowserController>().Use<VideoBrowserController>();
            For<YouTubeRequestSettings>().Use(new YouTubeRequestSettings("Music Instructor",
                                                                         "AI39si5JFd4Ly6A2ptl860ze7B1ribtPzSmPBr9Dmym9HFE-OUBsAwGsEgzZcUKgokLnWkJfwCnaSBbCk5XZSho2yRuG1WIJPQ"));
            For<IVideoKeywordSearch>().Use<VideoSearchQuery>();
        }

        private static T ResolveEntityByUserId<T>(IContext context) where T : class
        {
            var session = context.GetInstance<IDocumentSession>();
            var authenticationSession = context.GetInstance<IAuthenticationSession>();
            var authenticatedUserId = authenticationSession.Id;
            if (authenticatedUserId == null)
                return null;

            var entity = session.Load<T>(authenticatedUserId);
            return entity;
        }
    }
}