using AleedShooping.Data.Entities;
using AleedShooping.Enums;
using AleedShooping.Helpers;

namespace AleedShooping.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;

        public SeedDb(DataContext context, IUserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();
            await CheckCountriesAsync();
            await CheckCategoriesAsync();
            await CheckRolesAsync();
            await CheckUserAsync("1017240121", "Alejandro", "Galeano", "alejandrogaleanomadrigal@gmail.com", "305 432 5671", "Calle 33 # 42 - 53", UserType.Admin);
            await CheckUserAsync("1152708952", "Edwin", "Correa", "edwinvelasquezcorrea@yopmail.com", "319 560 5554", "Calle 33 # 42 - 53", UserType.User);
            
        }

        private async Task<User> CheckUserAsync(
   string document,
   string firstName,
   string lastName,
   string email,
   string phone,
   string address,
   UserType userType)
        {
            User user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                user = new User
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    UserName = email,
                    PhoneNumber = phone,
                    Address = address,
                    Document = document,
                    City = _context.Cities.FirstOrDefault(),
                    UserType = userType,
                    
                };

                await _userHelper.AddUserAsync(user, "123456");
                await _userHelper.AddUserToRoleAsync(user, userType.ToString());

            }

            return user;
        }
        private async Task CheckRolesAsync()
        {
            await _userHelper.CheckRoleAsync(UserType.Admin.ToString());
            await _userHelper.CheckRoleAsync(UserType.User.ToString());
        }
        private async Task CheckCountriesAsync()
        {
            if (!_context.Countries.Any())
            {
                _context.Countries.Add(new Country
                {
                    Name = "Colombia",
                    States = new List<State>()
            {
                new State()
                {
                    Name = "Antioquia",
                    Cities = new List<City>() {
                        new City() { Name = "Medellín" },
                        new City() { Name = "Itagüí" },
                        new City() { Name = "Envigado" },
                        new City() { Name = "Bello" },
                        new City() { Name = "Sabaneta" },
                        new City() { Name = "La Estrella" },
                        new City() { Name = "Copacabana" },
                    }
                },

            }
                });


            }

            await _context.SaveChangesAsync();
        }

        private async Task CheckCategoriesAsync()
        {
            if (!_context.Categories.Any())
            {
                _context.Categories.Add(new Category { Name = "Dulces" });
                _context.Categories.Add(new Category { Name = "Globos" });
                _context.Categories.Add(new Category { Name = "Peluches" });
                _context.Categories.Add(new Category { Name = "Flores" });
                _context.Categories.Add(new Category { Name = "Empaques" });
                _context.Categories.Add(new Category { Name = "Decoracion" });
                _context.Categories.Add(new Category { Name = "Accesorios" });
                _context.Categories.Add(new Category { Name = "Belleza" });
                _context.Categories.Add(new Category { Name = "Nutrición" });
                _context.Categories.Add(new Category { Name = "Celebraciones" });
                _context.Categories.Add(new Category { Name = "Ropa" });

            }
            await _context.SaveChangesAsync();
        }
    }
}
