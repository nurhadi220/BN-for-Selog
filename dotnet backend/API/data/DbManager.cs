using MySql.Data.MySqlClient;
using System.Data;

public class DbManager
{
    private readonly string _connectionString;

    public DbManager(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }

    private MySqlConnection GetConnection()
    {
        return new MySqlConnection(_connectionString);
    }

    // GET Semua Barang
    public List<Barang> GetAllBarangs()
    {
        List<Barang> BarangsList = new List<Barang>();
        try
        {
            using (MySqlConnection connection = GetConnection())
            {
                string query = "SELECT * FROM arofan";
                MySqlCommand command = new MySqlCommand(query, connection);
                connection.Open();

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Barang barang = new Barang
                        {
                            id_barang = reader["id_barang"].ToString(),
                            part_number = reader["part_number"]?.ToString(),
                            material_name = reader["material_name"]?.ToString(),
                            brand_name = reader["brand_name"]?.ToString(),
                            lokasi = reader["lokasi"]?.ToString(),
                            berat = Convert.ToDecimal(reader["berat"]),
                            panjang = Convert.ToInt32(reader["panjang"]),
                            lebar = Convert.ToInt32(reader["lebar"]),
                            tinggi = Convert.ToInt32(reader["tinggi"]),
                            stok = Convert.ToInt32(reader["stok"]),
                            id_gambar = reader["id_gambar"]?.ToString(),
                            gambar = reader["gambar"]?.ToString()
                        };
                        BarangsList.Add(barang);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Terjadi kesalahan: {ex.Message}");
        }

        return BarangsList;
    }

    // CREATE Barang
    public int CreateBarang(Barang barang)
    {
        using (var connection = GetConnection())
        {
            string query = @"INSERT INTO arofan 
                (id_barang, part_number, material_name, brand_name, lokasi, berat, panjang, lebar, tinggi, stok, id_gambar, gambar) 
                VALUES 
                (@IdBarang, @PartNumber, @MaterialName, @BrandName, @Lokasi, @Berat, @Panjang, @Lebar, @Tinggi, @Stok, @IdGambar, @Gambar)";
            
            using (var command = new MySqlCommand(query, connection))
            {
                string generatedId = Guid.NewGuid().ToString(); // UUID
                command.Parameters.AddWithValue("@IdBarang", generatedId);
                command.Parameters.AddWithValue("@PartNumber", barang.part_number);
                command.Parameters.AddWithValue("@MaterialName", barang.material_name);
                command.Parameters.AddWithValue("@BrandName", barang.brand_name);
                command.Parameters.AddWithValue("@Lokasi", barang.lokasi);
                command.Parameters.AddWithValue("@Berat", barang.berat);
                command.Parameters.AddWithValue("@Panjang", barang.panjang);
                command.Parameters.AddWithValue("@Lebar", barang.lebar);
                command.Parameters.AddWithValue("@Tinggi", barang.tinggi);
                command.Parameters.AddWithValue("@Stok", barang.stok);
                command.Parameters.AddWithValue("@IdGambar", barang.id_gambar);
                command.Parameters.AddWithValue("@Gambar", barang.gambar);

                connection.Open();
                return command.ExecuteNonQuery();
            }
        }
    }

    // UPDATE Barang
    public int UpdateBarang(string id, Barang barang)
    {
        using (var connection = GetConnection())
        {
            string query = @"UPDATE arofan SET 
                part_number = @PartNumber,
                material_name = @MaterialName,
                brand_name = @BrandName,
                lokasi = @Lokasi,
                berat = @Berat,
                panjang = @Panjang,
                lebar = @Lebar,
                tinggi = @Tinggi,
                stok = @Stok,
                id_gambar = @IdGambar,
                gambar = @Gambar
                WHERE id_barang = @IdBarang";
            
            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@PartNumber", barang.part_number);
                command.Parameters.AddWithValue("@MaterialName", barang.material_name);
                command.Parameters.AddWithValue("@BrandName", barang.brand_name);
                command.Parameters.AddWithValue("@Lokasi", barang.lokasi);
                command.Parameters.AddWithValue("@Berat", barang.berat);
                command.Parameters.AddWithValue("@Panjang", barang.panjang);
                command.Parameters.AddWithValue("@Lebar", barang.lebar);
                command.Parameters.AddWithValue("@Tinggi", barang.tinggi);
                command.Parameters.AddWithValue("@Stok", barang.stok);
                command.Parameters.AddWithValue("@IdGambar", barang.id_gambar);
                command.Parameters.AddWithValue("@Gambar", barang.gambar);
                command.Parameters.AddWithValue("@IdBarang", id);

                connection.Open();
                return command.ExecuteNonQuery();
            }
        }
    }

    // DELETE Barang
    public int DeleteBarang(string id)
    {
        using (var connection = GetConnection())
        {
            string query = "DELETE FROM arofan WHERE id_barang = @Id";
            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Id", id);

                connection.Open();
                return command.ExecuteNonQuery();
            }
        }
    }

    // GET all crew
public List<Crew> GetAllCrews()
{
    List<Crew> crews = new();
    using var connection = GetConnection();
    string query = "SELECT * FROM crew";
    using var command = new MySqlCommand(query, connection);
    connection.Open();
    using var reader = command.ExecuteReader();
    while (reader.Read())
    {
        crews.Add(new Crew
        {
            id = Convert.ToInt32(reader["id"]),
            nama = reader["nama"].ToString(),
            jabatan = reader["jabatan"].ToString()
        });
    }
    return crews;
}

// CREATE crew
public int CreateCrew(Crew crew)
{
    using var connection = GetConnection();
    string query = "INSERT INTO crew (id, nama, jabatan) VALUES (@IdCrew, @Nama, @Jabatan)";
    using var command = new MySqlCommand(query, connection);
    command.Parameters.AddWithValue("@IdCrew", crew.id);
    command.Parameters.AddWithValue("@Nama", crew.nama);
    command.Parameters.AddWithValue("@Jabatan", crew.jabatan);
    connection.Open();
    return command.ExecuteNonQuery();
}

// UPDATE crew
public int UpdateCrew(string id, Crew crew)
{
    using var connection = GetConnection();
    string query = "UPDATE crew SET nama = @Nama, jabatan = @Jabatan WHERE id = @IdCrew";
    using var command = new MySqlCommand(query, connection);
    command.Parameters.AddWithValue("@Nama", crew.nama);
    command.Parameters.AddWithValue("@Jabatan", crew.jabatan);
    command.Parameters.AddWithValue("@IdCrew", id);
    connection.Open();
    return command.ExecuteNonQuery();
}

// DELETE crew
public int DeleteCrew(string id)
{
    using var connection = GetConnection();
    string query = "DELETE FROM crew WHERE id = @IdCrew";
    using var command = new MySqlCommand(query, connection);
    command.Parameters.AddWithValue("@IdCrew", id);
    connection.Open();
    return command.ExecuteNonQuery();
}

public List<Bincard> GetAllBincards()
{
    List<Bincard> list = new List<Bincard>();
    using (var connection = GetConnection())
    {
        string query = "SELECT * FROM bincard WHERE is_deleted = FALSE";
        using (var command = new MySqlCommand(query, connection))
        {
            connection.Open();
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Bincard b = new Bincard
                    {
                        id = Convert.ToInt32(reader["id"]),
                        tanggal = Convert.ToDateTime(reader["tanggal"]),
                        ms = reader["ms"].ToString(),
                        tujuan = reader["tujuan"].ToString(),
                        masuk = Convert.ToInt32(reader["masuk"]),
                        keluar = Convert.ToInt32(reader["keluar"]),
                        sisa = Convert.ToInt32(reader["sisa"]),
                        id_picking = Convert.ToInt32(reader["id_picking"]),
                        id_barang = reader["id_barang"].ToString(),
                        is_deleted = Convert.ToBoolean(reader["is_deleted"])
                    };
                    list.Add(b);
                }
            }
        }
    }
    return list;
}

public int CreateBincard(Bincard bincard)
{
    using (var connection = GetConnection())
    {
        string query = @"
            INSERT INTO bincard 
            (tanggal, ms, tujuan, masuk, keluar, sisa, id_picking, id_barang, is_deleted) 
            VALUES 
            (@Tanggal, @Ms, @Tujuan, @Masuk, @Keluar, @Sisa, @IdPicking, @IdBarang, @IsDeleted)";
        
        using (var command = new MySqlCommand(query, connection))
        {
            command.Parameters.AddWithValue("@Tanggal", bincard.tanggal);
            command.Parameters.AddWithValue("@Ms", bincard.ms);
            command.Parameters.AddWithValue("@Tujuan", bincard.tujuan);
            command.Parameters.AddWithValue("@Masuk", bincard.masuk);
            command.Parameters.AddWithValue("@Keluar", bincard.keluar);
            command.Parameters.AddWithValue("@Sisa", bincard.sisa);
            command.Parameters.AddWithValue("@IdPicking", bincard.id_picking);
            command.Parameters.AddWithValue("@IdBarang", bincard.id_barang);
            command.Parameters.AddWithValue("@IsDeleted", bincard.is_deleted); // false = aktif, true = soft deleted

            connection.Open();
            return command.ExecuteNonQuery();
        }
    }
}

// UPDATE Bincard
public int UpdateBincard(int id, Bincard bincard)
{
    using (var connection = GetConnection())
    {
        string query = @"UPDATE bincard SET 
            tanggal = @Tanggal,
            ms = @Ms,
            tujuan = @Tujuan,
            masuk = @Masuk,
            keluar = @Keluar,
            sisa = @Sisa,
            id_picking = @IdPicking,
            id_barang = @IdBarang
            WHERE id = @Id";
        var command = new MySqlCommand(query, connection);

        command.Parameters.AddWithValue("@Tanggal", bincard.tanggal);
        command.Parameters.AddWithValue("@Ms", bincard.ms);
        command.Parameters.AddWithValue("@Tujuan", bincard.tujuan);
        command.Parameters.AddWithValue("@Masuk", bincard.masuk);
        command.Parameters.AddWithValue("@Keluar", bincard.keluar);
        command.Parameters.AddWithValue("@Sisa", bincard.sisa);
        command.Parameters.AddWithValue("@IdPicking", bincard.id_picking);
        command.Parameters.AddWithValue("@IdBarang", bincard.id_barang);
        command.Parameters.AddWithValue("@Id", id);

        connection.Open();
        return command.ExecuteNonQuery();
    }
}

// DELETE Bincard
public int SoftDeleteBincard(int id)
{
    using (var connection = GetConnection())
    {
        string query = "UPDATE bincard SET is_deleted = TRUE WHERE id = @Id";
        using (var command = new MySqlCommand(query, connection))
        {
            command.Parameters.AddWithValue("@Id", id);
            connection.Open();
            return command.ExecuteNonQuery();
        }
    }
}

public List<Bincard> GetAllSoftDelete()
{
    List<Bincard> list = new List<Bincard>();
    using (var connection = GetConnection())
    {
        string query = "SELECT * FROM bincard";
        using (var command = new MySqlCommand(query, connection))
        {
            connection.Open();
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Bincard b = new Bincard
                    {
                        id = Convert.ToInt32(reader["id"]),
                        tanggal = Convert.ToDateTime(reader["tanggal"]),
                        ms = reader["ms"].ToString(),
                        tujuan = reader["tujuan"].ToString(),
                        masuk = Convert.ToInt32(reader["masuk"]),
                        keluar = Convert.ToInt32(reader["keluar"]),
                        sisa = Convert.ToInt32(reader["sisa"]),
                        id_picking = Convert.ToInt32(reader["id_picking"]),
                        id_barang = reader["id_barang"].ToString(),
                        is_deleted = Convert.ToBoolean(reader["is_deleted"])
                    };
                    list.Add(b);
                }
            }
        }
    }
    return list;
}

}

