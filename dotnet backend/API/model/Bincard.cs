public class Bincard
{
    public int id { get; set; }
    public DateTime tanggal { get; set; }
    public string? ms { get; set; }
    public string? tujuan { get; set; }
    public int masuk { get; set; }
    public int keluar { get; set; }
    public int sisa { get; set; }
    public int id_picking { get; set; }     // Foreign Key ke Crew.id
    public string? id_barang { get; set; }      // Foreign Key ke Barang.id_barang
    public bool is_deleted { get; set; }
}
