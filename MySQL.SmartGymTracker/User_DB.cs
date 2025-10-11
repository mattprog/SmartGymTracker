using System.Data;
using MySql.Data.MySqlClient;

namespace MySQL.SmartGymTracker
{
    // NOTE filebase does not check if directory exsits
    //   Assumes that the directory exists
    public class User_DB
    {
        private readonly Database _db;

        public User_DB()
        {
            _db = new DB();
        }

        public List<User> GetAllUsers()
        {
            string sql = "SELECT * FROM users";
            var dbreturn = _db.ExecuteSelect(sql);
            List<User> users = new List<User>();
            // TODO convert DataTable to List<User>
            return users;
        }

        public User? AddUser(User user)
        {
            // TODO setup for real user
            string sql = "INSERT INTO users (username, email, password_hash) VALUES (@username, @email, @password_hash)";
            var parameters = new MySqlParameter[]
            {
                new MySqlParameter("@username", user.Username),
                new MySqlParameter("@email", user.Email),
                new MySqlParameter("@password_hash", user.PasswordHash)
            };
            _db.ExecuteNonQuery(sql, parameters);

            // Get updated records include all values
            string selectSql = "SELECT * FROM users WHERE username = @username";
            var result = _db.ExecuteSelect(selectSql, parameters);

            if (result.Rows.Count > 0)
            {
                // TODO convert to User
                return result.Rows[0];
            }
            return null;
        }

        public List<User> UpdateUser(User user)
        {
            // TODO setup for real user, get non NULL
            string sql = "UPDATE users SET username = @username, email = @email, password_hash = @password_hash WHERE id = @id";
            var parameters = new MySqlParameter[]
            {
                new MySqlParameter("@username", user.Username),
                new MySqlParameter("@email", user.Email),
                new MySqlParameter("@password_hash", user.PasswordHash),
                new MySqlParameter("@id", user.Id)
            };
            _db.ExecuteNonQuery(sql, parameters);

            string selectSql = "SELECT * FROM users WHERE id = @id";
            var result = _db.ExecuteSelect(selectSql, parameters);
            if(result.Rows.Count > 0)
            {
                // TODO convert to User
                List<User> users = new List<User>();
                return users;
            }
            return null;
        }

        public List<User> DeleteUser(int userId)
        {
            // TODO setup for real user, get non NULL
            string sqlSelect = "SELECT * FROM users WHERE id = @id";
            var parameters = new MySqlParameter[]
            {
                new MySqlParameter("@id", userId)
            };
            var result = _db.ExecuteSelect(sqlSelect, paramaters);
            if(result.Rows.Count == 0)
            {
                // User not found
                return;
            }
            // TODO convert to User if needed
            List<User> users = new List<User_DB>();

            string sql = "DELETE FROM users WHERE id = @id";
            _db.ExecuteNonQuery(sql, parameters);

            return users;
        }
    }
}

/*
        private string _dir;
        private string _inventoryRoot;
        private static InventoryFilebase? _instance;


        /*
         * Makes filebase a singlton
         
public static InventoryFilebase Current
{
    get
    {
        if (_instance == null)
        {
            _instance = new InventoryFilebase();
        }

        return _instance;
    }
}


/*
 * Constructor that gets location to save files
 
private InventoryFilebase()
{
    _dir = Directory.GetCurrentDirectory() + "\\Database\\Filebase\\";
    _inventoryRoot = $"{_dir}\\Inventory";
}


/*
 * For creating unique sku number
 
public int LastKey
{
    get
    {
        if (Inventory.Any())
        {
            return Inventory.Select(x => x.Sku).Max();
        }
        return 0;
    }
}


/*
 * For creating new product or updating existing product
 
public Product AddOrUpdate(Product product)
{
    //set up a new Id if one doesn't already exist
    if (product.Sku <= 0)
    {
        product.Sku = LastKey + 1;
    }

    //Go to filepath of specified product
    string path = $"{_inventoryRoot}\\{product.Sku}.json";


    //If product already existed delete it for updating
    if (File.Exists(path))
    {
        File.Delete(path);
    }

    //Create the new/updated product file
    File.WriteAllText(path, JsonConvert.SerializeObject(product));

    //return the product, which now has an unique id
    return product;
}


/*
 * For returning all products in inventory
 
public List<Product> Inventory
{
    get
    {
        var root = new DirectoryInfo(_inventoryRoot);
        var _products = new List<Product>();
        foreach (var productFile in root.GetFiles())
        {
            var product = JsonConvert
                .DeserializeObject<Product>
                (File.ReadAllText(productFile.FullName));
            if (product != null)
            {
                _products.Add(product);
            }
        }
        return _products;
    }
}


/*
 * Find product in filebase by id and delete it if id exists
 
public Product? Delete(int sku)
{
    //Go to filepath of specified product
    string path = $"{_inventoryRoot}\\{sku}.json";

    //Find file and delete it
    if (!File.Exists(path))
    {
        return null;
    }
    var deletedProd = Inventory.FirstOrDefault(i => i.Sku == sku);
    File.Delete(path);
    return deletedProd;
*/