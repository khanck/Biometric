namespace TCC.Payment.Integration.Models
{
    public class UserInfo
    {
        public string ID { get; set; } = "9999";
        public string UniqueID { get; set; } = "9999";
        public string Name { get; set; } = "User9999";
        public List<int> AuthInfo { get; set; } = new List<int> { 3, 9,0, 0, 0, 0, 0, 0 };
        public int Privilege { get; set; } = 2;
        public string CreateDate { get; set; } = "2024-04-11 01:30:02";
        public int UsePeriodFlag { get; set; } = 0;
        public string RegistDate { get; set; } = "2024-04-11 01:30:02";
        public string ExpireDate { get; set; } = "2027-04-11 01:30:02";
        public string Password { get; set; } = "1111";
        public int GroupCode { get; set; } = 0;
        public int AccessGroupCode { get; set; } = 0;
        public int UserType { get; set; } = 0;
        public int TimezoneCode { get; set; } = 0;
        public int BlackList { get; set; } = 0;
        public int FPIdentify { get; set; } = 0;
        public int FaceIdentify { get; set; } = 0;
        public List<int> DuressFinger { get; set; } = new List<int> { 0, 0, 0 };
        public int Partition { get; set; } = 0;
        public int APBExcept { get; set; } = 0;
        public int APBZone { get; set; } = 0;
        public string WorkCode { get; set; } = "****";
        public string MealCode { get; set; } = "****";
        public string MoneyCode { get; set; } = "****";
        public int MessageCode { get; set; } = 0;
        public int VerifyLevel { get; set; } = 0;
        public int PositionCode { get; set; } = 1000;
        public string Department { get; set; } = "Department";
        public string LoginPW { get; set; } = "LoginPassword";
        public int LoginAllowed { get; set; } = 0;
        public string Picture { get; set; } = "";
        public string EmployeeNum { get; set; } = "A20180100001";
        public string Email { get; set; } = "ABC@gmail.co.kr";
        public string Phone { get; set; } = "***-****-****";
    }

    public class UserFPInfo
    {
        public int FingerID { get; set; } = 1;
        public int MinConvType { get; set; } = 3;
        public int TemplateIndex { get; set; } = 1;
        public string TemplateData { get; set; } = "TestData";
    }

    public class UserFaceInfo
    {
        public int UserID { get; set; } = 1;
        public int Index { get; set; } = 1;
        public int Type { get; set; } = 0;
        public int SubIndex { get; set; } = 1;
        public int TemplateSize { get; set; } = 4042;
        public string TemplateData { get; set; } = "";
    }

    public class UserCardInfo
    {
        public string CardNum { get; set; } = "TestNum";
    }

    public class UserFaceWTInfo
    {
        public int UserID { get; set; } = 1;
        public int TemplateSize { get; set; } = 1;
        public string TemplateData { get; set; } = "TestData";
        public int TemplateType { get; set; } = 1;
    }

    public class UserIrisInfo
    {
        public int UserID { get; set; } = 1;
        public int EyeType { get; set; } = 1;
        public int TemplateSize { get; set; } = 1;
        public string TemplateData { get; set; } = "";
    }

    public class CreateUserRequestDTO
    {
        public UserInfo UserInfo { get; set; } = new UserInfo();
        public List<UserFPInfo> UserFPInfo { get; set; } = new List<UserFPInfo>
    {
        new UserFPInfo { FingerID = 1, TemplateIndex = 1, TemplateData = "TestData" },
        new UserFPInfo { FingerID = 1, TemplateIndex = 2, TemplateData = "TestData" }
    };
    //    public List<UserFaceInfo> UserFaceInfo { get; set; } = new List<UserFaceInfo>
    //{
    //    new UserFaceInfo { SubIndex = 1 },
        
    //};
        public List<UserCardInfo> UserCardInfo { get; set; } = new List<UserCardInfo>
    {
        new UserCardInfo { CardNum = "TestNum" }
    };
        public List<UserFaceWTInfo> UserFaceWTInfo { get; set; } = new List<UserFaceWTInfo>();
        public List<UserIrisInfo> UserIrisInfo { get; set; } = new List<UserIrisInfo>
    {
        new UserIrisInfo { TemplateData = "" }
    };
        public void UpdateUserID(int newUserID)
        {
            //foreach (var faceInfo in UserFaceInfo)
            //{
            //    faceInfo.UserID = newUserID;
            //}

           
            
            foreach (var irisInfo in UserIrisInfo)
            {
                irisInfo.UserID = newUserID;
            }

          
        }
    }
}
