using AutoMapper;
using HackerNews.Api.Helpers;
using HackerNews.Api.Models;
using HackerNews.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace HackerNews.Api.Controllers;

[ApiController]
[Route("best-stories")]
public class StoryController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IMemoryCache _memoryCache;
    private readonly IHackerNewsService _hackerNewsService;
    private readonly CacheOptions _cacheOptions;

    public StoryController(
        IMapper mapper,
        IMemoryCache memoryCache,
        IHackerNewsService hackerNewsService,
        IOptions<CacheOptions> cacheOptions)
    {
        _mapper = mapper;
        _memoryCache = memoryCache;
        _hackerNewsService = hackerNewsService;
        _cacheOptions = cacheOptions.Value;
    }

    [HttpGet("{count:int}")]
    public async Task<ActionResult> GetBestStories(int count)
    {
        if (count <= 0)
        {
            return BadRequest(new { ErrorMessage = "Count should have positive value" });
        }

        if (_memoryCache.TryGetValue<List<StoryResponseDto>>(CacheKeys.BestStoriesDetailed, out var dtos)
            && dtos.Count >= count)
        {
            return Ok(dtos.Take(count));
        }

        var bestStoriesDetailed = await _hackerNewsService.GetBestStories(count);

        dtos = _mapper.Map<List<StoryResponseDto>>(bestStoriesDetailed);
        _memoryCache.Set(CacheKeys.BestStoriesDetailed, dtos, TimeSpan.FromSeconds(_cacheOptions.QueryResultTtlInSeconds));

        return Ok(dtos);
    }
}