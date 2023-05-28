﻿using CIAC_TAS_Service.Contracts.V1.Requests;
using CIAC_TAS_Service.Contracts.V1.Responses;
using Refit;
using static CIAC_TAS_Service.Contracts.V1.ApiRoute;

namespace CIAC_TAS_Service.Sdk
{
    [Headers("Authorization: Bearer")]
    public interface IEstudianteMateriaServiceApi
    {
        [Get("/" + EstudianteMaterias.GetAll)]
        Task<ApiResponse<PagedResponse<EstudianteMateriaResponse>>> GetAllAsync();

        [Get("/" + EstudianteMaterias.Get)]
        Task<ApiResponse<EstudianteMateriaResponse>> GetAsync(int estudianteId, int grupoId, int materiaId);

        [Post("/" + EstudianteMaterias.Create)]
        Task<ApiResponse<EstudianteMateriaResponse>> CreateAsync([Body] CreateEstudianteMateriaRequest estudianteMateriaRequest);

        [Delete("/" + EstudianteMaterias.Delete)]
        Task<ApiResponse<EstudianteMateriaResponse>> DeleteAsync(int estudianteId, int grupoId, int materiaId);
    }
}
