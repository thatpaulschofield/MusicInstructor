using System;
using MusicInstructor.Web.Models;
using Raven.Client;
using Raven.Client.Embedded;
using StructureMap;
using StructureMap.Configuration;
using StructureMap.Configuration.DSL;
using StructureMap.Configuration.DSL.Expressions;

namespace MusicInstructor.Tests
{
    public class UnitTestEnvironment : Registry
    {
        public UnitTestEnvironment()
        {
            Profile("UnitTests", c =>
                                       {
                                           var store = new EmbeddableDocumentStore
                                           {
                                               RunInMemory = true,
                                           }.Initialize();

                                           //Raven.Client.Indexes.IndexCreation.CreateIndexes(System.Reflection.Assembly.GetCallingAssembly(), store);
                                           
                                           c.For<IDocumentSession>().Use(x => store.OpenSession());

                                           c.For<IAuthenticationSession>().Use<MockAuthenticationSession>();
                                       });
        }
    }

    public class MockAuthenticationSession : IAuthenticationSession
    {
        public MockAuthenticationSession()
        {
        }

        public Guid Id { get; set; }
    }
}