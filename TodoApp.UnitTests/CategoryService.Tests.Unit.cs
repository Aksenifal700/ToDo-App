using Moq;
using TodoApp.BusinessLogic.Exceptions;
using TodoApp.BusinessLogic.Services;
using TodoApp.Interfaces;
using TodoApp.Interfaces.DTOs.Category;
using TodoApp.Interfaces.IServices;
using TodoApp.UnitTests.Helper;

namespace TodoApp.UnitTests;

public class CategoryServiceTests
{
    private readonly Mock<ICategoryRepository> _categoryRepositoryMock;
    private readonly Mock<ICachedQueryService> _cachedQueryServiceMock;
    private readonly CategoryService _categoryService;

    public CategoryServiceTests()
    {
        _cachedQueryServiceMock = new Mock<ICachedQueryService>();
        _categoryRepositoryMock = new Mock<ICategoryRepository>();
        _categoryService = new CategoryService(_categoryRepositoryMock.Object, _cachedQueryServiceMock.Object);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnCategory_WhenCategoryExist()
    {
        //Arrange
        var categoryId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var expectedDto = new CategoryDto
        {
            Id = categoryId, Name = "Test"
        };
        
        _categoryRepositoryMock
            .Setup(x => x.GetByIdAsync(categoryId, userId))
            .ReturnsAsync(expectedDto);
        CacheMockHelper.SetupPassThrough<CategoryDto>(_cachedQueryServiceMock);
        
        //Act
        var result = await _categoryService.GetByIdAsync(categoryId, userId);
        
        //Assert
        Assert.Equal(expectedDto, result);
        _categoryRepositoryMock.Verify(x => x.GetByIdAsync(categoryId, userId), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnsNull_WhenCategoryDoesNotExist()
    {
        //Arrange
        var categoryId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        
        _categoryRepositoryMock
            .Setup(x => x.GetByIdAsync(categoryId, userId))
            .ReturnsAsync((CategoryDto?)null);
        
        CacheMockHelper.SetupPassThrough<CategoryDto>(_cachedQueryServiceMock);
        
        //Act
        var result = await _categoryService.GetByIdAsync(categoryId, userId);
        
        //Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetByUserIdAsync_ShouldReturnCategories_WhenCategoriesExists()
    {
        //Arrange
        var categoryId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var expectedList = new List<CategoryDto>
        {
            new() { Id = categoryId, Name = "Test" }
        };
        
        _categoryRepositoryMock
            .Setup(x => x.GetByUserIdAsync(userId))
            .ReturnsAsync(expectedList);
        
        CacheMockHelper.SetupPassThrough<List<CategoryDto>>(_cachedQueryServiceMock);
        
        //Act
        var result = await _categoryService.GetByUserIdAsync(userId);
        
        //Assert
        Assert.Equal(expectedList, result);
        _categoryRepositoryMock.Verify(x => x.GetByUserIdAsync(userId), Times.Once);
    }

    [Fact]
    public async Task GetByUserIdAsync_ShouldReturnEmptyList_WhenCategoriesDoNotExistt()
    {
        //Arrange
        var userId = Guid.NewGuid();
        
        _categoryRepositoryMock
            .Setup(x => x.GetByUserIdAsync(userId))!
            .ReturnsAsync((List<CategoryDto>?)null);
        CacheMockHelper.SetupPassThrough<List<CategoryDto>>(_cachedQueryServiceMock);
        
        //Act
        var result = await _categoryService.GetByUserIdAsync(userId);
        
        //Assert
        Assert.Empty(result);
        _categoryRepositoryMock.Verify(x => x.GetByUserIdAsync(userId), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnCategory_WhenCategoryCreated()
    {
        //Arrange
        var userId = Guid.NewGuid();
        var dto = new CreateCategoryDto{ Name = "Work" };
        var createdDto = new CategoryDto { Id = Guid.NewGuid(), Name = "Test" };
        
        _categoryRepositoryMock
            .Setup(x => x.GetByNameAndUserIdAsync(dto.Name, userId))
            .ReturnsAsync((CategoryDto?)null);
      
        _categoryRepositoryMock
            .Setup(x => x.AddAsync(dto, userId))
            .ReturnsAsync(createdDto);
        
        // Act
        var result = await _categoryService.CreateAsync(dto, userId);

        // Assert
        Assert.Equal(createdDto, result);
        _categoryRepositoryMock.Verify(x => x.AddAsync(dto, userId), Times.Once);
        _cachedQueryServiceMock.Verify(x => x.InvalidateAsync($"category:{userId}"), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_ShouldTHrowAlreadyExistsException_WhenCategoryAlreadyExists()
    {
        //Arrange
        var userId = Guid.NewGuid();
        var dto = new CreateCategoryDto{ Name = "Work" };
        var existing = new CategoryDto { Id = Guid.NewGuid(), Name = "Test" };
        
        _categoryRepositoryMock
            .Setup(x => x.GetByNameAndUserIdAsync(dto.Name, userId))
            .ReturnsAsync(existing);
        
        //Act
        await Assert.ThrowsAsync<AlreadyExistsException>(() => 
            _categoryService.CreateAsync(dto, userId));
        
        //Assert
        _categoryRepositoryMock
            .Verify(x => x.GetByNameAndUserIdAsync(dto.Name, userId), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateCategory_WhenCategoryUpdated()
    {
        // Arrange
        var id = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var dto = new UpdateCategoryDto { Name = "Updated" };
        var updatedDto = new CategoryDto { Id = id, Name = "Updated" };

        _categoryRepositoryMock
            .Setup(x => x.UpdateAsync(id, dto, userId))
            .ReturnsAsync(updatedDto);

        // Act
        var result = await _categoryService.UpdateAsync(id, dto, userId);

        // Assert
        Assert.Equal(updatedDto, result);
        _cachedQueryServiceMock.Verify(
            x => x.InvalidateAsync($"category:{userId}:{id}", $"category:{userId}"), 
            Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ShouldThrowNotFoundException_WhenCategoryDoesNotExist()
    {
        //Arrange
        var id = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var dto = new UpdateCategoryDto { Name = "Updated" };
        
        _categoryRepositoryMock
            .Setup(x => x.UpdateAsync(id, dto, userId))
            .ReturnsAsync((CategoryDto?)null);
        
        //Act
        await Assert.ThrowsAsync<NotFoundException>(
            () => _categoryService.UpdateAsync(id, dto, userId));

        //Assert
        _cachedQueryServiceMock.Verify(
            x => x.InvalidateAsync(It.IsAny<string[]>()), 
            Times.Never);
    }
    
    [Fact]
    public async Task DeleteAsync_WhenCategoryExists_DeletesSuccessfully()
    {
        // Arrange
        var id = Guid.NewGuid();
        var userId = Guid.NewGuid();

        _categoryRepositoryMock
            .Setup(x => x.DeleteAsync(id, userId))
            .ReturnsAsync(true);

        // Act
        await _categoryService.DeleteAsync(id, userId);

        // Assert
        _cachedQueryServiceMock.Verify(
            x => x.InvalidateAsync($"category:{userId}:{id}", $"category:{userId}"), 
            Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_WhenCategoryDoesNotExist_ThrowsNotFoundException()
    {
        // Arrange
        var id = Guid.NewGuid();
        var userId = Guid.NewGuid();

        _categoryRepositoryMock
            .Setup(x => x.DeleteAsync(id, userId))
            .ReturnsAsync(false);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(
            () => _categoryService.DeleteAsync(id, userId));
    }
}