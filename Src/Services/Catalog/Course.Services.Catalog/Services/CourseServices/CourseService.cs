using AutoMapper;
using Course.Services.Catalog.Dtos;
using Course.Services.Catalog.Models;
using Course.Services.Catalog.Settings;
using Mass = MassTransit;
using MicroService.Shareds.Dtos;
using MongoDB.Driver;
using MicroService.Shareds.Messages;

namespace Course.Services.Catalog.Services.CourseServices
{
    public class CourseService : ICourseService
    {
        private readonly IMongoCollection<Coursee> _courseDatabese;
        private readonly IMongoCollection<Category> _categoryDatabese;

        private readonly IMapper _mapper;
        private readonly Mass.IPublishEndpoint _publishEndPoint;
        public CourseService(IMapper mapper, IDatabaseSettings databaseSettings, Mass.IPublishEndpoint publishEndPoint)
        {
            var connect = new MongoClient(databaseSettings.ConnectionString);
            var database = connect.GetDatabase(databaseSettings.DatabaseName);
            _courseDatabese = database.GetCollection<Coursee>(databaseSettings.CourseCollectionName);
            _categoryDatabese = database.GetCollection<Category>(databaseSettings.CategoryCollectionName);
            _mapper = mapper;
            _publishEndPoint = publishEndPoint;
        }

        public async Task<Response<List<CourseListDto>>> GetAllAsync()
        {
            var data = await _courseDatabese.Find(course => true).ToListAsync();
            if (data.Any())
                foreach (var course in data)
                {
                    course.Category = await _categoryDatabese.Find(x => x.Id == course.CategoryId).FirstAsync();
                }
            else
                data = new List<Coursee>();


            return Response<List<CourseListDto>>.Success(_mapper.Map<List<CourseListDto>>(data), 200);
        }
        public async Task<Response<CourseListDto>> GetByIdAsync(string id)
        {
            var data = await _courseDatabese.Find<Coursee>(x => x.Id == id).FirstOrDefaultAsync();
            if (data != null)
            {
                data.Category = await _categoryDatabese.Find<Category>(x => x.Id == data.CategoryId).FirstAsync();
                return Response<CourseListDto>.Success(_mapper.Map<CourseListDto>(data), 200);
            }
            return Response<CourseListDto>.Fail("Course Not Found", 404);
        }

        public async Task<Response<List<CourseListDto>>> GetByCourseUserIdAsync(string userId)
        {
            var data = await _courseDatabese.Find<Coursee>(x => x.UserId == userId).ToListAsync();
            if (data.Any())
            {
                foreach (var userCourse in data)
                {
                    userCourse.Category = await _categoryDatabese.Find<Category>(x => x.Id == userCourse.CategoryId).FirstAsync();
                }


            }
            else
            {
                data = new List<Coursee>();
            }
            return Response<List<CourseListDto>>.Success(_mapper.Map<List<CourseListDto>>(data), 200);
        }

        public async Task<Response<CourseCreateDto>> CreateAsync(CourseCreateDto p)
        {
            var courseMap = _mapper.Map<Coursee>(p);

            courseMap.CreatedTime = DateTime.Now;
            await _courseDatabese.InsertOneAsync(courseMap);

            return Response<CourseCreateDto>.Success(_mapper.Map<CourseCreateDto>(courseMap), 200);
        }
        public async Task<Response<NoContent>> UpdateAsync(CourseUpdateDto p)
        {
            var courseMap = _mapper.Map<Coursee>(p);
            courseMap.CreatedTime = courseMap.CreatedTime;
            var result = await _courseDatabese.FindOneAndReplaceAsync(x => x.Id == p.Id, courseMap);
            if (result == null)
            {
                return Response<NoContent>.Fail("Course No Content", 404);
            }
            await _publishEndPoint.Publish<CourseNameChangedEvent>(new CourseNameChangedEvent { ProductId = courseMap.Id, UpdateName = p.Name });

            return Response<NoContent>.Success(204);
        }
        public async Task<Response<NoContent>> DeleteAsync(string id)
        {
            var result = await _courseDatabese.DeleteOneAsync(x => x.Id == id);
            if (result.DeletedCount > 0)
            {
                return Response<NoContent>.Success(204);

            }
            return Response<NoContent>.Fail("Course Not Found", 404);
        }
    }
}
