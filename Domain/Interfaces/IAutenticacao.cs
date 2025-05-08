using Domain.Entities;
using Domain.Entities.ViewModel;
using Domain.Requests;
using Domain.ViewModel;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Data;

namespace Domain.Interfaces
{
    public interface IAutenticacao
    {
        Task<string> GerarTokenJWT(DTOToken oDtoToken);
    }
}