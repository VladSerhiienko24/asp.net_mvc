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
            // ���������� �������������� ����� �� �� ����
            context.TypeTours.AddOrUpdate(x => x.TypeTourID,
                new TypeTour() { TypeTourID = 1, TypeTourName = "�����" },
                new TypeTour() { TypeTourID = 2, TypeTourName = "�������" },
                new TypeTour() { TypeTourID = 3, TypeTourName = "�������" }
                );

            //���������� ������
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

            // ���������� �����
            context.Tours.AddOrUpdate(x => x.TourID,
                new Tour()
                {
                    TourID = 1,
                    TourName = "����� � �����",
                    TourSights = "����, ����, �������� ������� � �����",
                    HostelID = 1,
                    TypeTourID = 1,
                    DateStart = "25 ������ 2018",
                    DateEnd = "25 ������� 2018",
                    StatusHot = false,
                    Price = 5000
                },
                new Tour()
                {
                    TourID = 2,
                    TourName = "��� �� ����������",
                    TourSights = "���� ���������, ����� ������� �������",
                    HostelID = 2,
                    TypeTourID = 2,
                    DateStart = "10 ����� 2018",
                    DateEnd = "17 ����� 2018",
                    StatusHot = true,
                    Price = 13000
                });

            //���������� �������������
            var userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(context));
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            // ������� ��� ����
            var role1 = new IdentityRole { Name = "admin" };
            var role2 = new IdentityRole { Name = "manager" };
            var role3 = new IdentityRole { Name = "user" };

            //��������� ���� � ��
            roleManager.Create(role1);
            roleManager.Create(role2);
            roleManager.Create(role3);

            var admin = new ApplicationUser
            {
                Id = "1",
                FirstName = "�������������",
                LastName = "�����������������",
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

            // ������� �������������
            var user = new ApplicationUser
            {
                Id = "3",
                FirstName = "����",
                LastName = "���������",
                Email = "vlad@gmail.com",
                UserName = "vlad@gmail.com"
            };

            string password3 = "Qwerty1!";
            var result3 = userManager.Create(user, password3);

            // ���� �������� ������������ ������ �������
            if (result1.Succeeded)
            {
                userManager.AddToRole(admin.Id, role1.Name);
            }
            if (result2.Succeeded)
            {
                // ��������� ��� ������������ ����
                userManager.AddToRole(manager.Id, role2.Name);
            }
            if (result3.Succeeded)
            {
                // ��������� ��� ������������ ����
                userManager.AddToRole(user.Id, role3.Name);
            }

            //���������� ��������
            context.StatusOrders.AddOrUpdate(x => x.StatusOrderID,
                new StatusOrder() { StatusOrderID = 1, StatusOrderName = "�������" },
                new StatusOrder() { StatusOrderID = 2, StatusOrderName = "�������" },
                new StatusOrder() { StatusOrderID = 3, StatusOrderName = "�������" }
                );

            //���������� ������� ������          
            context.ResponseHostels.AddOrUpdate(x => x.ResponseID,
                new ResponseHostel()
                {
                    ResponseID = 1,
                    ResponseName = "�������� �����",
                    Mark = 5,
                    DateTime = DateTime.Now,
                    HostelID = 1,
                    UserId = "3"
                },
                new ResponseHostel()
                {
                    ResponseID = 2,
                    ResponseName = "������ �����",
                    Mark = 2,
                    DateTime = DateTime.Now,
                    HostelID = 2,
                    UserId = "3"
                }
                );

            //���������� ������� �����
            context.ResponseTours.AddOrUpdate(x => x.ResponseID,
                new ResponseTour()
                {
                    ResponseID = 1,
                    ResponseName = "�������� ���",
                    Mark = 4,
                    DateTime = DateTime.Now,
                    TourID = 1,
                    UserId = "3"
                },
                new ResponseTour()
                {
                    ResponseID = 2,
                    ResponseName = "������� ���",
                    Mark = 3,
                    DateTime = DateTime.Now,
                    TourID = 2,
                    UserId = "3"
                }
                );

            //���������� �������
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
