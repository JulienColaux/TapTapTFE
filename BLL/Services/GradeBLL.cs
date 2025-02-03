using DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace BLL.Services
{
    public class GradeBLL
    {
        //----------------------------INJECTION DE LA DE LA DAL-------------------------------------------------------------------


        private readonly GradeDAL _gradeDAL;

        public GradeBLL(GradeDAL gradeDAL)
        {
            _gradeDAL = gradeDAL;
        }


        //----------------------------GET Grade BY ID-----------------------------------------------------------------------------


        public async Task<Grade> GetTropheeById(int id)
        {
            if (id <= 0)
            {
                throw new System.Exception("L'ID n'est pas valide.");
            }
            return await _gradeDAL.GetGradeById(id);
        }


        //-------------------------------GET ALL GRADE--------------------------------------------------------------------------------------------------------


        public async Task<List<Grade>> GetAllGrades()
        {
            return await _gradeDAL.GetAllGrades();
        }
    }
}
