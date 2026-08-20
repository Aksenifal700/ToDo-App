using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TodoApp.Interfaces;
using TodoApp.Interfaces.DTOs.Auth;
using TodoApp.Interfaces.Entities;

namespace TodoApp.DataAccess.Database.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public UserRepository(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<UserDto?> CreateUserAsync(RegisterDto dto, byte[] passwordHash, byte[] passwordSalt)
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = dto.Email,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt
        };
        
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
        
        return _mapper.Map<UserDto>(user);
    }

    public async Task<UserDto?> GetByIdAsync(Guid userId)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == userId);
        
        return user is null 
            ? null 
            : _mapper.Map<UserDto>(user);
    }
    
    public async  Task<UserDto?> GetByEmailAsync(string email)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == email);
        
        return user is null 
            ? null
            : _mapper.Map<UserDto>(user);
    }
    

    public async Task<bool> ExistsByEmailAsync(string email)
    {
        return await _context.Users.AnyAsync(u => u.Email == email);
    }
    
}