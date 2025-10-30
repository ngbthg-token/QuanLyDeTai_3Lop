using System;
using System.Text;
using BLL_QLDeTai;
using DTO_QLDeTai;
using System.Linq;

namespace GUI_QLDeTai
{
    class Program
    {
        private static DeTaiBLL _deTaiBLL;
        private static DeTaiGUI _deTaiGUI;
        private const string TEN_FILE_XML = @"../../DeTaiNghienCuu.xml";

        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;

            _deTaiBLL = new DeTaiBLL();
            _deTaiGUI = new DeTaiGUI(_deTaiBLL);


            _deTaiBLL.LoadData(TEN_FILE_XML);

            while (true)
            {
                HienThiMenu();
                Console.Write("Mời bạn chọn chức năng: ");
                string luaChon = Console.ReadLine();

                if (luaChon == "0")
                {
                 
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("\nĐang thực hiện lưu dữ liệu vào file...");


                    if (_deTaiBLL.LuuDuLieu(TEN_FILE_XML))
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Đã lưu dữ liệu thành công!");
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("LỖI: Lưu dữ liệu không thành công!");
                    }

                    Console.ResetColor();
                    Console.WriteLine("Cảm ơn đã sử dụng chương trình!");
                    break; 
                }

                XuLyChucNang(luaChon);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\nNhấn phím bất kỳ để tiếp tục...");
                Console.ResetColor();
                Console.ReadKey();
                Console.Clear();
            }
        }

        static void HienThiMenu()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("=========================================================");
            Console.WriteLine("==     CHUONG TRINH QUAN LY DE TAI NGHIEN CUU KHOA HOC ==");
            Console.WriteLine("=========================================================");
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("== 1. Thêm mới một đề tài vào danh sách.               ==");
            Console.WriteLine("== 2. Xuất toàn bộ danh sách đề tài.                   ==");
            Console.WriteLine("== 3. Tìm kiếm đề tài.                                 ==");
            Console.WriteLine("== 4. Xuất danh sách đề tài theo tên Giảng viên.       ==");
            Console.WriteLine("== 5. Cập nhật kinh phí (tăng 10%).                    ==");
            Console.WriteLine("== 6. Xuất danh sách đề tài có kinh phí > 10 triệu.    ==");
            Console.WriteLine("== 7. Xuất ds đề tài Lý thuyết có thể triển khai.      ==");
            Console.WriteLine("== 8. Xuất ds đề tài Kinh tế có > 100 câu hỏi khảo sát.==");
            Console.WriteLine("== 9. Xuất ds đề tài có thời gian thực hiện > 4 tháng. ==");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("== 0. Lưu và Thoát chương trình.                       ==");
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("=========================================================");
            Console.ResetColor();
        }

        static void XuLyChucNang(string luaChon)
        {
            switch (luaChon)
            {
                case "1":
                    _deTaiGUI.ThemDeTaiMoi();
                    break;
                case "2":
                    _deTaiGUI.HienThiDanhSach(_deTaiBLL.GetAllDeTai(), "DANH SÁCH TẤT CẢ ĐỀ TÀI");
                    break;
                case "3":
                    _deTaiGUI.TimKiem();
                    break;
                case "4":
                    Console.WriteLine("\n--- CHỨC NĂNG 4: XUẤT DS THEO TÊN GIẢNG VIÊN ---");
                    Console.Write("Nhập tên Giảng viên cần xem: ");
                    string tenGV = Console.ReadLine();
                    var dsTheoGV = _deTaiBLL.TimKiemTheoTenGV(tenGV);
                    _deTaiGUI.HienThiDanhSach(dsTheoGV, $"DS ĐỀ TÀI CỦA GV: {tenGV}");
                    break;
                case "5":
                    Console.WriteLine("\n--- CHỨC NĂNG 5: CẬP NHẬT KINH PHÍ (TĂNG 10%) ---");
                    _deTaiBLL.CapNhatKinhPhiTang10PhanTram();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Đã cập nhật hệ số tăng kinh phí. Hệ số mới: {DeTaiBLL.HeSoTangKinhPhi:P}");
                    Console.ResetColor();
                    break;
                case "6":
                    Console.WriteLine("\n--- CHỨC NĂNG 6: XUẤT DS CÓ KINH PHÍ > 10 TRIỆU ---");
                    var dsTren10Tr = _deTaiBLL.LayDSTheoKinhPhi(10000000);
                    _deTaiGUI.HienThiDanhSach(dsTren10Tr, "DS ĐỀ TÀI CÓ KINH PHÍ > 10 TRIỆU");
                    break;
                case "7":
                    Console.WriteLine("\n--- CHỨC NĂNG 7: XUẤT DS LÝ THUYẾT CÓ KHẢ NĂNG TRIỂN KHAI ---");
                    var dsLTCoTK = _deTaiBLL.GetDeTaiLyThuyetCoKhaNangTrienKhai();
                    _deTaiGUI.HienThiDanhSach(dsLTCoTK.Cast<DeTaiDTO>().ToList(), "DS ĐỀ TÀI LÝ THUYẾT CÓ KHẢ NĂNG TRIỂN KHAI");
                    break;
                case "8":
                    Console.WriteLine("\n--- CHỨC NĂNG 8: XUẤT DS KINH TẾ CÓ > 100 CÂU HỎI ---");
                    var dsKTCauHoi = _deTaiBLL.GetDSKinhTeTheoSoCauHoi(100);
                    _deTaiGUI.HienThiDanhSach(dsKTCauHoi.Cast<DeTaiDTO>().ToList(), "DS ĐỀ TÀI KINH TẾ CÓ > 100 CÂU HỎI");
                    break;
                case "9":
                    Console.WriteLine("\n--- CHỨC NĂNG 9: XUẤT DS CÓ THỜI GIAN THỰC HIỆN > 4 THÁNG ---");
                    var dsTheoTG = _deTaiBLL.GetDSTheoThoiGian(4);
                    _deTaiGUI.HienThiDanhSach(dsTheoTG, "DS ĐỀ TÀI THỰC HIỆN > 4 THÁNG");
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Lựa chọn không hợp lệ. Vui lòng chọn lại.");
                    Console.ResetColor();
                    break;
            }
        }
    }
}