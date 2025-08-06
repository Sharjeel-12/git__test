using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment_03.Services
{
    public interface IPaymentServices
    {
        void MakeTransaction();
        bool ValidatePayment();
        int GetAvailableAmount();
    }

    public class CreditCard : IPaymentServices
    {
        private static readonly int _CardNumber=1111111;
        private static readonly int _CVC = 222;
        private static  int _AvailableAmount = 40000;
        public static int Card_number;
        public static int CVC;
        public static int Amount;
        public CreditCard(int card_number,int cvc, int amount)
        {
            Card_number = card_number;
            CVC = cvc;
            Amount = amount;
        }
        public int GetAvailableAmount()
        {
            return _AvailableAmount;
        }
        public bool ValidatePayment()
        {
            bool valid = false;
            if (Card_number == _CardNumber && CVC==_CVC) 
            {
                Console.WriteLine("Your card details are valid");
                valid = true;
            }
            else
            {
                Console.WriteLine("Invalid card details. Please enter correct Card number or CVC");
            }
            return valid;
        }

        public void MakeTransaction()
        {
            if (ValidatePayment())
            {
                if (Amount > _AvailableAmount)
                {
                    Console.WriteLine("Transaction Successful");
                    _AvailableAmount = _AvailableAmount-Amount;
                }
                else
                {
                    Console.WriteLine("The card doesnot have sufficient balance");
                }
            }
        }


    }



    public class PayPalPayment : IPaymentServices
    {
        private static readonly string _Email="username@example.com";
        private static readonly string _Password = "12345";
        private static int _AvailableAmount = 40000;
        public static string Email;
        public static string Password;
        public static int Amount;
        
        public PayPalPayment(string email, string password, int amount)
        {
            Email= email;
            Password= password;
            Amount = amount;
        }
        public int GetAvailableAmount()
        {
            return _AvailableAmount;
        }

        public bool ValidatePayment()
        {
            bool valid = false;
            if (Email==_Email && Password==_Password)
            {
                Console.WriteLine("Your account details are valid");
                valid = true;
            }
            else
            {
                Console.WriteLine("Invalid credentials. Please enter correct email or password");
            }
            return valid;
        }

        public void MakeTransaction()
        {
            if (ValidatePayment())
            {
                if (Amount > _AvailableAmount)
                {
                    Console.WriteLine("Transaction Successful");
                    _AvailableAmount = _AvailableAmount - Amount;
                }
                else
                {
                    Console.WriteLine("The card doesnot have sufficient balance");
                }
            }
        }


    }




    public class BankTransferPayment : IPaymentServices
    {
        private static readonly string _AccountNumber = "1000000000001";
        private static readonly string _Pin = "1234";
        private static int _AvailableAmount = 60000;
        public static string AccountNumber;
        public static string Pin;
        public static int Amount;

        public BankTransferPayment(string acct_num, string pin, int amount)
        {
            AccountNumber = acct_num;
            Pin = pin;
            Amount = amount;
        }
        public int GetAvailableAmount()
        {
            return _AvailableAmount;
        }
        public bool ValidatePayment()
        {
            bool valid = false;
            if (AccountNumber==_AccountNumber && Pin == _Pin)
            {
                Console.WriteLine("Your account details are valid");
                valid = true;
            }
            else
            {
                Console.WriteLine("Invalid credentials. Please enter correct Pin or Account Number");
            }
            return valid;
        }

        public void MakeTransaction()
        {
            if (ValidatePayment())
            {
                if (Amount > _AvailableAmount)
                {
                    Console.WriteLine("Transaction Successful");
                    _AvailableAmount = _AvailableAmount - Amount;
                }
                else
                {
                    Console.WriteLine("The card doesnot have sufficient balance");
                }
            }
        }


    }

    public class CryptoPayment : IPaymentServices
    {
        private static readonly string _WalletNumber = "100001";
        private static int _AvailableAmount = 600000;
        public static string WalletNumber;
        public static int Amount;

        public CryptoPayment(string wallet_num, int amount)
        {
            WalletNumber=wallet_num;
            Amount = amount;
        }
        public int GetAvailableAmount()
        {
            return _AvailableAmount;
        }
        public bool ValidatePayment()
        {
            bool valid = false;
            if (WalletNumber==_WalletNumber)
            {
                Console.WriteLine("Your account details are valid");
                valid = true;
            }
            else
            {
                Console.WriteLine("Invalid credentials. Please enter correct wallent Number");
            }
            return valid;
        }

        public void MakeTransaction()
        {
            if (ValidatePayment())
            {
                if (Amount > _AvailableAmount)
                {
                    Console.WriteLine("Transaction Successful");
                    _AvailableAmount = _AvailableAmount - Amount;
                }
                else
                {
                    Console.WriteLine("The card doesnot have sufficient balance");
                }
            }
        }


    }

    //*****************************************************************************************************************

}
