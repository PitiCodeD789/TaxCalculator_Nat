using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace TaxEngine.Models
{
    public class TaxCalculatorCommand 
    {


        /// <summary>
        /// ก1 รายรับต่อปีทั้งหมด
        /// </summary>
        public decimal TotalIncome { get; set; }
        /// <summary>
        /// ข4 ผู้เสียภาษีพิการหรือเปล่า
        /// </summary>
        public bool IsDisabled { get; set; }
        /// <summary>
        /// อายุ
        /// </summary>
        public int Age { get; set; }
        /// <summary>
        /// ข3 กองทุนสงเคราะห์ครูเอกชน
        /// </summary>
        public decimal TeacherFund { get; set; }
        /// <summary>
        /// ข1 กองทุนสำรองเลี้ยงชีพ
        /// </summary>
        public decimal ProvFund { get; set; }
        /// <summary>
        /// ข2  กองทุนข้าราชการบำนาญ
        /// </summary>
        public decimal GPF { get; set; }
        /// <summary>
        /// ข5 ชดเชยตามกฏหมายแรงงาน
        /// </summary>
        public decimal LeaveCompensate { get; set; }
        /// <summary>
        /// 1 จ่ายภาษีร่วมกับคู่สมรสหรือไม่
        /// </summary>
        public bool ApplyWithSpouse { get; set; }
        /// <summary>
        /// 2 ผู้เสียภาษีแต่งงานหรือยัง
        /// </summary>
        public bool IsMarried { get; set; }
        /// <summary>
        /// 2 คู่สมรสผู้เสียภาษีหักลดได้มั้ย
        /// </summary>
        public bool IsSpouseEligible { get; set; }
        /// <summary>
        /// 3 ลูกที่ลดได้ที่เกิดก่อน 2561
        /// </summary>
        public int EligibleChildBefore2561 { get; set; }
        /// <summary>
        /// 3 ลูกเลี้ยงที่ลดหย่อนได้
        /// </summary>
        public int EligibleAdoptedChild { get; set; }
        /// <summary>
        /// 3 ลูกที่ลดได้ที่เกิดก่อนที่เกิดหลัง 2561
        /// </summary>
        public int EligibleChildAfter2561 { get; set; }
        /// <summary>
        /// 4 จำนวนพ่อแม่ของตัวเองที่ใช้สิทธิ
        /// </summary>
        public int OwnParent { get; set; }
        /// <summary>
        /// 4 จำนวนพ่อแม่ของคู่สมรสที่ใช้สิทธิ
        /// </summary>
        public int SpouseParent { get; set; }
        /// <summary>
        /// 5 มีการดูแลคนพิการที่ไม่ใช่คนในครอบครัว
        /// </summary>
        public bool HasNonFamilyDisable { get; set; }
        /// <summary>
        /// 5 จำนวนคนพิการในครบครัวที่ดูแล
        /// </summary>
        public int InFamilyDisable { get; set; }
        /// <summary>
        /// 6 ประกันของพ่อแม่
        /// </summary>
        public decimal ParentalInsurance { get; set; }
        /// <summary>
        /// 7.1 เบี้ยประกันชีวิตตนเอง
        /// </summary>
        public decimal LifeInsurance { get; set; }
        /// <summary>
        /// 7.2 เบี้ยประกันสุขภาพตนเอง
        /// </summary>
        public decimal HealthInsurance { get; set; }
        /// <summary>
        /// 9 เงินสะสมกองทุนการออมแห่งชาติ
        /// </summary>
        public decimal SavingFund { get; set; }
        /// <summary>
        /// 11 LTF
        /// </summary>
        public decimal LTF { get; set; }
        /// <summary>
        /// 10 RMF
        /// </summary>
        public decimal RMF { get; set; }
        /// <summary>
        /// 7.3 เบี้ยประกันชีวิตแบบบำนาญ
        /// </summary>
        public decimal PensionInsurance { get; set; }
        /// <summary>
        /// 20. เงินบริจาคพรรคการเมือง 
        /// </summary>
        public decimal PoliticalParty { get; set; }
        /// <summary>
        /// 17. ค่าท่องเที่ยว “จังหวัดท่องเที่ยวรอง” + หลักแล้ว
        /// </summary>
        public decimal TravelExpense { get; set; }
        /// <summary>
        /// 14 ประกันสังคม
        /// </summary>
        public decimal SocialSecurity { get; set; }
        /// <summary>
        /// 12 ดอกเบี้ยอสังหา
        /// </summary>
        public decimal HousingInterest { get; set; }
        /// <summary>
        /// 18 ค่าลงหุ้นหรือลงทุนในการจัดตั้งหรือเพิ่มทุนในกิจการที่ประกอบอุตสาหกรรมเป้าหมาย
        /// </summary>
        public decimal StartupInvestment { get; set; }
        /// <summary>
        /// 15 ค่าซื้อและค่าติดตั้งระบบกล้องโทรทัศน์วงจรปิด
        /// </summary>
        public decimal CctvExpense { get; set; }
        /// <summary>
        /// 21 ค่าซื้อสินค้าหรือค่าบริการ
        /// </summary>
        public decimal OtherExpense { get; set; }
        /// <summary>
        /// ก8 เงินบริจาคสนับสนุนการศึกษา การกีฬา
        /// </summary>
        public decimal EducationDonation { get; set; }
        /// <summary>
        /// ก10 เงินบริจาค
        /// </summary>
        public decimal OtherDonation { get; set; }
        /// <summary>
        /// 13 เงินได้ที่จ่ายเพื่อซื้ออสังหาริมทรัพย์ฯ
        /// </summary>
        public decimal RealEstatePrice { get; set; }
        /// <summary>
        /// ปีที่ซื้อบ้าน พศ 4 หลัก
        /// </summary>
        public int HouseYearBuy { get; set; }
        /// <summary>
        /// 19 ค่าฝากครรภ์และค่าคลอดบุตร
        /// </summary>
        public decimal MaternityAllowance { get; set; }
        /// <summary>
        /// จำนวนภาษีที่คาดหวัง
        /// </summary>
        public decimal ExpectedResult { get; set; }
    }
}
