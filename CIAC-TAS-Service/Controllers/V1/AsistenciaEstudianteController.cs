﻿using AutoMapper;
using CIAC_TAS_Service.Contracts.V1;
using CIAC_TAS_Service.Contracts.V1.Requests;
using CIAC_TAS_Service.Contracts.V1.Requests.Queries;
using CIAC_TAS_Service.Contracts.V1.Responses;
using CIAC_TAS_Service.Domain;
using CIAC_TAS_Service.Domain.Estudiante;
using CIAC_TAS_Service.Helpers;
using CIAC_TAS_Service.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CIAC_TAS_Service.Controllers.V1
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
    [Produces("application/json")]
    public class AsistenciaEstudianteController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IAsistenciaEstudianteService _asistenciaEstudianteService;
        private readonly IUriService _uriService;

        public AsistenciaEstudianteController(IMapper mapper, IAsistenciaEstudianteService asistenciaEstudianteService, IUriService uriService)
        {
            _mapper = mapper;
            _asistenciaEstudianteService = asistenciaEstudianteService;
            _uriService = uriService;
        }

        [HttpGet(ApiRoute.AsistenciaEstudiantes.GetAll)]
        [ProducesResponseType(typeof(AsistenciaEstudianteResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAll([FromQuery] PaginationQuery paginationQuery)
        {
            var pagination = _mapper.Map<PaginationFilter>(paginationQuery);
            var asistenciaEstudiantes = await _asistenciaEstudianteService.GetAsistenciaEstudiantesAsync(pagination);
            var asistenciaEstudianteResponses = _mapper.Map<List<AsistenciaEstudianteResponse>>(asistenciaEstudiantes);

            if (pagination == null || pagination.PageNumber < 1 || pagination.PageSize < 1)
            {
                return Ok(new PagedResponse<AsistenciaEstudianteResponse>(asistenciaEstudianteResponses));
            }

            var paginationResponse = PaginationHelpers.CreatePaginatedResponse(_uriService, pagination, asistenciaEstudianteResponses);

            return Ok(paginationResponse);
        }

        [HttpGet(ApiRoute.AsistenciaEstudiantes.Get)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(AsistenciaEstudianteResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get([FromRoute] int asistenciaEstudianteId)
        {
            var asistenciaEstudiante = await _asistenciaEstudianteService.GetAsistenciaEstudianteByIdAsync(asistenciaEstudianteId);

            if (asistenciaEstudiante == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<AsistenciaEstudianteResponse>(asistenciaEstudiante));
        }

        [HttpPost(ApiRoute.AsistenciaEstudiantes.Create)]
        [ProducesResponseType(typeof(AsistenciaEstudianteResponse), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateAsistenciaEstudianteRequest asistenciaEstudianteRequest)
        {
            var asistenciaEstudiante = new AsistenciaEstudiante
            {
                EstudianteId = asistenciaEstudianteRequest.EstudianteId,
                AsistenciaEstudianteHeaderId = asistenciaEstudianteRequest.AsistenciaEstudianteHeaderId
            };

            var created = await _asistenciaEstudianteService.CreateAsistenciaEstudianteAsync(asistenciaEstudiante);

            if (!created)
            {
                return BadRequest(new ErrorResponse
                {
                    Errors = new List<ErrorModel>
                {
                    new ErrorModel { Message = "Unable to create [AsistenciaEstudiante]"}
                }
                });
            }

            var locationUri = _uriService.GetAsistenciaEstudianteUri(asistenciaEstudiante.Id.ToString());

            var response = _mapper.Map<AsistenciaEstudianteResponse>(asistenciaEstudiante);

            return Created(locationUri, response);
        }

        [HttpPut(ApiRoute.AsistenciaEstudiantes.Update)]
        [ProducesResponseType(typeof(AsistenciaEstudianteResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Update([FromRoute] int asistenciaEstudianteId, [FromBody] UpdateAsistenciaEstudianteRequest request)
        {
            var asistenciaEstudiante = await _asistenciaEstudianteService.GetAsistenciaEstudianteByIdAsync(asistenciaEstudianteId);
            
            asistenciaEstudiante.EstudianteId = request.EstudianteId;
            asistenciaEstudiante.AsistenciaEstudianteHeaderId = request.AsistenciaEstudianteHeaderId;

            var update = await _asistenciaEstudianteService.UpdateAsistenciaEstudianteAsync(asistenciaEstudiante);

            if (!update)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<AsistenciaEstudianteResponse>(asistenciaEstudiante));
        }

        [HttpDelete(ApiRoute.AsistenciaEstudiantes.Delete)]
        [ProducesResponseType(typeof(AsistenciaEstudianteResponse), (int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Delete([FromRoute] int asistenciaEstudianteId)
        {
            var deleted = await _asistenciaEstudianteService.DeleteAsistenciaEstudianteAsync(asistenciaEstudianteId);

            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
