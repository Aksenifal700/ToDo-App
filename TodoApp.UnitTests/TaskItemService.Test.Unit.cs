using Moq;
using TodoApp.BusinessLogic.Exceptions;
using TodoApp.BusinessLogic.Services;
using TodoApp.Interfaces;
using TodoApp.Interfaces.DTOs.Category;
using TodoApp.Interfaces.DTOs.TaskItem;
using TodoApp.Interfaces.IServices;
using TodoApp.UnitTests.Helper;

namespace TodoApp.UnitTests;

public class TaskItemServiceTestUnit
{
    private readonly Mock<ITaskItemRepository> _taskItemRepositoryMock;
    private readonly Mock<ICachedQueryService> _cachedQueryServiceMock;
    private readonly Mock<ICategoryRepository> _categoryRepositoryMock;
    private readonly TaskItemService _taskItemService;

    public TaskItemServiceTestUnit()
    {
        _taskItemRepositoryMock = new Mock<ITaskItemRepository>();
        _cachedQueryServiceMock = new Mock<ICachedQueryService>();
        _categoryRepositoryMock = new Mock<ICategoryRepository>();
        _taskItemService = new TaskItemService(_taskItemRepositoryMock.Object,
            _cachedQueryServiceMock.Object,
            _categoryRepositoryMock.Object);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnTaskItem_whenTaskItemExists()
    {
        //Arrange
        var taskItemid = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var expectedDto = new TaskItemDto
        {
            Id = taskItemid, Title = "test"
        };
        
        _taskItemRepositoryMock
            .Setup(x => x.GetByIdAsync(taskItemid,userId))
            .ReturnsAsync(expectedDto);
        CacheMockHelper.SetupPassThrough<TaskItemDto>(_cachedQueryServiceMock);
        
        //Act
        var result = await _taskItemService.GetByIdAsync(taskItemid,userId);
        
        //Assert
        Assert.Equal(taskItemid, result.Id);
        _taskItemRepositoryMock.Verify(x => x.GetByIdAsync(taskItemid,userId), Times.Once);

    }

    [Fact]
    public async Task GetByIdAsync_ShouldThrowNotFoundException_WhenTaskItemDoesNotExist()
    {
        //Arrange
        var taskItemid = Guid.NewGuid();
        var userId = Guid.NewGuid();

        _taskItemRepositoryMock
            .Setup(x => x.GetByIdAsync(taskItemid, userId))
            .ReturnsAsync((TaskItemDto?)null);
        CacheMockHelper.SetupPassThrough<TaskItemDto>(_cachedQueryServiceMock);

        //Act
        var act = async () => await _taskItemService.GetByIdAsync(taskItemid,userId);
        
        //Assert
        await Assert.ThrowsAsync<NotFoundException>(act);
        
        _cachedQueryServiceMock.Verify(
            x => x.InvalidateAsync(It.IsAny<string[]>()), 
            Times.Never);
    }

    [Fact]
    public async Task GetByUserIdAsync_ShouldReturnTaskItem_WhenTaskItemExists()
    {
        //Arrange
        var taskItemId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var expectedList = new List<TaskItemDto>
        {
            new() { Id = taskItemId, Title = "test" }
        };
        
        _taskItemRepositoryMock
            .Setup(x => x.GetByUserIdAsync(userId))
            .ReturnsAsync(expectedList);
        CacheMockHelper.SetupPassThrough<List<TaskItemDto>>(_cachedQueryServiceMock);
        
        //Act
        var result = await _taskItemService.GetByUserIdAsync(userId);
        
        //Assert
        Assert.Equal(expectedList, result);
        _taskItemRepositoryMock.Verify(x => x.GetByUserIdAsync(userId), Times.Once);

    }

    [Fact]
    public async Task GetByUserIdAsync_ShouldReturnNull_WhenTaskItemDoesNotExist()
    {
        //Arrange
        var userId = Guid.NewGuid();
        
        _taskItemRepositoryMock
            .Setup(x => x.GetByUserIdAsync(userId))
            .ReturnsAsync((List<TaskItemDto>?)null);
        CacheMockHelper.SetupPassThrough<List<TaskItemDto>>(_cachedQueryServiceMock);
        
        //Act
        var result = await _taskItemService.GetByUserIdAsync(userId);
        
        //Arrange
        Assert.Empty(result);
        _taskItemRepositoryMock.Verify(x => x.GetByUserIdAsync(userId), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnTaskItem_WhenCategoryExists()
    {
        //Arrange
        var userId = Guid.NewGuid();
        var categoryId = Guid.NewGuid();
        var existingCategory = new CategoryDto { Id = categoryId, Name = "Work" };
        var dto = new CreateTaskItemDto { Title = "test", CategoryId = categoryId };
        var createdDto = new TaskItemDto { Id = Guid.NewGuid(), Title = "test" };
        
        _categoryRepositoryMock
            .Setup(x => x.GetByIdAsync(categoryId, userId))
            .ReturnsAsync(existingCategory);
        _taskItemRepositoryMock
            .Setup(x => x.AddAsync(dto, userId))
            .ReturnsAsync(createdDto);
        CacheMockHelper.SetupPassThrough<TaskItemDto>(_cachedQueryServiceMock);
        
        //Act
        var result = await _taskItemService.CreateAsync(dto, userId);
        
        //Arrange
        Assert.Equal(createdDto, result);
        _taskItemRepositoryMock.Verify(x => x.AddAsync(dto, userId), Times.Once);
        _categoryRepositoryMock.Verify(x => x.GetByIdAsync(categoryId, userId), Times.Once);
        _cachedQueryServiceMock.Verify(x => x.InvalidateAsync(It.IsAny<string[]>()), Times.Once);
    }

    
    [Fact]
    public async Task CreateAsync_ShouldReturnTaskItem_WhenCategoryIdIsNull()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var dto = new CreateTaskItemDto { Title = "test", CategoryId = null };
        var createdDto = new TaskItemDto { Id = Guid.NewGuid(), Title = "test" };

        _taskItemRepositoryMock
            .Setup(x => x.AddAsync(dto, userId))
            .ReturnsAsync(createdDto);
        CacheMockHelper.SetupPassThrough<TaskItemDto>(_cachedQueryServiceMock);

        // Act
        var result = await _taskItemService.CreateAsync(dto, userId);

        // Assert
        Assert.Equal(createdDto, result);
        _categoryRepositoryMock.Verify(
            x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()), Times.Never);
        _taskItemRepositoryMock.Verify(x => x.AddAsync(dto, userId), Times.Once);
        _cachedQueryServiceMock.Verify(x => x.InvalidateAsync(It.IsAny<string[]>()), Times.Once);
    }
    
    [Fact]
    public async Task CreateAsync_ShouldThrowNotFoundException_WhenCategoryDoesNotExist()
    {
        //Assert
        var userId = Guid.NewGuid();
        var categoryId = Guid.NewGuid();
        var dto = new CreateTaskItemDto { Title = "test", CategoryId = categoryId };
        
        _categoryRepositoryMock
            .Setup(x => x.GetByIdAsync(categoryId, userId))
            .ReturnsAsync((CategoryDto?)null);
        
        //Act
        var exception = await Record.ExceptionAsync(() => _taskItemService.CreateAsync(dto, userId));
        
        //Assert
        Assert.IsType<NotFoundException>(exception);
        _categoryRepositoryMock.Verify(x => x.GetByIdAsync(categoryId, userId), Times.Once);
        _taskItemRepositoryMock.Verify(
            x => x.AddAsync(It.IsAny<CreateTaskItemDto>(), It.IsAny<Guid>()), Times.Never);
        _cachedQueryServiceMock.Verify(x => x.InvalidateAsync(It.IsAny<string[]>()), Times.Never);
    }

    [Fact]
    public async Task GetPagedAsync_ShouldReturnPagedResult_WhenCalledWithParameters()
    {
        //Arrange
        var userId = Guid.NewGuid();
        var categoryId = Guid.NewGuid();
        var pageNumber = 1;
        var pageSize = 10;
        var searchTerm = "test";
        var isCompleted = true;
        var items = new List<TaskItemDto>();
        {
            new TaskItemDto { Id = Guid.NewGuid(), Title = "test", CategoryId = categoryId };
        }
        var totalCount = 1;
        
        _taskItemRepositoryMock
            .Setup(x => x.GetPagedAsync( userId, pageNumber, pageSize, searchTerm, categoryId, isCompleted))
            .ReturnsAsync((items, totalCount));
        
        //Act
        var result = await _taskItemService.GetPagedAsync(userId, pageNumber, pageSize, searchTerm, categoryId, isCompleted);
        
        //Assert
        Assert.Equal(items, result.Items);
        Assert.Equal(totalCount, result.TotalCount);
        _taskItemRepositoryMock.Verify(
            x => x.GetPagedAsync( userId, pageNumber, pageSize, searchTerm, categoryId, isCompleted),Times.Once);
    }

    [Fact]
    public async Task GetPagedAsync_ShouldReturnEmptyReult_WhenNoItemsFound()
    {
        //Arrange
        var userId = Guid.NewGuid();
        var pageNumber = 1;
        var pageSize = 10;
        
        _taskItemRepositoryMock
            .Setup(x => x.GetPagedAsync( userId, pageNumber, pageSize, null, null, null))
            .ReturnsAsync((new List<TaskItemDto>(), 0));
        
        //Act
        var result = await _taskItemService.GetPagedAsync(userId, pageNumber, pageSize);
        
        //Assert
        Assert.Empty(result.Items);
        Assert.Equal(0, result.TotalCount);
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnTaskItem_WhenTaskItemExists()
    {
        //Arrange
        var id = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var categoryId = Guid.NewGuid();
        var existingCategory = new CategoryDto { Id = categoryId, Name = "test" };
        var dto = new UpdateTaskItemDto {Title = "updated", CategoryId = categoryId};
        var updateDto = new TaskItemDto { Id = id, Title = "test" };
        
        _categoryRepositoryMock
            .Setup(x => x.GetByIdAsync(categoryId, userId))
            .ReturnsAsync(existingCategory);
        _taskItemRepositoryMock
            .Setup(x => x.UpdateAsync(id, dto, userId))
            .ReturnsAsync(updateDto);
        CacheMockHelper.SetupPassThrough<TaskItemDto>(_cachedQueryServiceMock);
        
        //Act
        var result = await _taskItemService.UpdateAsync(id, dto, userId);
        
        //Assert
        Assert.Equal(updateDto, result);
        _categoryRepositoryMock.Verify(x => x.GetByIdAsync(categoryId, userId), Times.Once);
        _taskItemRepositoryMock.Verify(x => x.UpdateAsync(id, dto, userId), Times.Once);
        _cachedQueryServiceMock.Verify(x => x.InvalidateAsync($"task:{userId}:{id}", $"tasks:{userId}"), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnTaskItem_WhenCategoryIdIsNull()
    {
        //Arrange
        var id = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var dto = new UpdateTaskItemDto { Title = "updated", CategoryId = null };
        var updateDto = new TaskItemDto { Id = Guid.NewGuid(), Title = "test" };
        
        _taskItemRepositoryMock
            .Setup(x => x.UpdateAsync(id,dto, userId))
            .ReturnsAsync(updateDto);
        CacheMockHelper.SetupPassThrough<TaskItemDto>(_cachedQueryServiceMock);
        
        //Act
        var result = await _taskItemService.UpdateAsync(id, dto, userId);
        
        //Assert 
        Assert.Equal(updateDto, result);
        _categoryRepositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()), Times.Never);
        _taskItemRepositoryMock.Verify(x => x.UpdateAsync(id,dto, userId), Times.Once);
        _cachedQueryServiceMock.Verify(x => x.InvalidateAsync($"task:{userId}:{id}", $"tasks:{userId}"), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ShouldThrowNotFoundException_WhenCategoryDoesNotExist()
    {
        //Arrange
        var id = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var categoryId = Guid.NewGuid();
        var dto = new UpdateTaskItemDto { Title = "updated", CategoryId = categoryId };
        
        _categoryRepositoryMock
            .Setup(x => x.GetByIdAsync(categoryId, userId))
            .ReturnsAsync((CategoryDto?)null);
        
        //Act
        var exception = await Record.ExceptionAsync(() => _taskItemService.UpdateAsync(id, dto, userId));
        
        //Assert
        Assert.IsType<NotFoundException>(exception);
        _categoryRepositoryMock.Verify(x => x.GetByIdAsync(categoryId, userId), Times.Once);
        _taskItemRepositoryMock.Verify(
            x => x.UpdateAsync(It.IsAny<Guid>(), It.IsAny<UpdateTaskItemDto>(), It.IsAny<Guid>()),
            Times.Never);
        _cachedQueryServiceMock.Verify(
            x => x.InvalidateAsync(It.IsAny<string[]>()), Times.Never);
    }
    
    [Fact]
    public async Task DeleteAsync_ShouldDeleteTaskItem_WhenTaskItemExists()
    {
        //Arrange
        var id = Guid.NewGuid();
        var userId = Guid.NewGuid();

        _taskItemRepositoryMock
            .Setup(x => x.DeleteAsync(id, userId))
            .ReturnsAsync(true);

        //Act
        await _taskItemService.DeleteAsync(id, userId);

        //Assert
        _taskItemRepositoryMock.Verify(x => x.DeleteAsync(id, userId), Times.Once);
        _cachedQueryServiceMock.Verify(
            x => x.InvalidateAsync($"task:{userId}:{id}", $"tasks:{userId}"),
            Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ShouldThrowNotFoundException_WhenTaskItemDoesNotExist()
    {
        //Arrange
        var id = Guid.NewGuid();
        var userId = Guid.NewGuid();

        _taskItemRepositoryMock
            .Setup(x => x.DeleteAsync(id, userId))
            .ReturnsAsync(false);

        //Act
        var exception = await Record.ExceptionAsync(() => _taskItemService.DeleteAsync(id, userId));

        //Assert
        Assert.IsType<NotFoundException>(exception);
        _taskItemRepositoryMock.Verify(x => x.DeleteAsync(id, userId), Times.Once);
        _cachedQueryServiceMock.Verify(
            x => x.InvalidateAsync(It.IsAny<string[]>()), Times.Never);
    }
}