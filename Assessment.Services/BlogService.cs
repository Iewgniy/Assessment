using Assessment.Data;
using Assessment.Services.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using SynicTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using Twilio.TwiML.Messaging;
using Twilio.TwiML.Voice;

namespace Assessment.Services
{
    public class BlogService : BaseService<AssessmentDataContext, Blog, BlogFilters>, IBlogService
    {
        public BlogService(AssessmentDataContext context)
            : base(context, x => x.Blogs)
        {

        }

        public override IQueryable<Blog> All(BlogFilters filters, params Func<IQueryable<Blog>, IIncludableQueryable<Blog, object>>[] includes)
        {
            var res = base.All(filters, includes);
            return res;
        }


        public override ServiceResult<Blog> CreateUpdate(Blog entity)
        {
            var result = ServiceResult<Blog>.Create(entity);

            if (entity.Id == 0)
            {
                Context.Blogs.Add(entity);
            }
            else
            {
                var existingEntity = GetById(entity.Id);
                if (!existingEntity.Success)
                {
                    result.AddError(entity.Id.ToString(), "No such Entity to update");
                    return result;
                }

                existingEntity.Object.Title = entity.Title;
                existingEntity.Object.Body = entity.Body;
                existingEntity.Object.AuthorID = entity.AuthorID;

                entity = existingEntity.Object;
            }

            if (Context.SaveChanges() < 1)
            {
                result.AddError(entity.Id.ToString(), "Saving Entity failed");
                result.Success = false;
            }

            return result;
        }

        public override ServiceResult<Blog> DeleteById(int id)
        {
            return base.DeleteById(id);
        }

        public override IQueryable<Blog> Get(BlogFilters filters, params Func<IQueryable<Blog>, IIncludableQueryable<Blog, object>>[] includes)
        {
            //var res = this.All(filters, includes);
            IQueryable<Blog> res = base.Context.Blogs.Include(x => x.Author).Where(x => x.Status != EntityStatus.Deleted);
            return res;
        }

        public override ServiceResult<Blog> GetById(int id, params Func<IQueryable<Blog>, IIncludableQueryable<Blog, object>>[] includes)
        {
            return base.GetById(id, x => x.Include(b => b.Author));
        }

    }

    public interface IBlogService
    {
        IQueryable<Blog> All(BlogFilters filters, params Func<IQueryable<Blog>, IIncludableQueryable<Blog, object>>[] includes);
        ServiceResult<Blog> CreateUpdate(Blog entity);
        ServiceResult<Blog> DeleteById(int id);
        IQueryable<Blog> Get(BlogFilters filters = null, params Func<IQueryable<Blog>, IIncludableQueryable<Blog, object>>[] includes);
        ServiceResult<Blog> GetById(int id, params Func<IQueryable<Blog>, IIncludableQueryable<Blog, object>>[] includes);
        ServiceResult<IPageOfList<Blog>> GetPage(BlogFilters filters, int? page = null, int? size = null, params Func<IQueryable<Blog>, IIncludableQueryable<Blog, object>>[] includes);
    }
}
