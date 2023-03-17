using BetterHealthManagementAPI.BetterHealth2023.Business.Utils;
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

        public async Task<CreateUnitErrorModel> Insert(CreateUnitModel createUnitModel)
        {
            var checkError = new CreateUnitErrorModel();
            if(await _unitRepo.CheckExistUnitName(createUnitModel.UnitName))
            {
                checkError.isError = true;
                checkError.alreadyExist = "Đơn vị tính sản phẩm này đã tồn tại";
                return checkError;
            }
            var unitID = Guid.NewGuid().ToString();
            var newUnitModel = new Repository.DatabaseModels.Unit()
            {
                Id = unitID,
                CreatedDate = CustomDateTime.Now,
                IsCountable = createUnitModel.isCountable,
                Status = true,
                UnitName = createUnitModel.UnitName.Trim()
            };
            await _unitRepo.Insert(newUnitModel);
            checkError.isError = false;
            return checkError;
        }

        public async Task<UpdateUnitErrorModel> Update(UpdateUnitModel updateUnitModel)
        {
            var checkError = new UpdateUnitErrorModel();
            if(await _unitRepo.CheckExistUnitNameUpdate(updateUnitModel.Id, updateUnitModel.UnitName))
            {
                checkError.isError = true;
                checkError.duplicateName = "Tên đơn vị cần cập nhật bị trùng với đơn vị khác!";
                return checkError;
            }
            var unit = await _unitRepo.Get(updateUnitModel.Id);
            unit.UnitName = updateUnitModel.UnitName.Trim();
            unit.IsCountable = updateUnitModel.isCountable;
            await _unitRepo.Update();
            checkError.isError = false;
            return checkError;
        }
    }
}
