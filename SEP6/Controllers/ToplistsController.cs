﻿using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SEP6.Database;
using SEP6.Utilities;
using TMDbLib.Client;

namespace SEP6.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ToplistsController :ControllerBase
    {
            private readonly ILogger<ToplistsController> _logger;
            private readonly MoviesContext _moviesContext;


            public ToplistsController(ILogger<ToplistsController> logger, MoviesContext moviesContext)
            {
                _logger = logger;
                _moviesContext = moviesContext;
            }

            [HttpPost]
            public ObjectResult Add([FromBody] Toplists toplist,string token)
            {
                var user = _moviesContext.Users.First(a => a.Token == token);
                
                if (ControllerUtilities.TokenVerification(user, toplist.UserId, token))
                    return Unauthorized("User/token mismatch");

                var movies = toplist.Movies.ToList();
                toplist.Movies.Clear();
                for (int i = 0; i < movies.Count; i++)
                {
                    var dbMovie = _moviesContext.Movies.First(a => a.Id == movies[i].Id);
                    toplist.Movies.Add(dbMovie);
                }
                
                _moviesContext.TopLists.Add(toplist);
                _moviesContext.SaveChanges();
                return Ok("Toplist added successfully");
            }
            
            [HttpPut]
            public ObjectResult Edit([FromBody] Toplists toplist,string token)
            {
                var user = _moviesContext.Users.First(a => a.Token == token);
                
                if (ControllerUtilities.TokenVerification(user, toplist.UserId, token))
                    return Unauthorized("User/token mismatch");
                
                
                var movies = toplist.Movies.ToList();
                
                var dbToplist = _moviesContext.TopLists
                    .Include(a => a.Movies)
                    .First(a => a.Id == toplist.Id);
                
                //clears the lists to avoid tracking on thr wrong entities
                dbToplist.Movies.Clear();
                toplist.Movies.Clear();
                
                //updates the data received in the dbTopList that is being tracked by the EF
                for (int i = 0; i < movies.Count; i++)
                {
                    var dbMovie = _moviesContext.Movies.First(a => a.Id == movies[i].Id);
                    dbToplist.Movies.Add(dbMovie);
                }

                dbToplist.Name = toplist.Name;
                
                _moviesContext.SaveChanges();
                return Ok("Toplist added successfully");
            }

            [HttpGet]
            public ObjectResult Get(string token, int userid)
            {
                var user = _moviesContext.Users
                    .Include(a => a.UserTopLists)
                    .First(a=> a.Id == userid);

                if (ControllerUtilities.TokenVerification(user, userid, token))
                    return Unauthorized("User/token mismatch");

                return Ok(user.UserTopLists);
            }
    }
}