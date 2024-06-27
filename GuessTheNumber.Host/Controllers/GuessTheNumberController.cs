using GuessTheNumber.Abstractions.Interfaces;
using GuessTheNumber.Application.Models.Requests;
using Microsoft.AspNetCore.Mvc;

namespace GuessTheNumber.Host.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GuessTheNumberController : ControllerBase
{
    private readonly IGameManager _gameManager;

    public GuessTheNumberController(IGameManager gameManager)
    {
        _gameManager = gameManager;
    }

    /// <summary>
    /// Метод для того чтобы задать правила игры, он создает сессию игры и сохраняет в SqlLite, в любое время можно подключиться к этой сессии
    /// </summary>
    /// <param name="request">Реквест, в котором хранятся правила игры</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns></returns>
    /// <response code="200">Успешное выполнение, игра создается с выбранными настройками</response>
    [Route("initialize")]
    [HttpPost]
    public async Task<IActionResult> InitializeGame([FromBody] InitializeGameRequest request,
        CancellationToken cancellationToken)
    {
        var sessionId = await _gameManager.InitializeGame(request, cancellationToken);
        return Ok($"Игра начата. Начинайте угадывать число, Ваша сессия - {sessionId}.");
    }

    /// <summary>
    /// Метод, который угадывает номер в определенной сессии
    /// </summary>
    /// <param name="sessionId">Id Сессии</param>
    /// <param name="guessedNumber">Преполагаемое число</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns></returns>
    /// <response code="200">Успешное выполнение, игра начнется</response>
    [Route("guess/{sessionId:guid}")]
    [HttpGet]
    public async Task<IActionResult> GuessNumber([FromRoute] Guid sessionId, [FromQuery] int guessedNumber,
        CancellationToken cancellationToken)
    {
        var result = await _gameManager.GuessNumber(sessionId, guessedNumber, cancellationToken);

        return Ok(result);
    }
}