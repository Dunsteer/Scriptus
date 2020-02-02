using BaseLogic.Abstractions;
using BaseLogic.Services;
using Commons.Models.Shared;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebServices.Controllers
{
    public class BaseRESTController<DB, SearchModel> : BaseController where DB : class
    {
        protected class RESTConfig
        {
            public bool Allowed { get; set; }
            public Type MapTo { get; set; }
            public Type MapToMin { get; set; }
        }

        protected class REST
        {
            protected Dictionary<string, RESTConfig> Config = new Dictionary<string, RESTConfig>()
            {
                ["GET"] = new RESTConfig { Allowed = true, MapTo = null, MapToMin = null },
                ["POST"] = new RESTConfig { Allowed = true, MapTo = null, MapToMin = null },
                ["PATCH"] = new RESTConfig { Allowed = true, MapTo = null, MapToMin = null },
                ["DELETE"] = new RESTConfig { Allowed = true, MapTo = null, MapToMin = null },
                ["PUT"] = new RESTConfig { Allowed = true, MapTo = null, MapToMin = null },
            };

            public RESTConfig GET { get { return Config["GET"]; } }
            public RESTConfig POST { get { return Config["POST"]; } }
            public RESTConfig PUT { get { return Config["PUT"]; } }
            public RESTConfig PATCH { get { return Config["PATCH"]; } }
            public RESTConfig DELETE { get { return Config["DELETE"]; } }
        }

        protected REST _REST;
        protected readonly BaseService<DB> _dataService;
        protected readonly MapperService _mapper;
        protected readonly ILogger _logger;

        public BaseRESTController(BaseService<DB> dataService, MapperService mapper, ILogger logger)
        {
            _REST = new REST();

            _dataService = dataService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet("")]
        public virtual async Task<IActionResult> GetAll([FromQuery]SearchModel search, [FromQuery] bool min = false)
        {
            try
            {
                if (!_REST.GET.Allowed) return StatusCode(405, "Method not allowed");

                var result = (typeof(SearchModel) == typeof(IDictionary<string, string>)) ? (await _dataService.Search((IDictionary<string, string>)search)) : (await _dataService.GetAll(search));

                var type = min ? _REST.GET.MapToMin : _REST.GET.MapTo;
                if (type == null) return Ok(result.ToList());

                var listType = typeof(List<>);
                var constructedListType = listType.MakeGenericType(type);

                int count = result.Count();
                if (search is PagerBase pager)
                {
                    if (pager.PageSize != null && pager.Page != null)
                    {
                        pager.Page = null;
                        pager.PageSize = null;
                        var result_all = await _dataService.GetAll(search);

                        if (result_all != null)
                            count = result_all.Count();
                    }
                }

                var response = new
                {
                    List = (IList)Activator.CreateInstance(constructedListType),
                    Count = count
                };

                foreach (var r in result)
                    response.List.Add(_mapper.Get().Map(r, typeof(DB), type));
                return Ok(response);
            }
            catch (Exception ex)
            {
                var message = ex.GetBaseException().Message;
                _logger.LogError(message);
                return BadRequest(message);
            }
        }

        [HttpGet("{id}")]
        public virtual async Task<IActionResult> Get(string id, [FromQuery] bool min = false)
        {
            try
            {
                if (!_REST.GET.Allowed) return StatusCode(405, "Method not allowed");

                var result = await _dataService.Get(id);
                if (result == null) return NotFound();

                var type = min ? _REST.GET.MapToMin : _REST.GET.MapTo;
                if (type == null) return Ok(result);

                return Ok(_mapper.Get().Map(result, typeof(DB), type));
            }
            catch (Exception ex)
            {
                var message = ex.GetBaseException().Message;
                _logger.LogError(message);
                return BadRequest(message);
            }
        }

        [HttpPost("")]
        public virtual async Task<IActionResult> Create([FromBody]DB model, [FromQuery] bool min = false)
        {
            try
            {
                if (!_REST.POST.Allowed) return StatusCode(405, "Method not allowed");

                var result = await _dataService.Create(model);
                if (result == null)
                {
                    return BadRequest();
                }

                var type = min ? _REST.POST.MapToMin : _REST.POST.MapTo;
                if (type == null) return Ok(result);

                return Ok(_mapper.Get().Map(result, typeof(DB), type));
            }
            catch (Exception ex)
            {
                var message = ex.GetBaseException().Message;
                _logger.LogError(message);
                return BadRequest(message);
            }
        }

        [HttpPost("multi")]
        public virtual async Task<IActionResult> CreateMultiple([FromBody]List<DB> models, [FromQuery] bool min = false)
        {
            try
            {
                if (models == null || models.Count == 0) return BadRequest();

                var result = await _dataService.CreateMany(models);
                if (result == null)
                {
                    return BadRequest();
                }

                var type = min ? _REST.POST.MapToMin : _REST.POST.MapTo;
                if (type == null) return Ok(result);

                var listType = typeof(List<>);
                var constructedListType = listType.MakeGenericType(type);

                var response = (IList)Activator.CreateInstance(constructedListType);
                foreach (var r in result)
                    response.Add(_mapper.Get().Map(r, typeof(DB), type));
                return Ok(response);
            }
            catch (Exception ex)
            {
                var message = ex.GetBaseException().Message;
                _logger.LogError(message);
                return BadRequest(message);
            }
        }

        [HttpPatch("{id}")]
        public virtual async Task<IActionResult> Patch(string id, [FromBody]JsonPatchDocument<DB> patch, [FromQuery] bool min = false)
        {
            try
            {
                if (!_REST.PATCH.Allowed) return StatusCode(405, "Method not allowed");

                var result = await _dataService.Patch(id, patch);
                if (result == null)
                {
                    return NotFound();
                }

                var type = min ? _REST.PATCH.MapToMin : _REST.PATCH.MapTo;
                if (type == null) return Ok(result);

                return Ok(_mapper.Get().Map(result, typeof(DB), type));
            }
            catch (Exception ex)
            {
                var message = ex.GetBaseException().Message;
                _logger.LogError(message);
                return BadRequest(message);
            }
        }

        [HttpDelete("{id}")]
        public virtual async Task<IActionResult> Delete(string id, [FromQuery] bool min = true)
        {
            try
            {
                if (!_REST.DELETE.Allowed) return StatusCode(405, "Method not allowed");

                var result = await _dataService.Delete(id);
                if (result == null)
                {
                    return NotFound();
                }

                var type = min ? _REST.DELETE.MapToMin : _REST.DELETE.MapTo;
                if (type == null) return Ok(result);

                return Ok(_mapper.Get().Map(result, typeof(DB), type));
            }
            catch (Exception ex)
            {
                var message = ex.GetBaseException().Message;
                _logger.LogError(message);
                return BadRequest(message);
            }
        }

        [HttpPut("")]
        public virtual async Task<IActionResult> Put([FromBody]List<DB> models, [FromQuery]bool noBody = false)
        {
            try
            {
                if (!_REST.PUT.Allowed) return StatusCode(405, "Method not allowed");

                var result = await _dataService.CreateOrUpdateBulk(models);
                if (result == null)
                {
                    return NotFound();
                }

                if (noBody)
                {
                    return Ok(true);
                }
                else
                {
                    var type = _REST.PUT.MapTo;
                    if (type == null) return Ok(result.ToList());

                    var listType = typeof(List<>);
                    var constructedListType = listType.MakeGenericType(type);

                    var response = (IList)Activator.CreateInstance(constructedListType);
                    foreach (var r in result)
                        response.Add(_mapper.Get().Map(r, typeof(DB), type));
                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
                var message = ex.GetBaseException().Message;
                _logger.LogError(message);
                return BadRequest(message);
            }
        }
    }
}
