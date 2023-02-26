using BetterHealthManagementAPI.BetterHealth2023.Repository.Repositories.ImplementedRepository.UnitRepos;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ErrorModels.UnitErrorModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.PagingModels;
using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.UnitModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Business.Service.Unit
{
    public class UnitService : IUnitService
    {
        private readonly IUnitRepo _unitRepo;

        public UnitService(IUnitRepo unitRepo)
        {
            _unitRepo = unitRepo;
        }
        public async Task<PagedResult<ViewUnitModel>> GetAll(GetUnitPagingModel pagingModel)
        {
            return await _unitRepo.GetAll(pagingModel);
        }

        public async Task<ViewUnitModel> Get(string id)
        {
            return await _unitRepo.GetViewModel<ViewUnitModel>(id);
        }

        public async Task<CreateUnitErrorModel> Insert(string unitName)
        {
            var checkError = new CreateUnitErrorModel();
            if(await _unitRepo.CheckExistUnitName(unitName))
            {
                checkError.isError = true;
                checkError.alreadyExist = "Đơn vị tính sản phẩm này đã tồn tại";
                return checkError;
            }
            var unitID = Guid.NewGuid().ToString();
            var newUnitModel = new Repository.DatabaseModels.Unit()
            {
                Id = unitID,
                CreatedDate = DateTime.Now,
                Status = true,
                UnitName = unitName.Trim()
            };
            await _unitRepo.Insert(newUnitModel);
            checkError.isError = false;
            return checkError;
        }

        public async Task<UpdateUnitErrorModel> Update(string id, string unitName)
        {
            var checkError = new UpdateUnitErrorModel();
            if(await _unitRepo.CheckExistUnitNameUpdate(id, unitName))
            {
                checkError.isError = true;
                checkError.duplicateName = "Tên danh mục cập nhật bị trùng với danh mục khác!";
                return checkError;
            }
            var unit = await _unitRepo.Get(id);
            unit.UnitName = unitName.Trim();
            await _unitRepo.Update();
            checkError.isError = false;
            return checkError;
        }
    }
}
