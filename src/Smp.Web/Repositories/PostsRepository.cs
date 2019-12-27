using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Smp.Web.Factories;
using Smp.Web.Models;

namespace Smp.Web.Repositories
{
    public interface IPostsRepository
    {
        Task CreatePost(Post post);
        Task<Post> GetPostById(Guid postId);
        Task<IList<Post>> GetPostsByReceiverId(Guid receiverId, int count);
    }

    public class PostsRepository : IPostsRepository
    {
        private readonly IDbConnection _dbConnection;

        public PostsRepository(IDbConnectionFactory connectionFactory)
        {
            _dbConnection = connectionFactory.GetDbConnection();
        }

        public async Task CreatePost(Post post)
        {
            await _dbConnection.ExecuteAsync("INSERT INTO [dbo].[Posts] ([Id], [ReceiverId], [SenderId], [Content], [CreatedAt]) VALUES (@Id, @ReceiverId, @SenderId, @Content, @CreatedAt)",
                new { Id = post.Id, ReceiverId = post.ReceiverId, SenderId = post.SenderId, Content = post.Content, CreatedAt = post.CreatedAt });
        }

        public Task<Post> GetPostById(Guid postId)
        {
            throw new NotImplementedException();
        }

        public async Task<IList<Post>> GetPostsByReceiverId(Guid receiverId, int count)
        {
            var dbPosts = (await _dbConnection.QueryAsync<Models.DTOs.Post>("SELECT TOP (@Count) * FROM [dbo].[Posts] WHERE [ReceiverId] = @ReceiverId ORDER BY [CreatedAt] DESC",
                new { Count = count, ReceiverId = receiverId })).ToList();

            return dbPosts.Select(dbPost => (Post)dbPost).ToList();
        }
    }
}