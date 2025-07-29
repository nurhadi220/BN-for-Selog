using Microsoft.AspNetCore.Mvc;

[Route("api/selog/crew")]
[ApiController]
public class CrewController : ControllerBase
{
    private readonly DbManager _dbArofan;

    public CrewController(IConfiguration configuration)
    {
        _dbArofan = new DbManager(configuration);
    }

    [HttpGet]
    public IActionResult GetCrews()
    {
        var response = new Response();
        try
        {
            response.status = 200;
            response.pesan = "Success";
            response.data = _dbArofan.GetAllCrews();
        }
        catch (Exception ex)
        {
            response.status = 500;
            response.pesan = ex.Message;
        }

        return Ok(response);
    }

    [HttpPost]
    public IActionResult CreateCrew([FromBody] Crew crew)
    {
        var response = new Response();
        try
        {
            var result = _dbArofan.CreateCrew(crew);
            response.status = 201;
            response.pesan = "Crew berhasil ditambahkan";
            response.data = result;
        }
        catch (Exception ex)
        {
            response.status = 500;
            response.pesan = ex.Message;
        }

        return Ok(response);
    }

    [HttpPut("{id}")]
    public IActionResult UpdateCrew(string id, [FromBody] Crew crew)
    {
        var response = new Response();
        try
        {
            var result = _dbArofan.UpdateCrew(id, crew);
            response.status = 200;
            response.pesan = "Crew berhasil diperbarui";
            response.data = result;
        }
        catch (Exception ex)
        {
            response.status = 500;
            response.pesan = ex.Message;
        }

        return Ok(response);
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteCrew(string id)
    {
        var response = new Response();
        try
        {
            var result = _dbArofan.DeleteCrew(id);
            response.status = 200;
            response.pesan = "Crew berhasil dihapus";
            response.data = result;
        }
        catch (Exception ex)
        {
            response.status = 500;
            response.pesan = ex.Message;
        }

        return Ok(response);
    }
}
