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
    /// Метод для того чтобы задать правила игры
    /// </summary>
    /// <param name="request">Реквест, в котором хранятся правила игры</param>
    /// <returns></returns>
    /// <response code="200">Успешное выполнение, игра создается с выбранными настройками</response>
    [Route("initialize")]
    [HttpPost]
    public IActionResult InitializeGame(InitializeGameRequest request)
    {
        _gameManager.InitializeGame(request);
        return Ok("Игра начата. Начинайте угадывать число Угадайте число.");
    }

    [Route("guess")]
    [HttpGet]
    public IActionResult GuessNumber(int number)
    {
        var result = _gameManager.GuessNumber(number);
        return Ok(result);
    }
}