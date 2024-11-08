using API.Dtos;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Services
{
    public class EmpresaService : IEmpresaService
    {
        private readonly IUnitOfWork _unitOfWork;

        public EmpresaService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<DatosAddUpdateDto> RegisterEmpresaAsync(EmpresaAddUpdateDto empresaAddDto, string rucFilial, string docProfesional)
        {
            DatosAddUpdateDto datosMostrar = new DatosAddUpdateDto();
            datosMostrar.mensaje = "";

            var empresa = new Empresa
            {
                nombre = empresaAddDto.nombre,
                email = empresaAddDto.email.ToLower().Replace(" ", ""),
                ruc = empresaAddDto.ruc.Replace(" ", ""),
                razonSocial = empresaAddDto.razonSocial.Trim(),
                comentarios = empresaAddDto.comentarios
            };
            empresa.idTipoUsuario = _unitOfWork.TiposUsuarios
                                    .Find(tu => tu.rol == "Cliente")
                                    .FirstOrDefault()?.id ?? 0;

            var emailUnico = _unitOfWork.Empresas
                                        .Find(e => e.email.ToLower() == empresaAddDto.email.Replace(" ", "").ToLower())
                                        .FirstOrDefault();
            var rucUnico = await _unitOfWork.Empresas
                                        .GetByIdAsync(empresaAddDto.ruc.Replace(" ", "").ToLower());
            var razonSocialUnica = _unitOfWork.Empresas
                                        .Find(e => e.razonSocial.ToLower().Replace(" ", "") == empresaAddDto.razonSocial.Replace(" ", "").ToLower())
                                        .FirstOrDefault();
            if (rucFilial =="null")
                rucFilial = null;
            if (docProfesional=="null")
                docProfesional = null;

            if (rucUnico == null)
            {
                if (emailUnico == null)
                {
                    if (razonSocialUnica == null)
                    {
                        ClienteRegistro registroEmpresaPrimeraVez = new ClienteRegistro
                        {
                            documentoProfesional = docProfesional,
                            rucFilial = rucFilial,
                            rucEmpresa = empresaAddDto.ruc,
                            documentoCliente = null,
                        };

                        _unitOfWork.Empresas.Add(empresa);
                        _unitOfWork.ClientesRegistrados.Add(registroEmpresaPrimeraVez);

                        await _unitOfWork.SaveAsync();

                        datosMostrar.error = "";
                        datosMostrar.mensaje = $"La empresa {empresa.razonSocial} ha sido registrada correctamente.";

                        return datosMostrar;
                    }
                    else
                    {
                        datosMostrar.error = $"Ya existe una empresa registrada con esa Razón Social.";
                        return datosMostrar;
                    }
                }
                else
                {
                    datosMostrar.error = $"Ya existe una empresa registrada con ese email.";
                    return datosMostrar;
                }
            }
            else
            {
                datosMostrar.error = $"Ya existe una empresa registrada con ese RUC.";
                return datosMostrar;
            }
        }

        public async Task<DatosAddUpdateDto> UpdateEmpresaAsync(Empresa empresaExiste, [FromBody] EmpresaAddUpdateDto empresaUpdateDto)
        {
            DatosAddUpdateDto datosMostrar = new DatosAddUpdateDto();
            datosMostrar.mensaje = "";
            Empresa emailUnico = null;
            Empresa razonSocialUnica = null;

            if (empresaUpdateDto.nombre != null)
                empresaExiste.nombre = empresaUpdateDto.nombre.Trim();

            if (empresaUpdateDto.comentarios != null)
                empresaExiste.comentarios = empresaUpdateDto.comentarios.Trim();

            if (empresaExiste.email != empresaUpdateDto.email.Replace(" ", ""))
            {
                empresaExiste.email = empresaUpdateDto.email.Replace(" ", "").ToLower();
                emailUnico = _unitOfWork.Empresas
                                        .Find(e => e.email.ToLower() == empresaUpdateDto.email.Replace(" ", "").ToLower())
                                        .FirstOrDefault();
            }

            if (empresaExiste.razonSocial != empresaUpdateDto.razonSocial.Trim())
            {
                empresaExiste.razonSocial = empresaUpdateDto.razonSocial.Trim();
                razonSocialUnica = _unitOfWork.Empresas
                                        .Find(e => e.razonSocial.ToLower() == empresaUpdateDto.razonSocial.Trim())
                                        .FirstOrDefault();
            }

            if (emailUnico == null)
            {
                if (razonSocialUnica == null)
                {
                    await _unitOfWork.SaveAsync();

                    datosMostrar.error = "";
                    datosMostrar.mensaje = $"La empresa {empresaExiste.razonSocial} ha sido actualizada correctamente.";
                    return datosMostrar;
                }
                else
                {
                    datosMostrar.error = $"Ya existe una empresa registrada con esa Razón Social.";
                    return datosMostrar;
                }
            }
            else
            {
                datosMostrar.error = $"Ya existe una empresa registrada con ese email.";
                return datosMostrar;
            }
        }
    }
}
