using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class GameController : ControllerBase
{
    private HashSet<string> _roomCodes;
    
    public GameController(HashSet<string> roomCodes)
    {
        _roomCodes = roomCodes;
    }
    
    [HttpPost("CreateRoom")]
    public ActionResult<string> CreateRoom()
    {
        var maxAttempts = 100;
        for (var j = 0; j < maxAttempts; j++) {
            var roomCode = "";
            for (var i = 0; i < 6; i++)
            {
                // generate a random 6 digit code
                roomCode += new Random().Next(0, 9).ToString();
            }

            if (_roomCodes.Contains(roomCode)) continue;
            _roomCodes.Add(roomCode);
            return Ok(roomCode);
        }
        
        return BadRequest("Failed to create room");
    }
    
    [HttpGet("VerifyCode/{id}")]
    public ActionResult<bool> VerifyCode(string id)
    {
        // Console.WriteLine($"Room code entered: --{id}--");
        // Console.WriteLine(id);
        // foreach (var roomCode in _roomCodes)
        // {
        //     Console.WriteLine(roomCode);
        // }
        return Ok(_roomCodes.Contains(id));
    }
}