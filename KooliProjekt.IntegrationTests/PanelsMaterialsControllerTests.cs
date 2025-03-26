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
    public class PanelMaterialsControllerTests : TestBase
    {
        private readonly HttpClient _client;
        private readonly ApplicationDbContext _context;

        public PanelMaterialsControllerTests()
        {
            var options = new WebApplicationFactoryClientOptions { AllowAutoRedirect = false };
            _client = Factory.CreateClient(options);
            _context = (ApplicationDbContext)Factory.Services.GetService(typeof(ApplicationDbContext));
        }

        [Fact]
        public async Task Index_should_return_correct_response()
        {
            // Act
            using var response = await _client.GetAsync("/PanelMaterials");

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Details_should_return_notfound_when_panel_material_not_found()
        {
            // Act
            using var response = await _client.GetAsync("/PanelMaterials/Details/100");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Details_should_return_notfound_when_id_is_missing()
        {
            // Act
            using var response = await _client.GetAsync("/PanelMaterials/Details/");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Details_should_return_ok_when_panel_material_found()
        {
            // Arrange
            var panel = new Panel
            {
                Title = "Test Panel"  // Provide necessary properties for Panel
            };
            var material = new Material
            {
                Title = "Test Material",  // Provide necessary properties for Material
                UnitPrice = 10.0m       // Provide a unit price or any other needed properties
            };
            var panelMaterial = new PanelMaterial { Title = "Material 1", Panel = panel, Material =  material };  // Changed 'list' to 'panelMaterial'
            _context.PanelMaterial.Add(panelMaterial);
            _context.SaveChanges();

            // Act
            using var response = await _client.GetAsync("/PanelMaterials/Details/" + panelMaterial.Id);

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Create_should_save_new_panel_material()
        {
            // Arrange
            // Create and save a new Panel object
            var panel = new Panel
            {
                Title = "Test Panel"  // Provide necessary properties for Panel
            };
            _context.Panel.Add(panel);

            // Create and save a new Material object
            var material = new Material
            {
                Title = "Test Material",  // Provide necessary properties for Material
                UnitPrice = 10.0m       // Provide a unit price or any other needed properties
            };
            _context.Material.Add(material);
            await _context.SaveChangesAsync();

            // Prepare form values for the PanelMaterial to create
            var formValues = new Dictionary<string, string>
            {
            { "Id", "0" },
            { "Title", "Test Panel Material" },
            { "PanelId", panel.Id.ToString() },  // Link to the created Panel
            { "MaterialId", material.Id.ToString() }  // Link to the created Material
            };

            using var content = new FormUrlEncodedContent(formValues);

            // Act
            using var response = await _client.PostAsync("/PanelMaterials/Create", content);

            // Assert
            Assert.True(
                response.StatusCode == HttpStatusCode.Redirect ||
                response.StatusCode == HttpStatusCode.MovedPermanently);

            var panelMaterial = _context.PanelMaterial.FirstOrDefault();
            Assert.NotNull(panelMaterial);
            Assert.NotEqual(0, panelMaterial.Id);
            Assert.Equal("Test Panel Material", panelMaterial.Title);
            Assert.Equal(panel.Id, panelMaterial.PanelId);  // Ensure the correct Panel is linked
            Assert.Equal(material.Id, panelMaterial.MaterialId);
        }
 

        [Fact]
        public async Task Create_should_not_save_invalid_new_panel_material()
        {
            // Arrange
            var formValues = new Dictionary<string, string>();
            formValues.Add("UnitPrice", "");  

            using var content = new FormUrlEncodedContent(formValues);

            // Act
            using var response = await _client.PostAsync("/PanelMaterials/Create", content);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.False(_context.PanelMaterial.Any());
        }
    }
}
