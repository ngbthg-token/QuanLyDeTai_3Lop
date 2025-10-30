using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTO_QLDeTai
{
    public abstract class DeTaiDTO
    {
        protected string chuTriDeTai;
        protected string giangVienHD;
        protected string maDeTai;
        protected string tenDeTai;
        protected DateTime thoiGianBatDau;
        protected DateTime thoiGianKetThuc;
        public string ChuTriDeTai { get; set; }
        public string GiangVienHD { get; set; }
        public string MaDeTai { get; set; }
        public string TenDeTai { get; set; }
        public DateTime ThoiGianBatDau { get; set; }
        public DateTime ThoiGianKetThuc { get; set; }

        public DeTaiDTO()
        {
            ChuTriDeTai = string.Empty;
            GiangVienHD = string.Empty;
            MaDeTai = string.Empty;
            TenDeTai = string.Empty;
            ThoiGianBatDau = DateTime.Now;
            ThoiGianKetThuc = DateTime.Now;
        }

        public DeTaiDTO(string chuTriDeTai, string giangVienHD, string maDeTai, string tenDeTai, DateTime thoiGianBatDau, DateTime thoiGianKetThuc)
        {
            this.ChuTriDeTai = chuTriDeTai;
            this.GiangVienHD = giangVienHD;
            this.MaDeTai = maDeTai;
            this.TenDeTai = tenDeTai;
            this.ThoiGianBatDau = thoiGianBatDau;
            this.ThoiGianKetThuc = thoiGianKetThuc;
        }
    }
}
