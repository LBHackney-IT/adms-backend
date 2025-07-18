using System;
using System.Linq.Expressions;
using Application.Apprentices;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ApprenticesController : Controller
{
   
    private readonly IReadRepository<Apprentice, ResponseApprenticeDto> _readRepository;
    private readonly IWriteRepository<Apprentice, WriteApprenticeDto> _writeRepository;
    
    public ApprenticesController(IReadRepository<Apprentice, ResponseApprenticeDto> readRepository, IWriteRepository<Apprentice, WriteApprenticeDto> writeRepository)
    {
        _readRepository = readRepository;
        _writeRepository = writeRepository;
    }
    
    [HttpGet("all")]
    [ProducesResponseType(typeof(List<ResponseApprenticeDto>), 200)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(statusCode: 400, type: typeof(ValidationProblemDetails))]
    public async Task<ActionResult<List<ResponseApprenticeDto>>> GetAll()
    {
        try
        {
            var apprentices = await _readRepository.GetAllAsync();
            return Ok(apprentices.ToList());

        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (Exception ex)
        {
            return BadRequest(new ValidationProblemDetails
            {
                Detail = ex.Message
            });
        }
    }
    
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ResponseApprenticeDto), 200)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(statusCode: 400, type: typeof(ValidationProblemDetails))]
    public async Task<ActionResult<Apprentice>> GetById(Guid id)
    {
        try
        {
            var apprentice = await _readRepository.GetByIdAsync(id);
            return Ok(apprentice);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (Exception ex)
        {
            return BadRequest(new ValidationProblemDetails
            {
                Detail = ex.Message
            });
        }
    }
    
    [HttpGet("uln/{uln}")]
    [ProducesResponseType(typeof(ResponseApprenticeDto), 200)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(statusCode: 400, type: typeof(ValidationProblemDetails))]
    public async Task<ActionResult<IEnumerable<Apprentice>>> GetByUlnAsync(decimal uln)
    {
        try
        {
            var apprentice = await _readRepository.GetByUlnAsync(uln);
            return Ok(apprentice);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (Exception ex)
        {
            return BadRequest(new ValidationProblemDetails
            {
                Detail = ex.Message
            });
        }
    }
    
   
    [HttpGet("find")]
    [ProducesResponseType(typeof(List<ResponseApprenticeDto>), 200)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(statusCode: 400, type: typeof(ValidationProblemDetails))]
    public async Task<ActionResult<List<ResponseApprenticeDto>>> Find(
        [FromQuery] DateTime? startDate = null,
        [FromQuery] DateTime? endDate = null,
        [FromQuery] string? status = null,
        [FromQuery] DirectorateCode? directorate = null,
        [FromQuery] ApprenticeshipProgram? apprenticeProgram = null)
    {
        try
        {
            Expression<Func<Apprentice, bool>> predicate = a =>
                (!startDate.HasValue || a.StartDate >= startDate.Value) &&
                (!endDate.HasValue || a.EndDate >= endDate.Value) &&
                (string.IsNullOrEmpty(status) || a.Status == status) &&
                (!directorate.HasValue || a.Directorate >= directorate.Value) &&
                (!apprenticeProgram.HasValue || a.ApprenticeProgram >= apprenticeProgram.Value);

            var apprentices = await _readRepository.FindAsync(predicate);

            return Ok(apprentices);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (Exception ex)
        {
            return BadRequest(new ValidationProblemDetails
            {
                Detail = ex.Message
            });
        }
    }
    
    [HttpPost("create")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(statusCode: 400, type: typeof(ValidationProblemDetails))]
    public async Task<ActionResult<WriteApprenticeDto>> ApprenticeCreate(WriteApprenticeDto apprenticeDto)
    {
        try
        {
            await _writeRepository.AddAsync(apprenticeDto);
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(new ValidationProblemDetails
            {
                Detail = ex.Message
            });
        }
    }
    
    [HttpPost("upload")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(statusCode: 400, type: typeof(ValidationProblemDetails))]
    public async Task<ActionResult<IEnumerable<WriteApprenticeDto>>> ApprenticeUpload(IEnumerable<WriteApprenticeDto> apprenticeDtos)
    {
        try
        {
            await _writeRepository.UploadAsync(apprenticeDtos);
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(new ValidationProblemDetails
            {
                Detail = ex.Message
            });
        }
    }
    
    
    [HttpPatch]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(statusCode: 400, type: typeof(ValidationProblemDetails))]
    public async Task<ActionResult> UpdateApprentice(Apprentice apprentice)
    {
        try
        {
            await _writeRepository.UpdateAsync(apprentice);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (Exception ex)
        {
            return BadRequest(new ValidationProblemDetails
            {
                Detail = ex.Message
            });
        }
    }
    
    [HttpDelete("{Id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(statusCode: 400, type: typeof(ValidationProblemDetails))]
    public async Task<IActionResult> DeleteApprentice(Guid id)
    {
        try
        {
            await _writeRepository.RemoveAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (Exception ex)
        {
            return BadRequest(new ValidationProblemDetails
            {
                Detail = ex.Message
            });
        }
    }

}