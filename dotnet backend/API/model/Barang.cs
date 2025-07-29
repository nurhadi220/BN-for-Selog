public class Barang
{
    public string? id_barang { get; set; }
    public string? part_number { get; set; }
    public string? material_name { get; set; }
    public string? brand_name { get; set; }
    public string? lokasi { get; set; }
    public decimal berat { get; set; }
    public int panjang { get; set; }
    public int lebar { get; set; }
    public int tinggi { get; set; }
    public int stok { get; set; }
    public IFormFile? foto { get; set; }
    public string? id_gambar { get; set; }
    public string? gambar { get; set; }
}
