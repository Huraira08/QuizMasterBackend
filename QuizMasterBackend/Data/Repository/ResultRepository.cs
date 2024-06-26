﻿using Microsoft.EntityFrameworkCore;
using QuizMasterBackend.Models;

namespace QuizMasterBackend.Data.Repository
{
    public class ResultRepository : IResultRepository
    {
        private readonly ApplicationDbContext _db;
        public ResultRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<List<ResultDTO>> GetResults(string id)
        {
            List<ResultDTO> results = await _db.Results.Where(r => r.UserId == id).Select(r => new ResultDTO()
            {
                Id = r.Id.ToString(),
                AttemptedDate = r.AttemptedDate,
                Score = r.Score,
            })
                .ToListAsync();
            return results;
        }

        public async Task<ResultDTO?> AddResult(ResultDTO resultDTO)
        {
            ApplicationUser? user = await _db.Users.FirstOrDefaultAsync(u => u.Id == resultDTO.UserId);
            if (user == null)
            {
                return null;
            }

            Result result = new Result()
            {
                AttemptedDate = resultDTO.AttemptedDate,
                Score = resultDTO.Score,
                UserId = resultDTO.UserId
            };

            await _db.Results.AddAsync(result);
            await _db.SaveChangesAsync();
            resultDTO.Id = result.Id.ToString();
            return resultDTO;
        }

        public async Task<List<ResultDTO>> GetTop10Results()
        {
            List<ResultDTO> top10Results = await _db.Results.OrderByDescending(result => result.Score)
                .Take(10)
                .Include(result => result.User)
                .Select(result => new ResultDTO()
                {
                    Id = result.Id.ToString(),
                    AttemptedDate = result.AttemptedDate,
                    Score = result.Score,
                    Name = result.User.UserName
                }).ToListAsync();

            return top10Results;
        }
    }
}
