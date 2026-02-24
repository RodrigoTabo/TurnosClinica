using Azure.Core;
using Microsoft.EntityFrameworkCore;
using TurnosClinica.Application.DTOs.Medicos;
using TurnosClinica.Infrastructure.Data;
using TurnosClinica.Models;

namespace TurnosClinica.Application.Services.Medicos
{
    public class MedicosService : IMedicosService
    {
        private readonly TurnosDbContext _context;

        public MedicosService(TurnosDbContext context)
        {
            _context = context;
        }

        public async Task<int> CrearAsync(CrearMedicoRequest request)
        {
            // Validamos que el nombre no sea nulo o vacío
            var nombre = (request.Nombre ?? "").Trim();
            // Si el nombre es nulo, vacío o solo contiene espacios en blanco, lanzamos una excepción
            if (string.IsNullOrWhiteSpace(nombre))
                throw new InvalidOperationException("El nombre es obligatorio.");
            // Validamos que el apellido no sea nulo o vacío
            var apellido = (request.Apellido ?? "").Trim();
            // Si el apellido es nulo, vacío o solo contiene espacios en blanco, lanzamos una excepción
            if (string.IsNullOrWhiteSpace(apellido))
                throw new InvalidOperationException("El apellido es obligatorio.");
            // Validamos que el DNI no sea cero
            var dni = request.DNI;
            // Si el DNI es cero, lanzamos una excepción
            if (dni == 0)
                throw new InvalidOperationException("El DNI es obligatorio.");

            //Validamos si existe la especialidad seleccionada
            var especialidad = await _context.Especialidades.AnyAsync(e => e.Id == request.EspecialidadId);
            //Si la especialidad no existe, lanzamos una excepción
            if (!especialidad)
                throw new KeyNotFoundException("La especialidad no existe.");
            //Validamos si existe el consultorio seleccionada
            var consultorio = await _context.Consultorios.AnyAsync(c => c.Id == request.ConsultorioId);
            //Si la especialidad no existe, lanzamos una excepción
            if (!consultorio)
                throw new KeyNotFoundException("El consultorio no existe.");

            //Validamos si existe el Medico mediante su DNI
            var existedni = await _context.Medicos.AnyAsync(m => m.DNI == request.DNI);
            //Si el DNI ya existe, lanzamos una excepción
            if (existedni)
                throw new InvalidOperationException("El medico ya esta registrado");

            //Existe el medico pero se habia dado de baja?
            var existeDNIEliminado = await _context.Medicos.IgnoreQueryFilters()
                .AnyAsync(m => m.DNI == dni && m.EliminadoEn != null);
            if (existeDNIEliminado)
                throw new InvalidOperationException("El medico existe. Está dado de baja");


            //Si no existe, creamos el medico
            var medico = new Medico
            {
                Id = request.Id,
                Nombre = request.Nombre,
                Apellido = request.Apellido,
                DNI = request.DNI,
                EspecialidadId = request.EspecialidadId,
                ConsultorioId = request.ConsultorioId,
                Activo = true
            };
            // Agregamos el medico a la base de datos
            _context.Medicos.Add(medico);
            await _context.SaveChangesAsync();
            // Retornamos el Id del medico creado
            return medico.Id;
        }

        public async Task<List<MedicoResponse>> ListarAsync(string nombre)
        {
            // Obtenemos la lista de medicos que no han sido eliminados
            var query = _context.Medicos.AsNoTracking().Where(m => m.EliminadoEn == null);

            // Si se ha proporcionado un nombre, filtramos la lista por nombre o apellido que contenga el texto ingresado (ignorando mayúsculas y minúsculas)
            if (!string.IsNullOrWhiteSpace(nombre))
            {
                var n = nombre.Trim().ToLower();
                query = query.Where(m => m.Nombre.ToLower().Contains(n) || m.Apellido.ToLower().Contains(n));
            }
            // Seleccionamos los campos necesarios para la respuesta y obtenemos la lista de medicos
            var lista = await query
                .Select(m => new MedicoResponse
                {
                    Id = m.Id,
                    Nombre = m.Nombre,
                    Apellido = m.Apellido,
                    DNI = m.DNI,
                    EspecialidadId = m.EspecialidadId,
                    Especialidad = m.Especialidad.Nombre,
                    ConsultorioId = m.ConsultorioId,
                    Consultorio = m.Consultorio.Institucion + ", "
                    + m.Consultorio.Ciudad.Nombre + ", "
                    + m.Consultorio.Ciudad.Provincia.Nombre + ", "
                    + m.Consultorio.Ciudad.Provincia.Pais.Nombre
                })
                .ToListAsync();
            // Retornamos la lista de medicos
            return lista;

        }

        public async Task<List<MedicoResponse>> ListarPorConsultorioAsync(int consultorioId)
        {
            return await _context.Medicos
                .AsNoTracking()
                .Where(m => m.ConsultorioId == consultorioId && m.EliminadoEn == null)
                .Select(m => new MedicoResponse
                {
                    Id = m.Id,
                    Nombre = m.Nombre,
                    Apellido = m.Apellido,
                    DNI = m.DNI,
                    EspecialidadId = m.EspecialidadId,
                    Especialidad = m.Especialidad.Nombre,
                    ConsultorioId = m.ConsultorioId,
                    Consultorio = m.Consultorio.Institucion
                })
                .ToListAsync();
        }

        public async Task<MedicoResponse> GetByIdAsync(int id)
        {
            // Obtenemos el medico por su Id, asegurándonos de que no haya sido eliminado
            var medico = await _context.Medicos.AsNoTracking()
                .Where(m => m.Id == id && m.EliminadoEn == null)
                .Select(m => new MedicoResponse
                {
                    Id = m.Id,
                    Nombre = m.Nombre,
                    Apellido = m.Apellido,
                    DNI = m.DNI,
                    EspecialidadId = m.EspecialidadId,
                    Especialidad = m.Especialidad.Nombre,
                    ConsultorioId = m.ConsultorioId,
                    Consultorio = m.Consultorio.Institucion + ", "
                    + m.Consultorio.Ciudad.Nombre + ", "
                    + m.Consultorio.Ciudad.Provincia.Nombre + ", "
                    + m.Consultorio.Ciudad.Provincia.Pais.Nombre
                })
                .FirstOrDefaultAsync();
            // Si el medico no existe, lanzamos una excepción
            if (medico == null)
                throw new KeyNotFoundException("El medico no esta registrado");
            // Retornamos el medico encontrado
            return medico;
        }

        public async Task UpdateAsync(int id, UpdateMedicoRequest request)
        {
            // Obtenemos el medico por su Id, asegurándonos de que no haya sido eliminado
            var medico = await _context.Medicos
                .Where(m => m.Id == id && m.EliminadoEn == null)
                .FirstOrDefaultAsync();
            // Validamos que el nombre no sea nulo o vacío
            var nombre = (request.Nombre ?? "").Trim();
            //Si el nombre es nulo, vacío o solo contiene espacios en blanco, lanzamos una excepción
            if (string.IsNullOrWhiteSpace(nombre))
                throw new InvalidOperationException("El nombre es obligatorio.");
            // Validamos que el apellido no sea nulo o vacío
            var apellido = (request.Apellido ?? "").Trim();
            // Si el apellido es nulo, vacío o solo contiene espacios en blanco, lanzamos una excepción
            if (string.IsNullOrWhiteSpace(apellido))
                throw new InvalidOperationException("El apellido es obligatorio.");
            // Validamos que el DNI no sea cero
            var dni = request.DNI;
            // Si el DNI es cero, lanzamos una excepción
            if (dni == 0)
                throw new InvalidOperationException("El DNI es obligatorio.");

            //Buscamos el medico por su Id, asegurándonos de que no haya sido eliminado
            var especialidad = _context.Especialidades.Any(e => e.Id == request.EspecialidadId);
            // Si la especialidad no existe, lanzamos una excepción
            if (!especialidad)
                throw new KeyNotFoundException("La especialidad no existe.");

            //Validamos si existe el consultorio seleccionada
            var consultorio = await _context.Consultorios.AnyAsync(c => c.Id == request.ConsultorioId);
            //Si la especialidad no existe, lanzamos una excepción
            if (!consultorio)
                throw new KeyNotFoundException("El consultorio no existe.");

            //Buscamos que no exista otro medico con el mismo DNI, excluyendo al medico que estamos actualizando
            var existedni = _context.Medicos.Any(m => m.DNI == request.DNI);
            // Si el DNI ya existe, lanzamos una excepción
            if (existedni)
                throw new InvalidOperationException("El medico ya esta registrado");

            //Existe el medico pero se habia dado de baja?
            var existeDNIEliminado = await _context.Medicos.IgnoreQueryFilters()
                .AnyAsync(m => m.DNI == dni && m.EliminadoEn != null);
            if (existeDNIEliminado)
                throw new InvalidOperationException("El medico existe. Está dado de baja");


            // Si el medico no existe, lanzamos una excepción
            medico.Nombre = request.Nombre;
            medico.Apellido = request.Apellido;
            medico.DNI = request.DNI;
            medico.EspecialidadId = request.EspecialidadId;

            await _context.SaveChangesAsync();

        }


        public async Task SoftDeleteAsync(int id)
        {
            // Obtenemos el medico por su Id, asegurándonos de que no haya sido eliminado
            var medico = await _context.Medicos
                .Where(m => m.Id == id && m.EliminadoEn == null)
                .FirstOrDefaultAsync();
            // Si el medico no existe, lanzamos una excepción
            if (medico == null)
                throw new KeyNotFoundException("El medico no esta registrado");
            // Marcamos el medico como eliminado estableciendo la fecha de eliminación
            medico.EliminadoEn = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }

    }
}
