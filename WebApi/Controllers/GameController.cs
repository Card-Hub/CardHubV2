using Microsoft.AspNetCore.Mvc;
using WebApi.Services;

namespace WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class GameController : ControllerBase
{
    private IDictionary<string, GameType> _roomCodes;
    
    public GameController(IDictionary<string, GameType> roomCodes)
    {
        _roomCodes = roomCodes;
    }
    
    
    [HttpPost("CreateRoom")]
    public ActionResult<string> CreateRoom([FromBody] GameType gameType)
    {
        const int maxAttempts = 100;
        for (var j = 0; j < maxAttempts; j++) {
            var roomId = "";
            for (var i = 0; i < 6; i++)
            {
                roomId += new Random().Next(0, 9).ToString();
            }
            if (!_roomCodes.TryAdd(roomId, gameType)) continue;

            return Ok(roomId);
        }
        
        return BadRequest("Failed to create room");
    }
    
    
    [HttpGet("VerifyCode/{roomId}")]
    public ActionResult<GameType> VerifyCode(string roomId)
    {
        if (_roomCodes.TryGetValue(roomId, out var gameType)) return Ok(gameType);

        return NotFound();
    }
}