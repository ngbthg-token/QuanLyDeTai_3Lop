using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DTO_QLDeTai
{
    public class DeTaiLyThuyetDTO : DeTaiDTO
    {
        private bool apDungThucTe;
        public bool ApDungThucTe
        {
            get { return apDungThucTe; }
            set { apDungThucTe = value; }
        }

        public DeTaiLyThuyetDTO() : base()
        {
            ApDungThucTe = false;
        }

        public DeTaiLyThuyetDTO(string ctri, string gv, string ma, string ten, DateTime bd, DateTime kt, bool apDungThucTe)
            : base(ctri, gv, ma, ten, bd, kt)
        {
            this.ApDungThucTe = apDungThucTe;
        }
    }
}
