using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MusicInstructor.Web.Domain;
using MusicInstructor.Web.Models;
using Raven.Abstractions.Indexing;

namespace MusicInstructor.Web.RavenDb
{
    public class ProfileViewModelIndex : Raven.Client.Indexes.AbstractIndexCreationTask<Login, ProfileViewModel>
    {
        public ProfileViewModelIndex()
        {
            TransformResults = (database, logins) => from login in logins
                                                     select new ProfileViewModel
                                                                {
                                                                    UserName = login.UserName
                                                                };
            Map = logins => from login in logins
                            select new ProfileViewModel
                            {
                                UserName = login.UserName
                            };
        }
    }
}