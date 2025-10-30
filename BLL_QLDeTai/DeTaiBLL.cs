using System;
using System.Collections.Generic;
using System.Linq;
using DTO_QLDeTai;
using DAL_QLDeTai;

namespace BLL_QLDeTai
{
    public class DeTaiBLL
    {
        private DeTaiDAL _deTaiDAL = new DeTaiDAL();
        private List<DeTaiDTO> _danhSachDeTai;
        public static double HeSoTangKinhPhi = 1.0;

        public DeTaiBLL()
        {
            _danhSachDeTai = new List<DeTaiDTO>();
        }

        public void LoadData(string tenFileXML)
        {
            _danhSachDeTai = _deTaiDAL.DocFileXML(tenFileXML);
            if (_danhSachDeTai != null && _danhSachDeTai.Count > 0)
            {
                Console.WriteLine($"Đã tải thành công {_danhSachDeTai.Count} đề tài từ file {tenFileXML}");
            }
        }


        public bool LuuDuLieu(string tenFileXML)
        {
         
            return _deTaiDAL.LuuFileXML(tenFileXML, _danhSachDeTai);
        }

        public List<DeTaiDTO> GetAllDeTai()
        {
            return _danhSachDeTai;
        }

        public bool AddDeTai(DeTaiDTO dtMoi)
        {
            if (TimKiemTheoMaSo(dtMoi.MaDeTai) != null)
            {
                Console.WriteLine("Lỗi: Mã đề tài đã tồn tại. Không thể thêm.");
                return false;
            }
            _danhSachDeTai.Add(dtMoi);
            return true;
        }

        public DeTaiDTO TimKiemTheoMaSo(string maSo)
        {
            return _danhSachDeTai.FirstOrDefault(dt => dt.MaDeTai.Equals(maSo, StringComparison.OrdinalIgnoreCase));
        }

        public List<DeTaiDTO> TimKiemTheoTen(string ten)
        {
            return _danhSachDeTai.Where(dt => dt.TenDeTai.ToLower().Contains(ten.ToLower())).ToList();
        }

        public List<DeTaiDTO> TimKiemTheoChuTri(string chutri)
        {
            return _danhSachDeTai.Where(dt => dt.ChuTriDeTai.ToLower().Contains(chutri.ToLower())).ToList();
        }

        public List<DeTaiDTO> TimKiemTheoTenGV(string tenGV)
        {
            return _danhSachDeTai.Where(dt => dt.GiangVienHD.ToLower().Contains(tenGV.ToLower())).ToList();
        }

        public double TinhKinhPhi(DeTaiDTO dt)
        {
            double kinhPhiGoc = 0;
            double kinhPhiHoTro = 0;

            if (dt is DeTaiLyThuyetDTO dtlt)
            {
                kinhPhiGoc = dtlt.ApDungThucTe ? 15000000 : 8000000;
            }
            else if (dt is DeTaiKinhTeDTO dtkt)
            {
                kinhPhiHoTro = dtkt.TinhKinhPhiHoTro();
                kinhPhiGoc = dtkt.SoCauHoi > 100 ? 12000000 : 7000000;
            }
            else if (dt is DeTaiCongNgheDTO dtcn)
            {
                kinhPhiHoTro = dtcn.TinhKinhPhiHoTro();
                if (dtcn.MoiTruong.Equals("Web", StringComparison.OrdinalIgnoreCase) || dtcn.MoiTruong.Equals("Mobile", StringComparison.OrdinalIgnoreCase))
                    kinhPhiGoc = 15000000;
                else if (dtcn.MoiTruong.Equals("Window", StringComparison.OrdinalIgnoreCase))
                    kinhPhiGoc = 10000000;
            }

            return (kinhPhiGoc + kinhPhiHoTro) * HeSoTangKinhPhi;
        }

        public void CapNhatKinhPhiTang10PhanTram()
        {
            HeSoTangKinhPhi *= 1.10;
        }

        public List<DeTaiDTO> LayDSTheoKinhPhi(double nguongtien)
        {
            return _danhSachDeTai.Where(dt => TinhKinhPhi(dt) > nguongtien).ToList();
        }

        public List<DeTaiLyThuyetDTO> GetDeTaiLyThuyetCoKhaNangTrienKhai()
        {
            return _danhSachDeTai.OfType<DeTaiLyThuyetDTO>().Where(dt => dt.ApDungThucTe).ToList();
        }

        public List<DeTaiKinhTeDTO> GetDSKinhTeTheoSoCauHoi(int soCauHoi)
        {
            return _danhSachDeTai.OfType<DeTaiKinhTeDTO>().Where(dt => dt.SoCauHoi > soCauHoi).ToList();
        }

        public List<DeTaiDTO> GetDSTheoThoiGian(int soThang)
        {
            return _danhSachDeTai.Where(dt => (dt.ThoiGianKetThuc - dt.ThoiGianBatDau).TotalDays > soThang * 30.44).ToList();
        }
    }
}