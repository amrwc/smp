using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Smp.Web.Models;
using Smp.Web.Models.Requests;
using Smp.Web.Repositories;

namespace Smp.Web.Services
{
    public interface IPostsService
    {
        Task CreatePost(Post post);
        Task<IList<Post>> GetPostsByReceiverId(Guid receiverId, int count);
    }

    public class PostsService : IPostsService
    {
        private readonly IPostsRepository _postsRepository;

        public PostsService(IPostsRepository postsRepository)
        {
            _postsRepository = postsRepository;
        }

        public async Task CreatePost(Post post)
        {
            await _postsRepository.CreatePost(post);
        }

        public async Task<IList<Post>> GetPostsByReceiverId(Guid receiverId, int count)
        {
            return await _postsRepository.GetPostsByReceiverId(receiverId, count);
        }
    }
}