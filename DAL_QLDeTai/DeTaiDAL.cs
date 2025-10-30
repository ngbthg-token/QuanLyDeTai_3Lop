using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using DTO_QLDeTai;

namespace DAL_QLDeTai
{
    public class DeTaiDAL
    {
        public List<DeTaiDTO> DocFileXML(string tenFile)
        {
            List<DeTaiDTO> danhSachKetQua = new List<DeTaiDTO>();
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(tenFile);
                XmlNodeList nodeList = doc.SelectNodes("/DSDeTai/DeTai");

                if (nodeList == null) return danhSachKetQua;

                foreach (XmlNode node in nodeList)
                {
                    DeTaiDTO dt;
                    int loai = int.Parse(node["Loai"].InnerText);
                    string ct = node["ChuTri"].InnerText;
                    string ten = node["Ten"].InnerText;
                    string ma = node["Ma"].InnerText;
                    string gv = node["GV"].InnerText;
                    DateTime bd = DateTime.Parse(node["BD"].InnerText);
                    DateTime kt = DateTime.Parse(node["KT"].InnerText);

                    if (loai == 1)
                    {
                        bool ad = node["ApDung"].InnerText.Equals("1");
                        dt = new DeTaiLyThuyetDTO(ct, gv, ma, ten, bd, kt, ad);
                    }
                    else if (loai == 2)
                    {
                        int sch = int.Parse(node["SoCauHoi"].InnerText);
                        dt = new DeTaiKinhTeDTO(ct, gv, ma, ten, bd, kt, sch);
                    }
                    else
                    {
                        string mt = node["Moitruong"].InnerText;
                        dt = new DeTaiCongNgheDTO(ct, gv, ma, ten, bd, kt, mt);
                    }
                    danhSachKetQua.Add(dt);
                }
            }
            catch (FileNotFoundException)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"LỖI: Không tìm thấy file {tenFile}. Hãy đảm bảo file XML nằm đúng vị trí.");
                Console.ResetColor();
                return new List<DeTaiDTO>();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"LỖI khi đọc file XML: {ex.Message}");
                Console.ResetColor();
                return new List<DeTaiDTO>();
            }
            return danhSachKetQua;
        }


        private void TaoVaThemNode(XmlDocument doc, XmlElement parent, string tenNode, string giaTri)
        {
            XmlElement node = doc.CreateElement(tenNode);
            node.InnerText = giaTri;
            parent.AppendChild(node);
        }

        public bool LuuFileXML(string tenFile, List<DeTaiDTO> danhSachDeTai)
        {
            try
            {
                XmlDocument doc = new XmlDocument();

                XmlElement root = doc.CreateElement("DSDeTai");
                doc.AppendChild(root);

                foreach (var dt in danhSachDeTai)
                {

                    XmlElement deTaiNode = doc.CreateElement("DeTai");


                    TaoVaThemNode(doc, deTaiNode, "ChuTri", dt.ChuTriDeTai);
                    TaoVaThemNode(doc, deTaiNode, "Ten", dt.TenDeTai);
                    TaoVaThemNode(doc, deTaiNode, "Ma", dt.MaDeTai);
                    TaoVaThemNode(doc, deTaiNode, "GV", dt.GiangVienHD);
                    TaoVaThemNode(doc, deTaiNode, "BD", dt.ThoiGianBatDau.ToString("yyyy/MM/dd"));
                    TaoVaThemNode(doc, deTaiNode, "KT", dt.ThoiGianKetThuc.ToString("yyyy/MM/dd"));


                    if (dt is DeTaiLyThuyetDTO dtlt)
                    {
                        TaoVaThemNode(doc, deTaiNode, "Loai", "1");
                        TaoVaThemNode(doc, deTaiNode, "ApDung", dtlt.ApDungThucTe ? "1" : "0");
                    }
                    else if (dt is DeTaiKinhTeDTO dtkt)
                    {
                        TaoVaThemNode(doc, deTaiNode, "Loai", "2");
                        TaoVaThemNode(doc, deTaiNode, "SoCauHoi", dtkt.SoCauHoi.ToString());
                    }
                    else if (dt is DeTaiCongNgheDTO dtcn)
                    {
                        TaoVaThemNode(doc, deTaiNode, "Loai", "3");
                        TaoVaThemNode(doc, deTaiNode, "Moitruong", dtcn.MoiTruong);
                    }


                    root.AppendChild(deTaiNode);
                }

                doc.Save(tenFile);
                return true;
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"LỖI khi ghi file XML: {ex.Message}");
                Console.ResetColor();
                return false;
            }
        }
    }
}