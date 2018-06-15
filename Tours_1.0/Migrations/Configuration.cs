namespace Tours_1._0.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Tours_1._0.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<Tours_1._0.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Tours_1._0.Models.ApplicationDbContext context)
        {
            // Заполнение разновидностей туров по их типу
            context.TypeTours.AddOrUpdate(x => x.TypeTourID,
                new TypeTour() { TypeTourID = 1, TypeTourName = "Отдых" },
                new TypeTour() { TypeTourID = 2, TypeTourName = "Шоппинг" },
                new TypeTour() { TypeTourID = 3, TypeTourName = "Экстрим" }
                );

            //Заполнение отелей
            context.Hostels.AddOrUpdate(x => x.HostelID,
                new Hostel()
                {
                    HostelID = 1,
                    HostelName = "Palace 5th",
                    HostelMark = 5,
                    Website = "https://kharkiv-palace.phnr.com/"
                },
                new Hostel()
                {
                    HostelID = 2,
                    HostelName = "Super Hotel",
                    HostelMark = 4,
                    Website = "http://www.ukraine-hotel.kiev.ua/"
                });

            // Заполнение туров
            context.Tours.AddOrUpdate(x => x.TourID,
                new Tour()
                {
                    TourID = 1,
                    TourName = "Отдых в Крыму",
                    TourSights = "Горы, пляж, красивые девушки и парни",
                    HostelID = 1,
                    TypeTourID = 1,
                    DateStart = "25 января 2018",
                    DateEnd = "25 февраля 2018",
                    StatusHot = false,
                    Price = 5000
                },
                new Tour()
                {
                    TourID = 2,
                    TourName = "Тур по Амстердаму",
                    TourSights = "Поля тюльпанов, улица Красных фонарей",
                    HostelID = 2,
                    TypeTourID = 2,
                    DateStart = "10 марта 2018",
                    DateEnd = "17 марта 2018",
                    StatusHot = true,
                    Price = 13000
                });

            //Заполнение пользователей
            var userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(context));
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            // создаем две роли
            var role1 = new IdentityRole { Name = "admin" };
            var role2 = new IdentityRole { Name = "manager" };
            var role3 = new IdentityRole { Name = "user" };

            //добавляем роли в бд
            roleManager.Create(role1);
            roleManager.Create(role2);
            roleManager.Create(role3);

            var admin = new ApplicationUser
            {
                Id = "1",
                FirstName = "Администратор",
                LastName = "Администраторский",
                Email = "admin@gmail.com",
                UserName = "admin@gmail.com"
            };

            string password1 = "Qwerty1!";
            var result1 = userManager.Create(admin, password1);

            var manager = new ApplicationUser
            {
                Id = "2",
                FirstName = "Post",
                LastName = "Malone",
                Email = "manager@gmail.com",
                UserName = "manager@gmail.com"
            };

            string password2 = "Qwerty1!";
            var result2 = userManager.Create(manager, password2);

            // создаем пользователей
            var user = new ApplicationUser
            {
                Id = "3",
                FirstName = "Влад",
                LastName = "Сергиенко",
                Email = "vlad@gmail.com",
                UserName = "vlad@gmail.com"
            };

            string password3 = "Qwerty1!";
            var result3 = userManager.Create(user, password3);

            // если создание пользователя прошло успешно
            if (result1.Succeeded)
            {
                userManager.AddToRole(admin.Id, role1.Name);
            }
            if (result2.Succeeded)
            {
                // добавляем для пользователя роль
                userManager.AddToRole(manager.Id, role2.Name);
            }
            if (result3.Succeeded)
            {
                // добавляем для пользователя роль
                userManager.AddToRole(user.Id, role3.Name);
            }

            //Заполнение статусов
            context.StatusOrders.AddOrUpdate(x => x.StatusOrderID,
                new StatusOrder() { StatusOrderID = 1, StatusOrderName = "Заказан" },
                new StatusOrder() { StatusOrderID = 2, StatusOrderName = "Оплачен" },
                new StatusOrder() { StatusOrderID = 3, StatusOrderName = "Отменен" }
                );

            //Заполнение отзывов отелей          
            context.ResponseHostels.AddOrUpdate(x => x.ResponseID,
                new ResponseHostel()
                {
                    ResponseID = 1,
                    ResponseName = "Отличный отель",
                    Mark = 5,
                    DateTime = DateTime.Now,
                    HostelID = 1,
                    UserId = "3"
                },
                new ResponseHostel()
                {
                    ResponseID = 2,
                    ResponseName = "Плохой отель",
                    Mark = 2,
                    DateTime = DateTime.Now,
                    HostelID = 2,
                    UserId = "3"
                }
                );

            //Заполнение отзывов туров
            context.ResponseTours.AddOrUpdate(x => x.ResponseID,
                new ResponseTour()
                {
                    ResponseID = 1,
                    ResponseName = "Отличный тур",
                    Mark = 4,
                    DateTime = DateTime.Now,
                    TourID = 1,
                    UserId = "3"
                },
                new ResponseTour()
                {
                    ResponseID = 2,
                    ResponseName = "Хороший тур",
                    Mark = 3,
                    DateTime = DateTime.Now,
                    TourID = 2,
                    UserId = "3"
                }
                );

            //Заполнение заказов
            context.Orders.AddOrUpdate(x => x.OrderID,
                new Order()
                {
                    OrderID = 1,
                    TourID = 1,
                    UserId = "3",
                    DateOrder = DateTime.Now.ToShortDateString(),
                    Price = 1200,
                    StatusOrderID = 1
                }
                );

            context.Tickets.AddOrUpdate(
                new Ticket() { OrderId = 1}
                );
        }
    }
}
