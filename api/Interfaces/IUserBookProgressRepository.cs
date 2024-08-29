using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;

namespace api.Interfaces
{
    public interface IUserBookProgressRepository
    {
        UserBookProgress GetProgressByUserAndBook(int userId, int bookId);
        IEnumerable<UserBookProgress> GetProgressesByUserId(int userId);
        void AddProgress(UserBookProgress progress);
        void UpdateProgress(UserBookProgress progress);
        void DeleteProgress(UserBookProgress progress);
    }
}