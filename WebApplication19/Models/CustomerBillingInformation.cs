﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VIMS.Models
{
    public class CustomerBillingInformation
    {
        [Key]
        public int Id { get; set; }
        public string BillNumber { get; set; }
        public int GetCustomerPolicyRecordId { get; set; }
        public CustomerPolicyRecord GetCustomerPolicyRecord { get; set; }

    }
}