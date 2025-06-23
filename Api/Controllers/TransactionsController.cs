using System;
using Application.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransactionsController : ControllerBase
{
    //GETALL
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ResponseTransactionDto>>> GetTransactions()
    {
        return Ok();
    }
}