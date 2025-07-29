using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

[Route("api/selog/Barang")]
[ApiController]
public class BarangController : ControllerBase
{
    private readonly DbManager _dbArofan;

    public BarangController(IConfiguration configuration)
    {
        _dbArofan = new DbManager(configuration);
    }

    [HttpGet]
    public IActionResult GetBarangs()
    {
        var response = new Response();

        try
        {
            response.status = 200;
            response.pesan = "Success";
            response.data = _dbArofan.GetAllBarangs();
        }
        catch (Exception ex)
        {
            response.status = 500;
            response.pesan = ex.Message;
            response.data = null;
        }

        return Ok(response);
    }

    [HttpPost("upload")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> CreateBarangWithUpload([FromForm] Barang dto)
    {
        var response = new Response();
        try
        {
            var idGambar = Guid.NewGuid().ToString();
            string urlGambar = "";

            if (dto.foto != null)
            {
                urlGambar = await UploadImageKit(dto.foto, idGambar + ".jpg");
            }

            var barang = new Barang
            {
                id_barang = idGambar,
                part_number = dto.part_number,
                material_name = dto.material_name,
                brand_name = dto.brand_name,
                lokasi = dto.lokasi,
                berat = dto.berat,
                panjang = dto.panjang,
                lebar = dto.lebar,
                tinggi = dto.tinggi,
                stok = dto.stok,
                id_gambar = idGambar,
                gambar = urlGambar
            };

            var result = _dbArofan.CreateBarang(barang);

            response.status = 201;
            response.pesan = "Barang berhasil ditambahkan";
            response.data = result;
        }
        catch (Exception ex)
        {
            response.status = 500;
            response.pesan = ex.Message;
        }

        return Ok(response);
    }

    [HttpPut("edit/{id}")]
[Consumes("multipart/form-data")]
public async Task<IActionResult> UpdateBarangWithUpload(string id, [FromForm] Barang dto)
{
    var response = new Response();
    try
    {
        string urlGambar = dto.gambar; // Default: gambar lama
        string idGambar = dto.id_gambar; // Default: ID gambar lama

        if (dto.foto != null)
        {
            // Kalau user upload gambar baru
            urlGambar = await UploadImageKit(dto.foto, idGambar + ".jpg");
        }

        var barang = new Barang
        {
            id_barang = id,
            part_number = dto.part_number,
            material_name = dto.material_name,
            brand_name = dto.brand_name,
            lokasi = dto.lokasi,
            berat = dto.berat,
            panjang = dto.panjang,
            lebar = dto.lebar,
            tinggi = dto.tinggi,
            stok = dto.stok,
            id_gambar = idGambar,
            gambar = urlGambar
        };

        var result = _dbArofan.UpdateBarang(id, barang);
        response.status = 200;
        response.pesan = "Barang berhasil diperbarui";
        response.data = result;
    }
    catch (Exception ex)
    {
        response.status = 500;
        response.pesan = ex.Message;
    }

    return Ok(response);
}


    [HttpDelete("hapus/{id}")]
    public IActionResult DeleteBarang(string id)
    {
        var response = new Response();
        try
        {
            var result = _dbArofan.DeleteBarang(id);
            response.status = 200;
            response.pesan = "Barang berhasil dihapus";
            response.data = result;
        }
        catch (Exception ex)
        {
            response.status = 500;
            response.pesan = ex.Message;
        }

        return Ok(response);
    }
    

    // ⬇️ Fungsi Upload ke ImageKit
    private async Task<string> UploadImageKit(IFormFile file, string fileName)
    {
        var privateKey = "private_XlVDZa4PnXuNMjUqSXb6z97I5VM="; // ← Ganti dengan Private API Key dari ImageKit
        var uploadUrl = "https://upload.imagekit.io/api/v1/files/upload";

        using var httpClient = new HttpClient();

        using var content = new MultipartFormDataContent();
        using var streamContent = new StreamContent(file.OpenReadStream());

        streamContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
        content.Add(streamContent, "file", fileName);

        content.Add(new StringContent(fileName), "fileName");
        content.Add(new StringContent("/selog"), "folder"); // opsional: simpan ke folder tertentu
        content.Add(new StringContent("false"), "useUniqueFilename");

        // Tambahkan Authorization Header
        var authHeader = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes($"{privateKey}:"));
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authHeader);

        var response = await httpClient.PostAsync(uploadUrl, content);
        var responseContent = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Upload gagal: " + responseContent);
        }

        // Parse JSON dan ambil URL
        using var jsonDoc = System.Text.Json.JsonDocument.Parse(responseContent);
        var imageUrl = jsonDoc.RootElement.GetProperty("url").GetString();

        return imageUrl ?? "";
    }


}
