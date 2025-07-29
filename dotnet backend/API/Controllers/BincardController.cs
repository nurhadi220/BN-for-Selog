using Microsoft.AspNetCore.Mvc;

[Route("api/selog/bincard")]
[ApiController]
public class BincardController : ControllerBase
{
    private readonly DbManager _dbArofan;

    public BincardController(IConfiguration configuration)
    {
        _dbArofan = new DbManager(configuration);
    }

    [HttpGet]
    public IActionResult GetBincards()
    {
        var response = new Response();
        try
        {
            response.status = 200;
            response.pesan = "Success";
            response.data = _dbArofan.GetAllBincards();
        }
        catch (Exception ex)
        {
            response.status = 500;
            response.pesan = ex.Message;
        }

        return Ok(response);
    }

    [HttpPost]
    public IActionResult CreateBincard([FromBody] Bincard bincard)
    {
        var response = new Response();
        try
        {
            var result = _dbArofan.CreateBincard(bincard);
            response.status = 201;
            response.pesan = "Bincard berhasil ditambahkan";
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
    public IActionResult UpdateBincard(int id, [FromBody] Bincard bincard)
    {
        var response = new Response();
        try
        {
            var result = _dbArofan.UpdateBincard(id, bincard);
            response.status = 200;
            response.pesan = "Bincard berhasil diperbarui";
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
public IActionResult SoftDelete(int id)
{
    var response = new Response();
    try
    {
        var result = _dbArofan.SoftDeleteBincard(id);
        response.status = 200;
        response.pesan = "Bincard berhasil dihapus (soft delete)";
        response.data = result;
    }
    catch (Exception ex)
    {
        response.status = 500;
        response.pesan = ex.Message;
    }
    return Ok(response);
}
[HttpGet("semua/bincard")]
   public IActionResult GetAllSoftDelete()
    {
        var response = new Response();
        try
        {
            response.status = 200;
            response.pesan = "Success";
            response.data = _dbArofan.GetAllSoftDelete();
        }
        catch (Exception ex)
        {
            response.status = 500;
            response.pesan = ex.Message;
        }

        return Ok(response);
    }

}
