using System;
using System.Linq;
using System.Web;
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
                                                if (uid.Id == null) return UserProfile.NullProfile;

                                                var authenticatedUserId = new Guid(uid.Id);
                                                return c.GetInstance<IDocumentSession>()
                                                           .Load<UserProfile>(authenticatedUserId)
                                                       ?? UserProfile.NullProfile
                                                    ;
                                            });
            For<Login>().Use(ResolveEntityByUserId<Login>);
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