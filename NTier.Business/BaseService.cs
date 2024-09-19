using AutoMapper;
using NTier.DataAccess.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTier.Business
{
    public class BaseService
    {
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly IMapper _mapper;

        public BaseService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
    }



    //public class BaseService : IBaseService
    //{
    //    public IUnitOfWork _UnitOfWork { get; set; }
    //    public IMapper _Mapper { get; set; }

    //    // Constructor to inject dependencies
    //    public BaseService(IUnitOfWork unitOfWork, IMapper mapper)
    //    {
    //        _UnitOfWork = unitOfWork;
    //        _Mapper = mapper;
    //    }
    //}

    //public class BaseService
    //{
    //    protected readonly IUnitOfWork _baseService._UnitOfWork;
    //    protected readonly IMapper _Mapper;

    //    public BaseService(IUnitOfWork unitOfWork, IMapper mapper)
    //    {
    //        _baseService._UnitOfWork = unitOfWork;
    //        _Mapper = mapper;
    //    }
    //}
}
