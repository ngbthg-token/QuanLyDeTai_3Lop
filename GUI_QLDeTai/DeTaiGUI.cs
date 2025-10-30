using System;
using System.Collections.Generic;
using System.Linq;
using BLL_QLDeTai;
using DTO_QLDeTai;
using System.Globalization;
namespace GUI_QLDeTai
{
    public class DeTaiGUI
    {
        private readonly DeTaiBLL _deTaiBLL;

        public DeTaiGUI(DeTaiBLL bll)
        {
            _deTaiBLL = bll;
        }


        public void HienThiDanhSach(List<DeTaiDTO> danhSach, string tieuDe)
        {
            Console.WriteLine($"\n======= {tieuDe.ToUpper()} =======");
            if (danhSach == null || danhSach.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("DANH SÁCH RỖNG!");
                Console.ResetColor();
                return;
            }

            Console.ForegroundColor = ConsoleColor.Green;

            Console.WriteLine("{0,-10} | {1,-30} | {2,-18} | {3,-15} | {4,-10} | {5,-10} | {6,15}",
                             "Mã ĐT", "Tên Đề Tài", "Chủ Trì", "GVHD", "Ngày BĐ", "Ngày KT", "Kinh Phí (VND)");
            Console.WriteLine(new string('-', 126)); 
            Console.ResetColor();

            foreach (var dt in danhSach)
            {
                double kinhPhi = _deTaiBLL.TinhKinhPhi(dt);

                Console.WriteLine("{0,-10} | {1,-30} | {2,-18} | {3,-15} | {4,-10:dd/MM/yyyy} | {5,-10:dd/MM/yyyy} | {6,15:N0}",
                               dt.MaDeTai,
                               dt.TenDeTai,
                               dt.ChuTriDeTai,
                               dt.GiangVienHD,
                               dt.ThoiGianBatDau,
                               dt.ThoiGianKetThuc, 
                               kinhPhi);
            }
            Console.WriteLine(new string('-', 126)); 
            Console.WriteLine($"Hệ số kinh phí hiện tại: {DeTaiBLL.HeSoTangKinhPhi:P}");
        }

        public void InChiTietDeTai(DeTaiDTO dt)
        {
            if (dt == null)
            {
                Console.WriteLine("Không tìm thấy đề tài.");
                return;
            }

            double kinhPhi = _deTaiBLL.TinhKinhPhi(dt);

            Console.WriteLine("-----------------------------------------------------");
            Console.WriteLine($"Mã số: {dt.MaDeTai}");
            Console.WriteLine($"Tên đề tài: {dt.TenDeTai}");
            Console.WriteLine($"Người hướng dẫn: {dt.GiangVienHD}");
            Console.WriteLine($"Người chủ trì: {dt.ChuTriDeTai}");
            Console.WriteLine($"Thời gian: {dt.ThoiGianBatDau:dd/MM/yyyy} - {dt.ThoiGianKetThuc:dd/MM/yyyy}");
            Console.WriteLine($"Kinh phí: {kinhPhi:N0} VND");

            if (dt is DeTaiLyThuyetDTO dtlt)
            {
                Console.WriteLine($"Lĩnh vực: Lý thuyết");
                Console.WriteLine($"Khả năng triển khai: {(dtlt.ApDungThucTe ? "Có" : "Không")}");
            }
            else if (dt is DeTaiKinhTeDTO dtkt)
            {
                Console.WriteLine($"Lĩnh vực: Kinh tế");
                Console.WriteLine($"Số câu hỏi: {dtkt.SoCauHoi}");
                Console.WriteLine($"Kinh phí hỗ trợ: {dtkt.TinhKinhPhiHoTro():N0} VND");
            }
            else if (dt is DeTaiCongNgheDTO dtcn)
            {
                Console.WriteLine($"Lĩnh vực: Công nghệ");
                Console.WriteLine($"Môi trường: {dtcn.MoiTruong}");
                Console.WriteLine($"Kinh phí hỗ trợ: {dtcn.TinhKinhPhiHoTro():N0} VND");
            }
            Console.WriteLine("-----------------------------------------------------");
        }


        public void ThemDeTaiMoi()
        {
            Console.WriteLine("\n--- CHỨC NĂNG 1: THÊM MỚI ĐỀ TÀI ---");
            Console.WriteLine("Chọn loại đề tài bạn muốn thêm:");
            Console.WriteLine("1. Đề tài Lý thuyết");
            Console.WriteLine("2. Đề tài Kinh tế");
            Console.WriteLine("3. Đề tài Công nghệ");
            Console.Write("Lựa chọn của bạn (1-3): ");

            int loai;
            while (!int.TryParse(Console.ReadLine(), out loai) || loai < 1 || loai > 3)
            {
                Console.Write("Vui lòng nhập một số từ 1 đến 3: ");
            }

            Console.Write("Mã số đề tài: ");
            string ma = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(ma) || _deTaiBLL.TimKiemTheoMaSo(ma) != null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Lỗi: Mã đề tài không được để trống hoặc đã bị trùng.");
                Console.ResetColor();
                return;
            }

            Console.Write("Tên đề tài: ");
            string ten = Console.ReadLine();
            Console.Write("Giảng viên hướng dẫn: ");
            string gv = Console.ReadLine();
            Console.Write("Người chủ trì: ");
            string ct = Console.ReadLine();


            DateTime bd = NhapNgayThang("Ngày bắt đầu (dd/MM/yyyy): ");
            DateTime kt;

            do
            {
                kt = NhapNgayThang("Ngày kết thúc (dd/MM/yyyy): ");
                if (kt <= bd)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Lỗi: Ngày kết thúc phải sau ngày bắt đầu. Vui lòng nhập lại.");
                    Console.ResetColor();
                }
            } while (kt <= bd);

            DeTaiDTO dtMoi = null;

            try
            {
                switch (loai)
                {
                    case 1:
                        Console.Write("Đề tài có áp dụng thực tế không? (true/false): ");
                        bool ad;
                        while (!bool.TryParse(Console.ReadLine(), out ad))
                        {
                            Console.Write("Vui lòng nhập 'true' hoặc 'false': ");
                        }
                        dtMoi = new DeTaiLyThuyetDTO(ct, gv, ma, ten, bd, kt, ad);
                        break;
                    case 2:
                        Console.Write("Nhập số câu hỏi khảo sát: ");
                        int sch;
                        while (!int.TryParse(Console.ReadLine(), out sch) || sch <= 0)
                        {
                            Console.Write("Vui lòng nhập số nguyên dương: ");
                        }
                        dtMoi = new DeTaiKinhTeDTO(ct, gv, ma, ten, bd, kt, sch);
                        break;
                    case 3:
                        Console.Write("Nhập môi trường triển khai (Web/Mobile/Window): ");
                        string mt = Console.ReadLine();
                        dtMoi = new DeTaiCongNgheDTO(ct, gv, ma, ten, bd, kt, mt);
                        break;
                }

                if (_deTaiBLL.AddDeTai(dtMoi))
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("\nĐã thêm đề tài mới thành công.");
                    Console.WriteLine("Lưu ý: Thay đổi sẽ được lưu khi bạn chọn chức năng '0' (Lưu và Thoát).");
                    Console.ResetColor();
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Lỗi khi thêm đề tài: {ex.Message}");
                Console.ResetColor();
            }
        }


        public void TimKiem()
        {
            Console.WriteLine("\n--- CHỨC NĂNG 3: TÌM KIẾM ĐỀ TÀI ---");
            Console.WriteLine("Bạn muốn tìm kiếm theo tiêu chí nào?");
            Console.WriteLine("1. Tìm theo Mã số đề tài");
            Console.WriteLine("2. Tìm theo Tên đề tài");
            Console.WriteLine("3. Tìm theo Tên Giảng viên hướng dẫn");
            Console.WriteLine("4. Tìm theo Tên Người chủ trì");
            Console.Write("Vui lòng chọn (1-4): ");

            string timKiemLuaChon = Console.ReadLine();
            string keyword;
            List<DeTaiDTO> ketQuaTimKiem = new List<DeTaiDTO>();

            switch (timKiemLuaChon)
            {
                case "1":
                    Console.Write("Nhập Mã số cần tìm: ");
                    keyword = Console.ReadLine();
                    DeTaiDTO dtTheoMa = _deTaiBLL.TimKiemTheoMaSo(keyword);
                    if (dtTheoMa != null)
                    {
                        ketQuaTimKiem.Add(dtTheoMa);
                    }
                    break;
                case "2":
                    Console.Write("Nhập Tên đề tài cần tìm: ");
                    keyword = Console.ReadLine();
                    ketQuaTimKiem = _deTaiBLL.TimKiemTheoTen(keyword);
                    break;
                case "3":
                    Console.Write("Nhập Tên Giảng viên cần tìm: ");
                    keyword = Console.ReadLine();
                    ketQuaTimKiem = _deTaiBLL.TimKiemTheoTenGV(keyword);
                    break;
                case "4":
                    Console.Write("Nhập Tên Người chủ trì cần tìm: ");
                    keyword = Console.ReadLine();
                    ketQuaTimKiem = _deTaiBLL.TimKiemTheoChuTri(keyword);
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Lựa chọn không hợp lệ.");
                    Console.ResetColor();
                    return;
            }

            if (ketQuaTimKiem.Count == 0)
            {
                Console.WriteLine("\nKhông tìm thấy kết quả nào phù hợp.");
            }
            else
            {
                Console.WriteLine($"\nĐã tìm thấy {ketQuaTimKiem.Count} kết quả phù hợp:");
                foreach (var dt in ketQuaTimKiem)
                {
                    InChiTietDeTai(dt);
                }
            }
        }

        private DateTime NhapNgayThang(string prompt)
        {
            DateTime dateValue;
            Console.Write(prompt);
            while (!DateTime.TryParseExact(Console.ReadLine(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateValue))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("Lỗi: Vui lòng nhập ngày theo định dạng dd/MM/yyyy: ");
                Console.ResetColor();
            }
            return dateValue;
        }
    }
}