﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIAC_TAS_Service.Contracts.V1.Responses
{
    public class RespuestasAsaResponse
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int ConfiguracionId { get; set; }
        public int PreguntaAsaId { get; set; }
        public DateTime FechaEntrada { get; set; }
        public int OpcionSeleccionadaId { get; set; }
        public bool EsExamen { get; set; }
    }
}
