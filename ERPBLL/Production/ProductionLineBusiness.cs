using ERPBLL.Production.Interface;
using ERPBO.Production.DomainModels;
using ERPBO.Production.DTOModel;
using ERPDAL.ProductionDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Production
{
   public class ProductionLineBusiness: IProductionLineBusiness
    {
        private readonly IProductionUnitOfWork _productionDb; // database
        private readonly ProductionLineRepository productionLineRepository; // table
        public ProductionLineBusiness(IProductionUnitOfWork productionDb)
        {
            this._productionDb = productionDb;
            productionLineRepository = new ProductionLineRepository(this._productionDb);
        }

        public IEnumerable<ProductionLine> GetAllProductionLineByOrgId(long orgId)
        {
            return productionLineRepository.GetAll(line => line.OrganizationId == orgId).ToList();
        }

        public ProductionLine GetProductionLineOneByOrgId(long id, long orgId)
        {
            return productionLineRepository.GetOneByOrg(line => line.LineId == id && line.OrganizationId == orgId);
        }

        public bool IsDuplicateLineNumber(string lineNumber, long id, long orgId)
        {
            return productionLineRepository.GetOneByOrg(line => line.LineNumber == lineNumber && line.LineId != id && line.OrganizationId == orgId) != null ? true : false;
        }

        public bool SaveUnit(ProductionLineDTO lineDTO, long userId, long orgId)
        {
            ProductionLine line = new ProductionLine();
            if (lineDTO.LineId == 0)
            {
                line.LineId = lineDTO.LineId;
                line.LineNumber = lineDTO.LineNumber;
                line.LineIncharge = lineDTO.LineIncharge;
                line.Remarks = lineDTO.Remarks;
                line.IsActive = lineDTO.IsActive;
                line.OrganizationId = orgId;
                line.EUserId = lineDTO.EUserId;
                line.EntryDate = DateTime.Now;
                productionLineRepository.Insert(line);
            }
            else
            {
                line = GetProductionLineOneByOrgId(lineDTO.LineId, orgId);
                line.LineNumber = lineDTO.LineNumber;
                line.LineIncharge = lineDTO.LineIncharge;
                line.Remarks = lineDTO.Remarks;
                line.IsActive = lineDTO.IsActive;
                line.OrganizationId = orgId;
                line.UpUserId = lineDTO.UpUserId;
                line.UpdateDate = DateTime.Now;
                productionLineRepository.Update(line);
            }
            return productionLineRepository.Save();
        }
    }
}
