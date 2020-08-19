using Legatto.BatchService;
using Maquina.BusinessDomain.RulesEngine.Service;
using Maquina.Service.Rules.Definition;
using Maquina.Service.Test;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Legatto.CoreDomain.Test
{
    [TestFixture]
    public class AccountTest : BaseTest
    {
        // crow bar linkeage - bodge for example
        List<Type> t = new List<Type> { typeof(RuleService), typeof(AccountRule) };

        [Test]
        public void BuildAccount_FileCount_Test()
        {
            // Arrange 
            User owner = new User { Role = UserRole.Owner, Email = "test@example.com", Name = "Joe Bloggs", Username = "JoeBloggs" };
            User son = new User { Role = UserRole.AccreditedParty, Email = "sonny@example.com", Name = "Sunny Bloggs", Username = "SunnyBloggs" };
            User daughter = new User { Role = UserRole.AccreditedParty, Email = "dotty@example.com", Name = "Dorothy Bloggs", Username = "DorothyBloggs" };

            RootFolder rootFolder = new RootFolder
            {
                Contents = new List<FileStructureEntity>
                {
                    new ChildFolder
                    {
                        Name = "Public Photos",
                        AuthorisedUsers = new List<User>{ son, daughter },
                        Contents = new List<FileStructureEntity>
                        {
                            new File { Name = "Public Photo1.jpg", Text="photo 1 here"},
                            new File { Name = "Public Photo2.jpg", Text="photo 2 here"},
                            new File { Name = "Public Photo3.jpg", Text="photo 3 here"},
                        }
                   },
                   new ChildFolder
                   {
                        Name = "Public Documents",
                        AuthorisedUsers = new List<User>{ son, daughter },
                        Contents = new List<FileStructureEntity>
                        {
                            new File { Name = "Public Document1.doc", Text="Document 1 here"},
                            new File { Name = "Public Document2.doc", Text="Document 2 here"},
                        }
                    },
                    new ChildFolder
                    {
                        Name = "Private Photos for Daughter",
                        AuthorisedUsers = new List<User>{ daughter },
                        Contents = new List<FileStructureEntity>
                        {
                            new File { Name = "Private Photo1.jpg", Text="photo 1 here"} ,
                            new File { Name = "Private Photo2.jpg", Text="photo 2 here"} ,
                            new File { Name = "Private Photo3.jpg", Text="photo 3 here"} ,
                            new File { Name = "Private Photo4.jpg", Text="photo 4 here"} ,
                            new File { Name = "Private Photo5.jpg", Text="photo 5 here"} ,
                        }
                   },
                   new ChildFolder
                   {
                        Name = "Private Documents for Son",
                        AuthorisedUsers = new List<User>{ son },
                        Contents = new List<FileStructureEntity>
                        {
                            new File { Name = "Private Document1.doc", Text="Document 1 here"} ,
                            new File { Name = "Private Document2.doc", Text="Document 2 here"} ,
                            new File { Name = "Private Document3.doc", Text="Document 3 here"} ,
                        }
                    },
                }
            };


            // Act 
            Account account = new Account { Owner = owner, RootFolder = rootFolder };

            // Assert 
            Assert.AreEqual(account.GetFileCount(owner), 13);
            Assert.AreEqual(account.GetFileCount(son), 8);
            Assert.AreEqual(account.GetFileCount(daughter), 10);
        }

        [Test]
        public void BuildBulkAccount_Test()
        {
            const int accountCount = 10;

            // Not so random seed => 
            Random random = new Random(314);

            User son = new User { Role = UserRole.AccreditedParty, Email = "sonny@example.com", Name = "Sunny Bloggs", Username = "SunnyBloggs" };
            User daughter = new User { Role = UserRole.AccreditedParty, Email = "dotty@example.com", Name = "Dorothy Bloggs", Username = "DorothyBloggs" };

            List<Account> bulkAccounts = new List<Account>();

            for (int i=0; i < accountCount; i++)
            {
                List<User> authorisedUsers = new List<User>();
                if (random.NextDouble() > 0.75)
                {
                    authorisedUsers.Add(son);
                }

                if (random.NextDouble() > 0.75)
                {
                    authorisedUsers.Add(daughter);
                }

                bulkAccounts.Add(new AccountFactory().BuildAccount(authorisedUsers, random.NextDouble()));
            }

            IRuleService ruleProcessor = GetInjection<IRuleService>();

            ruleProcessor.AddRule("And(Account.HasFileCount(fileCount:Int32:100),Not(Account.HasAccreditedParties()))=>Message.Publish(message:String:\"Account with large number of files, only Owner defined\")");
            ruleProcessor.AddRule("And(Not(Account.HasFileCount(fileCount:Int32:100)),Account.HasAccreditedParties())=>Message.Publish(message:String:\"Under utilised Account\")");

            ruleProcessor.Process(bulkAccounts);
        }
    }
}
