using System;
using System.Collections.Generic;
using System.Text;

namespace Legatto.CoreDomain.Test
{
    public class AccountFactory
    {
        private static int accountNo = 1;

        public List<FileStructureEntity> BuildFolderContents(List<User> authorisedUsers, int dirCount, int depth, int fileCount)
        {
            List<FileStructureEntity> folderContents = new List<FileStructureEntity>();

            for(int i=0; i < fileCount; i++)
            {
                folderContents.Add(new File { Name = $"Document {i}.Doc", Text = $"Document {i} here" });
            }

            for(int i=0; depth > 0 && i < dirCount; i++)
            {
                folderContents.Add
                    (
                        new ChildFolder
                        {
                            AuthorisedUsers = authorisedUsers,
                            Contents = BuildFolderContents(authorisedUsers, dirCount, depth - 1, fileCount)
                        }
                    );
            }

            // Shrink the authorised users as the generator runs (10% chance)
            if( authorisedUsers.Count > 0 && new Random().NextDouble() < 0.1)
            {
                authorisedUsers.RemoveAt(0);
            }

            return folderContents;
        }

        /// <summary>
        /// Randomised accounts based on scale factor
        /// </summary>
        /// <param name="factor">scale factor in range 0.0 to 1.0</param>
        /// <returns>Account</returns>
        public Account BuildAccount(List<User> authorisedUsers, double factor)
        {
            User owner = new User { Role = UserRole.Owner, Email = $"test_{accountNo}@example.com", Name = $"Joe Bloggs {accountNo}", Username = $"JoeBloggs{accountNo}" };

            accountNo++;

            int dirCount = (int)((factor - (factor % 0.1)) * 10);
            int dirDepth = (int)((factor - (factor % 0.01)) * 100) % 10;
            int dirFiles = (int)((factor - (factor % 0.001)) * 1000) % 10;

            // The recursive routine can prune the user list as it traverses - so create a shallow copy to seed to traversal
            List<User> directoryAuthorisedUser = new List<User>();
            directoryAuthorisedUser.AddRange(authorisedUsers);

            RootFolder rootFolder = new RootFolder
            {
                Contents = BuildFolderContents(directoryAuthorisedUser, dirCount, dirDepth, dirFiles), 
                AuthorisedUsers = directoryAuthorisedUser
            };

            // Act 
            Account account 
                = new Account 
                { 
                    Id = accountNo,
                    Owner = owner, 
                    AccreditedParties = authorisedUsers, 
                    RootFolder = rootFolder 
                };

            return account;
        }
    }
}
