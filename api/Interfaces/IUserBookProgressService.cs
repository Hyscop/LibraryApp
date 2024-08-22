using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;

namespace api.Interfaces
{
    public interface IUserBookProgressService
    {
        UserBookProgress GetProgressByUserAndBook(int userId, int bookId);
        void StartReadingBook(int userId, int bookId);
        void UpdateReadingProgress(int userId, int bookId, int currentPage);
        void FinishReadingBook(int userId, int bookId);
    }
}