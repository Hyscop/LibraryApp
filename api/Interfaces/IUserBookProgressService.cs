using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs;
using api.Models;

namespace api.Interfaces
{
    public interface IUserBookProgressService
    {
        UserBookProgressDto GetProgressByUserAndBook(int userId, int bookId);
        void StartReadingBook(int userId, int bookId);
        void UpdateReadingProgress(int userId, int bookId, int currentPage);
        void FinishReadingBook(int userId, int bookId);
    }
}