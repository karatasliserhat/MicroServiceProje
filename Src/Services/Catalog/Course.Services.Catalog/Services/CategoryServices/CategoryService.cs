using AutoMapper;
using Course.Services.Catalog.Dtos;
using Course.Services.Catalog.Models;
using Course.Services.Catalog.Settings;
using MicroService.Shareds.Dtos;
using MongoDB.Driver;

namespace Course.Services.Catalog.Services.CategoryServices
{
    public class CategoryService : ICategoryService
    {
        private readonly IMongoCollection<Category> _categoryDatabase;
        private readonly IMapper _mapper;

        public CategoryService(IMapper mapper, IDatabaseSettings databaseSettings)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);
            var database = client.GetDatabase(databaseSettings.DatabaseName);
            _categoryDatabase = database.GetCollection<Category>(databaseSettings.CategoryCollectionName);
            _mapper = mapper;
        }

        public async Task<Response<List<CategoryListDto>>> GetAllAsync()
        {
            var data = await _categoryDatabase.Find(category => true).ToListAsync();

            return Response<List<CategoryListDto>>.Success(_mapper.Map<List<CategoryListDto>>(data), 200);
        }
        public async Task<Response<CategoryListDto>> GetByIdAsync(string id)
        {
            var data = await _categoryDatabase.Find(x => x.Id == id).FirstOrDefaultAsync();
            if (data == null)
                return Response<CategoryListDto>.Fail("Category Not Found", 404);
            return Response<CategoryListDto>.Success(_mapper.Map<CategoryListDto>(data), 200);
        }
        public async Task<Response<CategoryCreateDto>> CreateAsync(CategoryCreateDto p)
        {
            var data = _mapper.Map<Category>(p);
            await _categoryDatabase.InsertOneAsync(data);

            return Response<CategoryCreateDto>.Success(_mapper.Map<CategoryCreateDto>(data), 200);
        }
        public async Task<Response<NoContent>> UpdateAsync(CategoryUpdateDto p)
        {
            var datamap = _mapper.Map<Category>(p);

            var result = await _categoryDatabase.FindOneAndReplaceAsync(x => x.Id == p.Id, datamap);
            if (result == null)
                return Response<NoContent>.Fail("Category No Content", 404);
            return Response<NoContent>.Success(204);


        }

        public async Task<Response<NoContent>> DeleteAsync(string id)
        {
            var data = await _categoryDatabase.DeleteOneAsync(x => x.Id == id);
            if (data.DeletedCount > 0)
                return Response<NoContent>.Success(204);
            return Response<NoContent>.Fail("Category No Content", 404);
        }
    }
}
