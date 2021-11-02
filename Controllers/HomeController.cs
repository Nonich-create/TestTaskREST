using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestTask.BLL.Interface;
using TestTask.Core.Entities;

namespace TestTaskREST.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {

        private readonly IElasticClient _client;
        private readonly IArticleService _articleService;
        public HomeController(IElasticClient client, IArticleService articleService)
        {
            _client = client;
            _articleService = articleService;
        }

        [HttpGet, Route("Get")]
        public async Task<Article> Get(Guid id)
        {
            var response = await _client.SearchAsync<Article>(a => a.Query(q => q.Term(t => t.Author, id) ||
            q.Match(m => m.Field(f => f.Author).Query(id.ToString()))));
          
            return response?.Documents?.FirstOrDefault();
        }

        [HttpGet, Route("GetAll")]
        public async Task<IEnumerable<Article>> GetAll()
        {
            return await _articleService.GetAllAsync();
        }

        [HttpGet, Route("GetTopDecem")]
        public async Task<IEnumerable<Article>> GetTopDecem()
        {
            return await _articleService.GetTopDecem();
        }

        [HttpGet, Route("GetDateRange")]
        public async Task<IEnumerable<Article>> GetDateRange()
        {

            return await _articleService.GetAllByDateRange();
        }

        [HttpPost, Route("Initializer")]
        public async Task Initializer()
        {
           await _articleService.Initializer();
        }

    }
}
