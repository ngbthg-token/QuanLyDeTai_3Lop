using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DTO_QLDeTai
{
    public class DeTaiKinhTeDTO : DeTaiDTO, IKinhPhiHoTro
    {
        private int soCauHoi;
        public int SoCauHoi
        {
            get { return soCauHoi; }
            set { soCauHoi = value; }
        }

        public DeTaiKinhTeDTO() : base()
        {
            SoCauHoi = 0;
        }

        public DeTaiKinhTeDTO(string ctri, string gv, string ma, string ten, DateTime bd, DateTime kt, int soCauHoi)
            : base(ctri, gv, ma, ten, bd, kt)
        {
            this.SoCauHoi = soCauHoi;
        }

        public double TinhKinhPhiHoTro()
        {
            if (SoCauHoi > 100)
                return SoCauHoi * 550;
            return SoCauHoi * 450;
        }
    }
}
