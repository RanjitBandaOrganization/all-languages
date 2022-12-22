using AutoMapper;
using CourseLibrary.API.Helpers;
using CourseLibrary.API.Models;
using CourseLibrary.API.ResourceParameters;
using CourseLibrary.API.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace CourseLibrary.API.Controllers
{
    //////basics we encounter when we implement the outer-facing contract for the API throughout this course, create controller, inject repository, create action methods, and return results.
    //////Think about requiring attribute routing, automatically returning a 400 Bad Request on bad input, and returning problem details on errors. 
    [ApiController]
    //[Route("api/[controller]")]   //refactoring the code will be a problem with this approach
    [Route("api/authors")]
    //////This ControllerBase class contains basic functionality controllers need like access to the model state, the current user, and common methods for returning responses.
    //////We could also inherit it from Controller, but by doing so, we'd also add support for views, which isn't needed when building an API.
    public class AuthorsController : ControllerBase
    {
        private readonly ICourseLibraryRepository _courseLibraryRepository;
        private readonly IMapper _mapper;

        public AuthorsController(ICourseLibraryRepository courseLibraryRepository,
            IMapper mapper)
        {
            _courseLibraryRepository = courseLibraryRepository ??
                throw new ArgumentNullException(nameof(courseLibraryRepository));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
            //_courseLibraryRepository.RestoreDataStore();
        }

        //////To return data, we need to add an action on our controller.
        //////IActionResult defines a contract that represents the results of an action method.
        ////[HttpGet("api/authors")]
        [HttpGet(Name = "GetAuthors")]
        [HttpHead()]
        //public IActionResult GetAuthors()
        public ActionResult<IEnumerable<AuthorDto>> GetAuthors(
            //[FromQuery]string mainCategory
            //[FromQuery(Name = "mainCategory")] string mainCategory
            //string mainCategory,
            //string searchQuery
            //BY DEFAULT THE COMPLEX TYPE IS EXPECTED TO BE BIND USING FROMBODY RESOURCE, AS CURRENTLY WE ARE SENDING VIA QUERY PARAMETERS
            //WE HAVE TO EXPLICITY MENTION AS FROMQUERY TO POPULATE THIS COMPLEX TYPE USING QUERY PARAMETER VALUES
            [FromQuery]AuthorsResourceParameters authorsResourceParameters
            )
        {
            var authorsFromRepo = _courseLibraryRepository.GetAuthors(authorsResourceParameters);
            //var authors = new List<AuthorDto>();

            //GENERATE URI s FOR PREVIOUS AND NEXT PAGES
            //URL HELPER CLASS HELPS TO GENERATE THESE LINKS AND AVAILABLE IN BASE CONTROLLER CLASS VIA URL PROPERTY
            var previousPageLink = authorsFromRepo.HasPrevious ?
                CreateAuthorsResourceUri(authorsResourceParameters, ResourceUriType.PreviousPage) :
                null;

            var nextPageLink = authorsFromRepo.HasNext ?
                CreateAuthorsResourceUri(authorsResourceParameters, ResourceUriType.NextPage) :
                null;

            var paginationMetadata = new
            {
                totalCount = authorsFromRepo.TotalCount,
                pageSize = authorsFromRepo.PageSize,
                currentPage = authorsFromRepo.CurrentPage,
                totalPages = authorsFromRepo.TotalPages,
                previousPageLink,
                nextPageLink
            };

            //ADDING THIS INFORMATION AS CUSTOMER HEADER TO THE RESPONSE
            Response.Headers.Add("X-Pagination",
                JsonSerializer.Serialize(paginationMetadata));

            //foreach (var author in authorsFromRepo)
            //{
            //    authors.Add(new AuthorDto()
            //    {
            //        Id = author.Id,
            //        Name = $"{author.FirstName} {author.LastName}",
            //        MainCategory = author.MainCategory,
            //        Age = author.DateOfBirth.GetCurrentAge()
            //    });
            //}
            var authors = _mapper.Map<IEnumerable<AuthorDto>>(authorsFromRepo);
            //////JsonResult is an action result which formats the given object as JSON.
            //return new JsonResult(authorsFromRepo);
            return Ok(authors);
        }

        //[HttpGet("{authorId:guid}")]  - with route constraint, if the method have overloading
        [HttpGet("{authorId}", Name = "GetAuthor")]
        public ActionResult<AuthorDto> GetAuthor(Guid authorId)
        {
            var authorFromRepo = _courseLibraryRepository.GetAuthor(authorId);

            if (authorFromRepo == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<AuthorDto>(authorFromRepo));
        }


        [HttpPost]
        public ActionResult<AuthorDto> CreateAuthor(AuthorForCreationDto author)
        {
            var authorEntity = _mapper.Map<Entities.Author>(author);
            _courseLibraryRepository.AddAuthor(authorEntity);
            _courseLibraryRepository.Save();

            var authorToReturn = _mapper.Map<AuthorDto>(authorEntity);

            return CreatedAtRoute("GetAuthor",
                new { authorId = authorToReturn.Id },
                authorToReturn);
        }

        [HttpOptions]
        public IActionResult GetAuthorsOptions()
        {
            Response.Headers.Add("Allow", "GET,OPTIONS,POST");
            return Ok();
        }

        [HttpDelete("{authorId}")]
        public ActionResult DeleteAuthor(Guid authorId)
        {
            var authorFromRepo = _courseLibraryRepository.GetAuthor(authorId);

            if (authorFromRepo == null)
            {
                return NotFound();
            }

            _courseLibraryRepository.DeleteAuthor(authorFromRepo);

            _courseLibraryRepository.Save();

            return NoContent();
        }
        private string CreateAuthorsResourceUri(
                   AuthorsResourceParameters authorsResourceParameters,
                   ResourceUriType type)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return Url.Link("GetAuthors",
                      new
                      {
                          //fields = authorsResourceParameters.Fields,
                          //orderBy = authorsResourceParameters.OrderBy,
                          pageNumber = authorsResourceParameters.PageNumber - 1,
                          pageSize = authorsResourceParameters.PageSize,
                          mainCategory = authorsResourceParameters.MainCategory,
                          searchQuery = authorsResourceParameters.SearchQuery
                      });
                case ResourceUriType.NextPage:
                    return Url.Link("GetAuthors",
                      new
                      {
                          //fields = authorsResourceParameters.Fields,
                          //orderBy = authorsResourceParameters.OrderBy,
                          pageNumber = authorsResourceParameters.PageNumber + 1,
                          pageSize = authorsResourceParameters.PageSize,
                          mainCategory = authorsResourceParameters.MainCategory,
                          searchQuery = authorsResourceParameters.SearchQuery
                      });
                case ResourceUriType.Current:
                default:
                    return Url.Link("GetAuthors",
                    new
                    {
                        //fields = authorsResourceParameters.Fields,
                        //orderBy = authorsResourceParameters.OrderBy,
                        pageNumber = authorsResourceParameters.PageNumber,
                        pageSize = authorsResourceParameters.PageSize,
                        mainCategory = authorsResourceParameters.MainCategory,
                        searchQuery = authorsResourceParameters.SearchQuery
                    });
            }

        }

        //private IEnumerable<LinkDto> CreateLinksForAuthor(Guid authorId, string fields)
        //{
        //    var links = new List<LinkDto>();

        //    if (string.IsNullOrWhiteSpace(fields))
        //    {
        //        links.Add(
        //          new LinkDto(Url.Link("GetAuthor", new { authorId }),
        //          "self",
        //          "GET"));
        //    }
        //    else
        //    {
        //        links.Add(
        //          new LinkDto(Url.Link("GetAuthor", new { authorId, fields }),
        //          "self",
        //          "GET"));
        //    }

        //    links.Add(
        //       new LinkDto(Url.Link("DeleteAuthor", new { authorId }),
        //       "delete_author",
        //       "DELETE"));

        //    links.Add(
        //        new LinkDto(Url.Link("CreateCourseForAuthor", new { authorId }),
        //        "create_course_for_author",
        //        "POST"));

        //    links.Add(
        //       new LinkDto(Url.Link("GetCoursesForAuthor", new { authorId }),
        //       "courses",
        //       "GET"));

        //    return links;
        //}

        //private IEnumerable<LinkDto> CreateLinksForAuthors(
        //    AuthorsResourceParameters authorsResourceParameters,
        //    bool hasNext, bool hasPrevious)
        //{
        //    var links = new List<LinkDto>();

        //    // self 
        //    links.Add(
        //       new LinkDto(CreateAuthorsResourceUri(
        //           authorsResourceParameters, ResourceUriType.Current)
        //       , "self", "GET"));

        //    if (hasNext)
        //    {
        //        links.Add(
        //          new LinkDto(CreateAuthorsResourceUri(
        //              authorsResourceParameters, ResourceUriType.NextPage),
        //          "nextPage", "GET"));
        //    }

        //    if (hasPrevious)
        //    {
        //        links.Add(
        //            new LinkDto(CreateAuthorsResourceUri(
        //                authorsResourceParameters, ResourceUriType.PreviousPage),
        //            "previousPage", "GET"));
        //    }

        //    return links;
        //}


    }
}
