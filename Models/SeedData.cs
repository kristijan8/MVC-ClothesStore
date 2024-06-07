using cstore.Areas.Identity.Data;
using cstore.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;

namespace cstore.Models
{
    public class SeedData
    {

        public static async Task CreateUserRoles(IServiceProvider serviceProvider)
        {
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<cstoreUser>>();
            IdentityResult roleResult;
            //Add Admin Role
            var roleCheck = await RoleManager.RoleExistsAsync("Admin");
            if (!roleCheck) { roleResult = await RoleManager.CreateAsync(new IdentityRole("Admin")); }
            cstoreUser user = await UserManager.FindByEmailAsync("admin@mvcmovie.com");
            if (user == null)
            {
                var User = new cstoreUser();
                User.Email = "admin@mvcmovie.com";
                User.UserName = "admin@mvcmovie.com";
                string userPWD = "Admin123";
                IdentityResult chkUser = await UserManager.CreateAsync(User, userPWD);
                //Add default User to Role Admin
                if (chkUser.Succeeded) { var result1 = await UserManager.AddToRoleAsync(User, "Admin"); }
            }
            var roleCheck2 = await RoleManager.RoleExistsAsync("User");
            if (!roleCheck2)
            {
                roleResult = await RoleManager.CreateAsync(new IdentityRole("User"));
            }
            cstoreUser user1 = await UserManager.FindByEmailAsync("user1@mvcbook.com");
            if (user1 == null)
            {
                var User = new cstoreUser();
                User.Email = "user1@mvcbook.com";
                User.UserName = "user1@mvcbook.com";
                string userPWD = "User1234";
                IdentityResult chkUser = await UserManager.CreateAsync(User, userPWD);
                if (chkUser.Succeeded)
                {
                    await UserManager.AddToRoleAsync(User, "User");
                }
            }
        }

        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new cstoreContext(
                serviceProvider.GetRequiredService<DbContextOptions<cstoreContext>>()))
            {
                CreateUserRoles(serviceProvider).Wait();

                if (context.Product.Any() || context.Category.Any() || context.Brand.Any() || context.Reviews.Any() || context.Users.Any())
                {
                    return;
                }


                context.Brand.AddRange(
                    new Brand
                    {
                        Name="Nike",
                        Description="Nike 1964",
                        Products=new List<Product> { 
                        new Product{
                        Name="T-shirt", Description="100% cotton t-shirt", Price=70, Color="White", Size=18,
                        ImageURL="https://static.nike.com/a/images/t_PDP_1728_v1/f_auto,q_auto:eco/40d92c61-a4fd-4098-8049-e5d2c105e91a/solo-swoosh-t-shirt-2CFK7L.png",
                        IsAvailable=true,
                        Reviews=new List<Reviews>
                        {
                            new Reviews{ProductId=1, AppUser="John Doe",Comment="Nice shirt to wear, but nothing special", Rating=3 }
                        }
                        },

                        new Product
                        {
                           Name="Hoodie", Description="100% cotton hoodie", Price=100, Color="Green", Size=14,
                        ImageURL="https://static.nike.com/a/images/c_limit,w_592,f_auto/t_product_v1/4b135943-9d49-4e25-b22b-85d23f611c53/giannis-standard-issue-mens-graphic-basketball-crew-Q5dm6w.png",
                        IsAvailable=true,
                        Reviews=new List<Reviews>
                        {
                            new Reviews{ProductId=2, AppUser="Jane Doe",Comment="Good hoodie", Rating=4 }
                        }
                        },

                        new Product{
                        Name="Hoodie", Description="100% cotton hoodie", Price=99, Color="Blue", Size=16,
                        ImageURL = "https://static.nike.com/a/images/t_PDP_1728_v1/f_auto,q_auto:eco/njqltik2pun0qj9nhoor/sportswear-club-fleece-pullover-hoodie-Gw4Nwq.png",
                        IsAvailable=true,
                        Reviews=new List<Reviews>
                        {
                            new Reviews{ProductId=3, AppUser="Sam Doe",Comment="Dissapointed! Want my money back", Rating=0}
                        }
                        },

                        new Product
                        {
                            
                        Name="Shoes", Description="Material: leather ", Price=179, Color="Black", Size=42,
                    ImageURL = "https://static.nike.com/a/images/t_PDP_1728_v1/f_auto,q_auto:eco/90d704ea-7732-4a95-a1ee-de579353e339/air-jordan-1-low-shoes-6Q1tFM.png",
                        IsAvailable=true,
                        Reviews=new List<Reviews>
                        {
                            new Reviews{ProductId=4, AppUser="Sam Doe",Comment="They are ok.", Rating=3}
                        }
                        },

                        new Product
                        {
                            Name="Shoes", Description="Material: leather ", Price=139, Color="White and Beige", Size=38,
                    ImageURL = "https://static.nike.com/a/images/t_PDP_1728_v1/f_auto,q_auto:eco/2e0ae78e-1939-468f-8019-8793feaa76e8/air-jordan-1-low-se-womens-shoes-HcLzB9.png",
                        IsAvailable=true,
                        Reviews=new List<Reviews>
                        {
                            new Reviews{ProductId=5, AppUser="Michaela A",Comment="Nice look, uncomfortable to wear.", Rating=3}
                        }
                        },

                        new Product {
                        Name="Shoes", Description="Material: leather ", Price=105, Color="Blue and Beige", Size=40,
                    ImageURL = "https://static.nike.com/a/images/t_PDP_1728_v1/f_auto,q_auto:eco/b4c65d3d-bdf9-42a6-adc7-961e7be19694/dunk-low-womens-shoes-kPGHX0.png",
                        IsAvailable=true,
                        Reviews=new List<Reviews>
                        {
                            new Reviews{ProductId=6, AppUser="Michaela A",Comment="Same like the others", Rating=3}
                        }
                        }, 
                        new Product {
                         Name="Shoes", Description="Material: leather ", Price=155, Color="Grey and Blue", Size=42,
                    ImageURL = "https://static.nike.com/a/images/t_PDP_864_v1/f_auto,b_rgb:f5f5f5/c72f8b35-0088-479d-972e-710f1a9fbc59/dunk-low-shoes-GksKbr.png",
                        IsAvailable=true,
                        Reviews=new List<Reviews>
                        {
                            new Reviews{ProductId=7, AppUser="Michael Smith",Comment="Nice!", Rating=5}
                        }
                        },
                        new Product {
                         Name="Shoes", Description="Material: leather ", Price=195, Color="White and Pink", Size=36,
                    ImageURL = "https://static.nike.com/a/images/t_PDP_1728_v1/f_auto,q_auto:eco/a9d437e6-a6aa-49cf-9536-9a1f3c57a227/dunk-high-up-womens-shoes-3DnDtC.png",
                        IsAvailable=true,
                        Reviews=new List<Reviews>
                        {
                            new Reviews{ProductId=7, AppUser="Michael Smith",Comment="Okay..", Rating=4}
                        }
                        },
                        }





                    },

                    new Brand { 
                    Name="Adidas", Description="Adidas 1949",
                    Products=new List<Product> {
                    
                    new Product{

                        Name="T-Shirt", Description="100% cotton ", Price=35, Color="Black", Size=14,
                    ImageURL = "https://assets.ajio.com/medias/sys_master/root/20220429/SdQF/626af183aeb26921af498f2b/-473Wx593H-469164938-black-MODEL.jpg",
                        IsAvailable=true,
                        Reviews=new List<Reviews>
                        {
                            new Reviews{ProductId=8, AppUser="Michael Smith",Comment="Nice material.", Rating=4}
                        }
                    },

                    new Product{
                     Name="Hoodie", Description="100% cotton ", Price=79, Color="Beige", Size=14,
                    ImageURL = "https://assets.adidas.com/images/w_940,f_auto,q_auto/d5a5957cf01d46a9a321af2a010259e6_9366/IC5592_21_model.jpg",
                        IsAvailable=true,
                        Reviews=new List<Reviews>
                        {
                            new Reviews{ProductId=9, AppUser="Lauler Castillo",Comment="Okay", Rating=3}
                        }

                    },

                    new Product{
                     Name="Hoodie", Description="100% cotton ", Price=99, Color="Beige", Size=10,
                    ImageURL = "https://assets.adidas.com/images/h_840,f_auto,q_auto,fl_lossy,c_fill,g_auto/25fcdeb6d9d8444abba091f1eedc08d1_9366/Trefoil_Hoodie_Beige_IK6471_21_model.jpg",
                        IsAvailable=true,
                        Reviews=new List<Reviews>
                        {
                            new Reviews{ProductId=10, AppUser="Lauler Castillo",Comment="Not a fan.", Rating=2}
                        }
                    },

                    new Product{
                     Name="Hoodie", Description="100% cotton ", Price=89, Color="Black", Size=12,
                    ImageURL = "https://img01.ztat.net/article/spp-media-p1/09f15b3996e447a8bbaccde0d7ec9665/a9531fe1f5094ceaa72ff7586c03e051.jpg?imwidth=1800",
                        IsAvailable=true,
                        Reviews=new List<Reviews>
                        {
                            new Reviews{ProductId=11, AppUser="Anne Gibbins",Comment="Very nice and warm!", Rating=5}
                        }
                    },

                    new Product{
                     Name="Shoes", Description="Material: leather ", Price=139, Color="Beige", Size=40,
                    ImageURL = "https://assets.adidas.com/images/h_840,f_auto,q_auto,fl_lossy,c_fill,g_auto/4e0564c27f754915b743afa200c7db08_9366/Samba_Originals_Shoes_White_ID2047_01_standard.jpg",
                        IsAvailable=true,
                        Reviews=new List<Reviews>
                        {
                            new Reviews{ProductId=12, AppUser="Anne Gibbins",Comment="Okay", Rating=3}
                        }
                    },

                    new Product{
                     Name="Shoes", Description="Material: leather ", Price=119, Color="White and Green", Size=39,
                    ImageURL = "https://cms-cdn.thesolesupplier.co.uk/2022/11/sporty-rich-x-adidas-samba-og-white-green-hq6075_w1024_h1024_pad_.jpg.webp",
                        IsAvailable=true,
                        Reviews=new List<Reviews>
                        {
                            new Reviews{ProductId=13, AppUser="Anonymous",Comment="Awful", Rating=1}
                        }
                    },

                    new Product
                    {
                         Name="Shoes", Description="Material: leather ", Price=99, Color="White and Green", Size=37,
                    ImageURL = "https://assets.adidas.com/images/h_840,f_auto,q_auto,fl_lossy,c_fill,g_auto/49300336593844a0876caea200f41262_9366/Stan_Smith_Bonega_Shoes_White_GY9310_010_hover_standard.jpg",
                        IsAvailable=true,
                        Reviews=new List<Reviews>
                        {
                            new Reviews{ProductId=14, AppUser="Anonymous",Comment="I like them", Rating=4}
                        }
                    },
                    }
                    
                    },

                    new Brand
                    {
                        Name="VANS", Description="VANS 1966",
                        Products = new List<Product>
                        { 
                        new Product{

                             Name="T-Shirt", Description="Material: Cotton ", Price=39, Color="Black", Size=16,
                    ImageURL = "https://imagescdn.simons.ca/images/8728-49992-1-A1_2/served-fresh-daily-t-shirt.jpg?__=2",
                        IsAvailable=true,
                        Reviews=new List<Reviews>
                        {
                            new Reviews{ProductId=15, AppUser="Laurel Castillo",Comment="Nice material", Rating=4}
                        }
                        },

                        new Product{
                          Name="T-Shirt", Description="Material: Cotton ", Price=49, Color="Black", Size=18,
                    ImageURL = "https://media.titus.de/media/image/c5/55/2b/vans-t-shirts-classic-navy-white-vorderansicht-0361788.jpg",
                        IsAvailable=true,
                        Reviews=new List<Reviews>
                        {
                            new Reviews{ProductId=16, AppUser="Laurel Castillo",Comment="The other was better", Rating=3}
                        }
                        },

                        new Product{
                         Name="Jeans", Description="Material: Denim ", Price=89, Color="Denim", Size=28,
                    ImageURL = "https://img01.ztat.net/article/spp-media-p1/9f4d51a0038a44c3a083e99ad3150139/51aa63855a41415e86d3c731c655d4df.jpg?imwidth=1800",
                        IsAvailable=true,
                        Reviews=new List<Reviews>
                        {
                            new Reviews{ProductId=17, AppUser="Paul Rudd",Comment="I liked them", Rating=5}
                        }
                        },

                        new Product{
                          Name="Shoes", Description="Material: Synthetics and leather ", Price=99, Color="Black", Size=39,
                    ImageURL = "https://d2ob0iztsaxy5v.cloudfront.net/product/197386/1973867350m8_zm.jpg",
                        IsAvailable=true,
                        Reviews=new List<Reviews>
                        {
                            new Reviews{ProductId=18, AppUser="Anne Gibbins",Comment="Comfortable to wear", Rating=4}
                        }
                        },

                        new Product{
                         Name="Shoes", Description="Material: Synthetics and leather ", Price=79, Color="Black", Size=39,
                    ImageURL = "https://www.pacsun.com/dw/image/v2/AAJE_PRD/on/demandware.static/-/Sites-pacsun_storefront_catalog/default/dwa6d5ea84/product_images/0542037910056NEW_00_091.jpg?sw=1000",
                        IsAvailable=true,
                        Reviews=new List<Reviews>
                        {
                            new Reviews{ProductId=19, AppUser="Sam Joe",Comment="Uncomfortable to wear", Rating=1}
                        }
                        },

                        new Product{
                          Name="Shoes", Description="Material: Synthetics and leather ", Price=79, Color="Black", Size=45,
                    ImageURL = "https://i.ebayimg.com/images/g/EXsAAOSwp5JjMwwK/s-l500.jpg",
                        IsAvailable=true,
                        Reviews=new List<Reviews>
                        {
                            new Reviews{ProductId=20, AppUser="Sam Joe",Comment="No comment", Rating=1}
                        }
                        },

                        new Product{
                              Name="Shoes", Description="Material: Synthetics and leather ", Price=79, Color="Grey", Size=45,
                    ImageURL = "https://cdn11.bigcommerce.com/s-f8ihez6tx6/images/stencil/1280x1280/products/12419/33210/Old-Skool-Purple-Heather__S_2__85253.1665352902.png?c=1",
                        IsAvailable=true,
                        Reviews=new List<Reviews>
                        {
                            new Reviews{ProductId=21, AppUser="Sam Joe",Comment="Good", Rating=3}
                        }
                        },
                        }
                        }

                    );
                context.SaveChanges();

                context.Category.AddRange(
                    new Category { Name="T-Shirt"},
                    new Category { Name = "Hoodie" },
                    new Category { Name = "Jeans" },
                    new Category { Name = "Shoes" }

                    );

                context.SaveChanges();
            }
        }
    }


}