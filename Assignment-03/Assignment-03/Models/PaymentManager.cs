using Assignment_03.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment_03.Models
{
    public class PaymentManager
    {
        private IPaymentServices _PaymentService;
        public void SetService(IPaymentServices PaymentService)
        {
            _PaymentService = PaymentService;
        }
        public void DoPayment()
        {
            _PaymentService.MakeTransaction();
        }
        public int GetAvailableBalance()
        {
            return _PaymentService.GetAvailableAmount();
        }

        public bool ValidateDetails()
        {
            bool status=_PaymentService.ValidatePayment();
            return status;
        }

    }
}
