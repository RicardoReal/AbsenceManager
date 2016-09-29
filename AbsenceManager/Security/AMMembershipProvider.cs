using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.Configuration;
using System.Collections.Specialized;
using System.Configuration.Provider;
using System.Configuration;
using System.Security.Cryptography;
using System.Text;
using AbsenceManager.Security;

namespace AbsenceManager
{
    public class AMMembershipProvider : MembershipProvider
    {
        #region Properties
        // Global connection string

        // Used when determining encryption key values.
        private MachineKeySection machineKey;

        private string pApplicationName;
        private bool pEnablePasswordReset;
        private bool pEnablePasswordRetrieval;
        private bool pRequiresQuestionAndAnswer;
        private bool pRequiresUniqueEmail;
        private int pMaxInvalidPasswordAttempts;
        private int pPasswordAttemptWindow;
        private MembershipPasswordFormat pPasswordFormat;
        private int pMinRequiredNonAlphanumericCharacters;
        private int pMinRequiredPasswordLength;
        private string pPasswordStrengthRegularExpression;

        public override string ApplicationName
        {
            get
            {
                return "Absence Manager";
            }
            set { }
        }

        public override bool EnablePasswordReset
        {
            get { return pEnablePasswordReset; }
        }

        public override bool EnablePasswordRetrieval
        {
            get { return pEnablePasswordRetrieval; }
        }

        public override int MinRequiredNonAlphanumericCharacters
        {
            get { return pMinRequiredNonAlphanumericCharacters; }
        }

        public override int MinRequiredPasswordLength
        {
            get { return pMinRequiredPasswordLength; }
        }

        public override string PasswordStrengthRegularExpression
        {
            get { return pPasswordStrengthRegularExpression; }
        }

        public override int PasswordAttemptWindow
        {
            get { return pPasswordAttemptWindow; }
        }

        public override MembershipPasswordFormat PasswordFormat
        {
            get { return pPasswordFormat; }
        }

        public override bool RequiresQuestionAndAnswer
        {
            get { return pRequiresQuestionAndAnswer; }
        }

        public override bool RequiresUniqueEmail
        {
            get { return pRequiresUniqueEmail; }
        }
        #endregion

        public override void Initialize(string name, NameValueCollection config)
        {
            // Initialize values from web.config.
            if (config == null)
                throw new ArgumentNullException("config");

            if (name == null || name.Length == 0)
                name = "AMMembershipProvider";

            if (String.IsNullOrEmpty(config["description"]))
            {
                config.Remove("description");
                config.Add("description", "AbsenceManager Membership provider");
            }

            // Initialize the abstract base class.
            base.Initialize(name, config);

            pApplicationName = GetConfigValue(config["applicationName"], System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath);
            pMaxInvalidPasswordAttempts = Convert.ToInt32(GetConfigValue(config["maxInvalidPasswordAttempts"], "5"));
            pPasswordAttemptWindow = Convert.ToInt32(GetConfigValue(config["passwordAttemptWindow"], "10"));
            pMinRequiredNonAlphanumericCharacters = Convert.ToInt32(GetConfigValue(config["minRequiredNonAlphanumericCharacters"], "0"));
            pMinRequiredPasswordLength = Convert.ToInt32(GetConfigValue(config["minRequiredPasswordLength"], "6"));
            pPasswordStrengthRegularExpression = Convert.ToString(GetConfigValue(config["passwordStrengthRegularExpression"], ""));
            pEnablePasswordReset = Convert.ToBoolean(GetConfigValue(config["enablePasswordReset"], "false"));
            pEnablePasswordRetrieval = Convert.ToBoolean(GetConfigValue(config["enablePasswordRetrieval"], "false"));
            pRequiresQuestionAndAnswer = Convert.ToBoolean(GetConfigValue(config["requiresQuestionAndAnswer"], "false"));
            pRequiresUniqueEmail = Convert.ToBoolean(GetConfigValue(config["requiresUniqueEmail"], "false"));

            string temp_format = config["passwordFormat"];
            if (temp_format == null)
            {
                temp_format = "Encrypted";
            }

            switch (temp_format)
            {
                case "Hashed":
                    pPasswordFormat = MembershipPasswordFormat.Hashed;
                    break;
                case "Encrypted":
                    pPasswordFormat = MembershipPasswordFormat.Encrypted;
                    break;
                case "Clear":
                    pPasswordFormat = MembershipPasswordFormat.Clear;
                    break;
                default:
                    throw new ProviderException("Password format not supported.");
            }

            // Initialize Connection.
            ConnectionStringSettings ConnectionStringSettings =
              ConfigurationManager.ConnectionStrings[config["connectionStringName"]];

            if (ConnectionStringSettings == null || ConnectionStringSettings.ConnectionString.Trim() == "")
            {
                throw new ProviderException("Connection string cannot be blank.");
            }

            // Get encryption and decryption key information from the configuration.
            Configuration cfg =
              WebConfigurationManager.OpenWebConfiguration(System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath);
            machineKey = (MachineKeySection)cfg.GetSection("system.web/machineKey");

            if (machineKey.ValidationKey.Contains("AutoGenerate"))
                if (PasswordFormat != MembershipPasswordFormat.Clear)
                    throw new ProviderException("Hashed or Encrypted passwords " +
                                                "are not supported with auto-generated keys.");
        }

        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            if (!ValidateUser(username, oldPassword))
                return false;

            ValidatePasswordEventArgs args =
              new ValidatePasswordEventArgs(username, newPassword, true);

            OnValidatingPassword(args);

            if (args.Cancel)
                if (args.FailureInformation != null)
                    throw args.FailureInformation;
                else
                    throw new MembershipPasswordException("Change password canceled due to new password validation failure.");

            using (AM_Entities ent = new AM_Entities())
            {
                User user = (from u in ent.Users where u.UserName == username select u).FirstOrDefault();

                user.Password = newPassword;

                ent.SaveChanges();
            }

            return true;
        }

        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            using (AM_Entities ent = new AM_Entities())
            {
                User user = (from us in ent.Users where us.UserName == username select us).FirstOrDefault();
                MembershipUser u = null;

                if (user != null) u = GetMembershipUser(user.UserName, (DateTime)user.CreationDate);

                return u;
            }
        }

        public override bool ValidateUser(string username, string password)
        {
            using (AM_Entities ent = new AM_Entities())
            {
                User user = (from u in ent.Users where u.UserName == username select u).FirstOrDefault();

                if (user == null) return false;

                return CheckPassword(password, user.Password);
            }
        }

        public string GetName(string userName)
        {
            using (AM_Entities ent = new AM_Entities())
            {
                return (from u in ent.Users where u.UserName == userName select u.Nome).FirstOrDefault();
            }
        }

        #region Não utilizados
        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override string GetUserNameByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public override string GetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override int MaxInvalidPasswordAttempts
        {
            get { throw new NotImplementedException(); }
        }

        public override int GetNumberOfUsersOnline()
        {
            throw new NotImplementedException();
        }

        public override bool UnlockUser(string userName)
        {
            throw new NotImplementedException();
        }

        public override string ResetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            throw new NotImplementedException();
        }

        public override void UpdateUser(MembershipUser user)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Helpers
        private string GetConfigValue(string configValue, string defaultValue)
        {
            if (String.IsNullOrEmpty(configValue))
                return defaultValue;

            return configValue;
        }

        // CheckPassword
        //   Compares password values based on the MembershipPasswordFormat.
        private bool CheckPassword(string password, string dbpassword)
        {
            Cipher c = new Cipher();
            bool b = false;

            c.HashVerify(password, "SHA256", dbpassword, out b);

            return b;
        }

        // EncodePassword
        //   Encrypts, Hashes, or leaves the password clear based on the PasswordFormat.
        public string EncodePassword(string password)
        {
            string encodedPassword = password;

            Cipher c = new Cipher();
            String s = String.Empty;
            switch (PasswordFormat)
            {
                case MembershipPasswordFormat.Clear:
                    break;
                case MembershipPasswordFormat.Encrypted:
                    c.HashCompute(password, "SHA256", out s);
                    encodedPassword = s;
                    break;
                case MembershipPasswordFormat.Hashed:
                    HMACSHA1 hash = new HMACSHA1();
                    hash.Key = HexToByte(machineKey.ValidationKey);
                    encodedPassword =
                      Convert.ToBase64String(hash.ComputeHash(Encoding.Unicode.GetBytes(password)));
                    break;
                default:
                    throw new ProviderException("Unsupported password format.");
            }

            return encodedPassword;
        }

        /// <summary>
        /// DEPRECATED - DO NOT USE
        /// </summary>
        public string DecodePassword(string password)
        {
            return Encoding.Unicode.GetString(DecryptPassword(Convert.FromBase64String(password)));
        }

        // HexToByte
        //   Converts a hexadecimal string to a byte array. Used to convert encryption
        // key values from the configuration.
        private byte[] HexToByte(string hexString)
        {
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            return returnBytes;
        }

        //
        // GetUserFromReader
        //    A helper function that takes the current row from the OdbcDataReader
        // and hydrates a MembershiUser from the values. Called by the 
        // MembershipUser.GetUser implementation.
        //

        private MembershipUser GetMembershipUser(String userName, DateTime creationDate)
        {
            object providerUserKey = null;
            string email = "";
            string passwordQuestion = "";
            string comment = "";
            bool isApproved = true;
            bool isLockedOut = false;
            DateTime lastLoginDate = new DateTime();
            DateTime lastActivityDate = new DateTime();
            DateTime lastPasswordChangedDate = new DateTime();

            DateTime lastLockedOutDate = new DateTime();

            MembershipUser u = new MembershipUser(this.Name,
                                                  userName,
                                                  providerUserKey,
                                                  email,
                                                  passwordQuestion,
                                                  comment,
                                                  isApproved,
                                                  isLockedOut,
                                                  creationDate,
                                                  lastLoginDate,
                                                  lastActivityDate,
                                                  lastPasswordChangedDate,
                                                  lastLockedOutDate);

            return u;
        }
        #endregion
    }
}