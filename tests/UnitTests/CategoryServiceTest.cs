using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using Xunit;
using api.Interfaces;
using api.Services;
using api.Models;
using FluentAssertions;
using AutoMapper;
using api.DTOs;

namespace tests.UnitTests
{
    public class CategoryServiceTest
    {
        private readonly Mock<ICategoryRepository> _mockCategoryRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly CategoryService _categoryService;


        public CategoryServiceTest()
        {
            _mockCategoryRepository = new Mock<ICategoryRepository>();
            _mockMapper = new Mock<IMapper>();

            _categoryService = new CategoryService(_mockCategoryRepository.Object, _mockMapper.Object);
        }

        [Fact]
        public void CategoryService_AddCategory_ShouldAddCategoryToRepository()
        {
            //Arrange

            var categoryDto = new CategoryDto
            {
                CategoryName = "Fiction"
            };

            var category = new Category
            {
                CategoryName = "Fiction"
            };

            _mockMapper.Setup(m => m.Map<Category>(categoryDto)).Returns(category);
            //Act

            _categoryService.AddCategory(categoryDto);

            //Assert

            _mockCategoryRepository.Verify(repo => repo.AddCategory(It.Is<Category>(c => c.CategoryName == "Fiction")), Times.Once);
        }

        [Fact]

        public void CategoryService_GetCategories_ShouldReturnsCategories()
        {

            //Arrange
            var categories = new List<Category>{
                new Category {CategoryId = 1, CategoryName = "Fiction"},
                new Category {CategoryId =2, CategoryName ="Action"}

            };

            _mockCategoryRepository.Setup(repo => repo.GetCategories()).Returns(categories);
            //Act

            var result = _categoryService.GetCategories();

            //Assert

            result.Should().HaveCount(2);
            result.Should().Contain(c => c.CategoryName == "Fiction");
            result.Should().Contain(c => c.CategoryName == "Action");

        }



        [Fact]
        public void CategoryService_GetCategoryById_ShouldReturnUser()
        {
            // Arrange

            var categoryId = 1;
            var category = new Category
            {
                CategoryId = categoryId,
                CategoryName = "Romance"
            };

            _mockCategoryRepository.Setup(repo => repo.GetCategoryById(categoryId)).Returns(category);

            // Act

            var result = _categoryService.GetCategoryById(categoryId);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<Category>();
            result.CategoryId.Should().Be(1);
            result.CategoryName.Should().Be("Romance");
        }

        [Fact]
        public void CategoryService_UpdateCategory_ShouldUpdateCategoryDetails()
        {
            // Arrange

            var existingCategory = new Category
            {
                CategoryId = 1,
                CategoryName = "Fiction"
            };

            var updatedCategoryDto = new CategoryDto
            {
                CategoryId = 1,
                CategoryName = "Action"
            };

            var updatedCategory = new Category
            {
                CategoryId = 1,
                CategoryName = "Action"
            };


            _mockCategoryRepository.Setup(repo => repo.GetCategoryById(1)).Returns(existingCategory);
            _mockMapper.Setup(m => m.Map<Category>(updatedCategoryDto)).Returns(updatedCategory);
            _mockCategoryRepository.Setup(repo => repo.UpdateCategory(It.IsAny<Category>())).Verifiable();
            // Act
            _categoryService.UpdateCategory(updatedCategoryDto);
            // Assert
            _mockCategoryRepository.Verify(repo => repo.UpdateCategory(It.Is<Category>(c => c.CategoryId == 1 && c.CategoryName == "Action")), Times.Once);
        }

        [Fact]
        public void CategoryService_DeleteCategory_ShouldDeleteCategoryFromRepo()
        {
            //Arrange
            var categoryId = 1;
            var category = new Category
            {
                CategoryId = categoryId,
                CategoryName = "Fiction"
            };

            _mockCategoryRepository.Setup(repo => repo.GetCategoryById(categoryId)).Returns(category);

            _mockCategoryRepository.Setup(repo => repo.DeleteCategory(categoryId)).Verifiable();

            //Act

            var result = _categoryService.DeleteCategory(categoryId);

            //Asert

            result.Should().BeTrue();
            _mockCategoryRepository.Verify(repo => repo.DeleteCategory(categoryId), Times.Once);
        }

    }

}