using Cartridge.Models;
using Dapper;
using Microsoft.AspNetCore.Identity;
using System.Data;

namespace Cartridge.Data
{
    public class ReviewRepository
    {

        private readonly IDbConnection _db;

        public ReviewRepository(IDbConnection db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Reviews>> GetReviewsByGameID(int id) 
        {
            return await _db.QueryAsync<Reviews>
            (@"SELECT 
                game_id AS GameID,
                users_id AS UserID,
                rating AS Rating,
                review_body AS ReviewBody,
                review_date AS ReviewDate,
                username AS Username
            FROM reviews
            WHERE game_id = @id",
             new {id});

        }

        public async Task<IEnumerable<Reviews>> GetReviewsByUserID(int id)
        {
            return await _db.QueryAsync<Reviews>
            (@"SELECT 
                game_id AS GameID,
                users_id AS UserID,
                rating AS Rating,
                review_body AS ReviewBody,
                review_date AS ReviewDate,
                username AS Username
            FROM reviews
            WHERE users_id = @id",
             new { id });

        }



        public async Task InsertReview(Reviews Review)
        {
            
            await _db.ExecuteAsync(
                 "INSERT INTO reviews (game_id, users_id, rating, review_body, review_date, username) VALUES " +
                 "(@Review.GameID, @Review.UserID, @Review.ReviewBody, @Review.ReviewDate, @Review.Username)",
                 new { Review.GameID, Review.UserID, Review.ReviewBody, Review.ReviewDate, @Review.Username } 
             );
        }


    }
}
