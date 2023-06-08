using AutoMapper;
using Course.Service.Discount.Dtos;
using Course.Service.Discount.Interfaces;
using Course.Service.Discount.Models;
using Dapper;
using MicroService.Shareds.Dtos;
using Npgsql;
using System.Data;

namespace Course.Service.Discount.Repositories
{

    public class Repository<ListDto, CreateDto, UpdateDto, T> : IRepository<ListDto, CreateDto, UpdateDto, T> where ListDto : BaseDto where CreateDto : BaseDto where UpdateDto : BaseDto where T : BaseEntity
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IDbConnection _connection;
        public Repository(IConfiguration configuration, IMapper mapper)
        {
            _configuration = configuration;
            _connection = new NpgsqlConnection(_configuration.GetConnectionString("PostgreSql"));
            _mapper = mapper;
        }

        public async Task<Response<CreateDto>> CreateDiscountAsync(CreateDto entity)
        {
            var map = _mapper.Map<T>(entity);
            var createResult = await _connection.ExecuteAsync("INSERT INTO discount(userid,rate,code) VALUES(@UserId,@Rate, @Code)", map);
            if (createResult > 0)
            {
                return Response<CreateDto>.Success(_mapper.Map<CreateDto>(map), 204);
            }
            return Response<CreateDto>.Fail("Registration failed ", 500);
        }

        public async Task<Response<NoContent>> DeleteDiscountAsync(int id)
        {
            var deleteDiscount = await _connection.ExecuteAsync("Delete * From discount Where id=@Id", id);
            if (deleteDiscount > 0)
            {
                return Response<NoContent>.Success(204);

            }
            return Response<NoContent>.Fail("Registration failed ", 404);
        }

        public async Task<Response<List<ListDto>>> GetAllDiscountAsync()
        {
            var discountAll = await _connection.QueryAsync<T>("select * from discount");
            var discount = discountAll.ToList();

            if (discount.Any())
            {
                return Response<List<ListDto>>.Success(_mapper.Map<List<ListDto>>(discount), 200);
            }
            return Response<List<ListDto>>.Fail("Discount Not Found", 404);
        }

        public async Task<Response<ListDto>> GetCodeUserDiscountAsync(string code, string userId)
        {
            var discounCode = await _connection.QueryAsync<T>("Select * From discount where code=@Code and userid=@UserId", new { Code = code, UserId = userId });

            var discount = discounCode.FirstOrDefault();

            if (discount == null)
            {
                return Response<ListDto>.Fail("Discount Not Found", 404);
            }
            return Response<ListDto>.Success(_mapper.Map<ListDto>(discount), 200);
        }

        public async Task<Response<ListDto>> GetDiscountAsync(int id)
        {
            var discountGet = await _connection.QueryAsync<T>("Select * From discount where id=@Id", new { Id = id });
            var discount = discountGet.FirstOrDefault();

            if (discount == null)
            {
                return Response<ListDto>.Fail("Discount Not Found", 404);
            }
            return Response<ListDto>.Success(_mapper.Map<ListDto>(discount), 200);
        }

        public async Task<Response<NoContent>> UpdateDiscountAsync(UpdateDto entity)
        {
            var map = _mapper.Map<T>(entity);
            var updateResult = await _connection.ExecuteAsync("update discount set userid=@UserId,rate=@Rate,code=@Code where id=@Id", map);
            if (updateResult > 0)
            {
                return Response<NoContent>.Success(204);
            }
            return Response<NoContent>.Fail("Registration failed ", 404);
        }
    }
}
