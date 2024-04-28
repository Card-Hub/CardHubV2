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
            var roomCode = "";
            for (var i = 0; i < 6; i++)
            {
                roomCode += new Random().Next(0, 9).ToString();
            }
            if (!_roomCodes.TryAdd(roomCode, gameType)) continue;

            return Ok(roomCode);
        }
        
        return BadRequest("Failed to create room");
    }
    
    [HttpGet("VerifyCode/{id}")]
    public ActionResult<bool> VerifyCode(string id)
    {
        return Ok(_roomCodes.ContainsKey(id));
    }
}