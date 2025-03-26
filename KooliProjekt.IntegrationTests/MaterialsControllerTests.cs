﻿using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using KooliProjekt.Data;
using KooliProjekt.IntegrationTests.Helpers;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace KooliProjekt.IntegrationTests
{
    [Collection("Sequential")]
    public class MaterialControllerTests : TestBase
    {
        private readonly HttpClient _client;
        private readonly ApplicationDbContext _context;

        public MaterialControllerTests()
        {
            var options = new WebApplicationFactoryClientOptions { AllowAutoRedirect = false };
            _client = Factory.CreateClient(options);
            _context = (ApplicationDbContext)Factory.Services.GetService(typeof(ApplicationDbContext));
        }

        [Fact]
        public async Task Index_should_return_correct_response()
        {
            // Arrange

            // Act
            using var response = await _client.GetAsync("/Materials");

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Details_should_return_notfound_when_material_not_found()
        {
            // Arrange

            // Act
            using var response = await _client.GetAsync("/Materials/Details/100");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Details_should_return_notfound_when_id_is_missing()
        {
            // Arrange

            // Act
            using var response = await _client.GetAsync("/Materials/Details/");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Details_should_return_ok_when_material_found()
        {
            // Arrange
            var material = new Material { Title = "Test Material" }; // Renamed 'list' to 'material'
            _context.Material.Add(material);
            _context.SaveChanges();

            // Act
            using var response = await _client.GetAsync("/Materials/Details/" + material.Id);

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Create_should_save_new_material()
        {
            // Arrange
            var formValues = new Dictionary<string, string>
            {
                { "Id", "0" },
                { "Name", "Name" },
                { "UnitPrice", "0" },
                { "Title", "Test Material" }
            };

            using var content = new FormUrlEncodedContent(formValues);

            // Act
            using var response = await _client.PostAsync("/Materials/Create", content);

            // Assert
            Assert.True(
                response.StatusCode == HttpStatusCode.Redirect ||
                response.StatusCode == HttpStatusCode.MovedPermanently);

            var material = _context.Material.FirstOrDefault(); // Renamed 'list' to 'material'
            Assert.NotNull(material);
            Assert.NotEqual(0, material.Id);
            Assert.Equal("Name", material.Name);
        }

        [Fact]
        public async Task Create_should_not_save_invalid_new_material()
        {
            // Arrange
            var formValues = new Dictionary<string, string>
            {
                { "UnitPrice", "" } // Invalid input
            };

            using var content = new FormUrlEncodedContent(formValues);

            // Act
            using var response = await _client.PostAsync("/Materials/Create", content);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.False(_context.Material.Any()); // Ensure no material was saved
        }
    }
}
