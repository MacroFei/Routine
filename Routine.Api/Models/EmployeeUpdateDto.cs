using Routine.Api.Entities;
using Routine.Api.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Routine.Api.Models
{
    //[EmployeeNoMustDifferentFromFirstNameAttribute(ErrorMessage ="员工编号必须和名不一样！！")]
    public class EmployeeUpdateDto : EmployeeAddOrUpdateDto
    {
       
    }
}
