using System;
using System.Linq.Expressions;
using Application.Apprentices;
using Domain.Entities;
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
    public async Task<ActionResult<List<ResponseApprenticeDto>>> GetAll()
    {
        return await _readRepository.GetAllAsync();
    }
    
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ResponseApprenticeDto), 200)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<Transaction>> GetById(Guid id)
    {
        var apprentice = await _readRepository.GetByIdAsync(id);
        if (apprentice == null)
        {
            return NotFound();
        }

        return Ok(apprentice);
    }
    
    [HttpGet("uln/{uln}")]
    [ProducesResponseType(typeof(IEnumerable<Apprentice>), 200)]
    public async Task<ActionResult<IEnumerable<Apprentice>>> GetByUlnAsync(decimal uln)
    {
        var apprentice = await _readRepository.GetByUlnAsync(uln);
        return Ok(apprentice);
    }
    
    [HttpPost("find")]
    [ProducesResponseType(typeof(List<ResponseApprenticeDto>), 200)]
    public async Task<ActionResult<List<ResponseApprenticeDto>>> Find(
        [FromBody] FindApprentice request)
    {
        Expression<Func<Apprentice, bool>> predicate = a =>
            (!request.StartDate.HasValue || a.StartDate >= request.StartDate.Value) &&
            (!request.EndDate.HasValue || a.EndDate >= request.EndDate.Value) &&
            (string.IsNullOrEmpty(request.Status) || a.Status == request.Status) &&
            (!request.Directorate.HasValue || a.Directorate >= request.Directorate.Value) &&
            (!request.ApprenticeProgram.HasValue || a.ApprenticeProgram >= request.ApprenticeProgram.Value);

        var apprentices = await _readRepository.FindAsync(predicate);

        return Ok(apprentices);
    }
    
    [HttpPost("create")]
    [ProducesResponseType(typeof(Apprentice), 201)]
    public async Task<ActionResult<WriteApprenticeDto>> ApprenticeCreate(WriteApprenticeDto apprenticeDto)
    {
        await _writeRepository.AddAsync(apprenticeDto);
        return Ok();
    }
    
    [HttpPost("upload")]
    [ProducesResponseType(typeof(Apprentice), 201)]
    public async Task<ActionResult<IEnumerable<WriteApprenticeDto>>> ApprenticeUpload(IEnumerable<WriteApprenticeDto> apprenticeDtos)
    {
        await _writeRepository.UploadAsync(apprenticeDtos);
        return Ok();
    }
    
    
    [HttpPatch]
    [ProducesResponseType(typeof(Apprentice), 200)]
    public async Task<ActionResult> UpdateApprentice(Apprentice apprentice)
    {
        await _writeRepository.UpdateAsync(apprentice);
        return NoContent();
    }
    
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(Apprentice), 200)]
    public async Task<IActionResult> DeleteApprentice(Guid id)
    {
        await _writeRepository.RemoveAsync(id);
        return NoContent();
    }

}