using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DTO_QLDeTai
{
    public class DeTaiCongNgheDTO : DeTaiDTO, IKinhPhiHoTro
    {
        private string moiTruong;
        public string MoiTruong
        {
            get { return moiTruong; } set { moiTruong = value; }
        }

        public DeTaiCongNgheDTO() : base()
        {
            MoiTruong = string.Empty;
        }

        public DeTaiCongNgheDTO(string ctri, string gv, string ma, string ten, DateTime bd, DateTime kt, string moiTruong)
            : base(ctri, gv, ma, ten, bd, kt)
        {
            this.MoiTruong = moiTruong;
        }

        public double TinhKinhPhiHoTro()
        {
            if (MoiTruong.Equals("Mobile", StringComparison.OrdinalIgnoreCase))
                return 1000000;
            else if (MoiTruong.Equals("Web", StringComparison.OrdinalIgnoreCase))
                return 800000;
            else if (MoiTruong.Equals("Window", StringComparison.OrdinalIgnoreCase))
                return 500000;
            return 0;
        }
    }
}