using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using DynamoDBMusicService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DynamoDBMusicService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MusicServiceController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<MusicServiceController> _logger;
        private readonly IDynamoDBContext _dynamoDbContext;
        private readonly IAmazonDynamoDB _dynamoDBClient;

        public MusicServiceController(ILogger<MusicServiceController> logger, IDynamoDBContext dynamoDbContext, IAmazonDynamoDB dynamoDBClient)
        {
            _logger = logger;
            _dynamoDBClient = dynamoDBClient;
            _dynamoDbContext = dynamoDbContext;            
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet]
        [Route("tables/get")]
        public async Task<IActionResult> GetTables()
        {
            var tables = await _dynamoDBClient.ListTablesAsync();
            return Ok(tables);
        }

        [HttpGet]
        [Route("music/{search}")]
        public async Task<IActionResult> GetMusic(string search)
        {
            var music = await _dynamoDbContext.QueryAsync<Music>(search).GetRemainingAsync();
            return Ok(music);
        }
    }
}
