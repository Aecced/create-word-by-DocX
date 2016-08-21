﻿using CreateWord.log;
using CreateWord.model;
using CreateWord.parcels;
using CreateWord.table;
using Novacode;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreateWord
{
    class Program
    {
        string WordPath = Environment.CurrentDirectory + "\\汇编文档.docx"; //文档路径
        List<FDXXtbl> lstFM;
        List<Parcelmodel> lstp;


        static string CompanyName = "";
        static void Main(string[] args)
        {
            try
            {
                CompanyName = args[0].ToString();
                Program.show();
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(typeof(Program), ex);
            }
        }

        /// <summary>
        /// 为了能够在静态的Main函数中调用非静态的createword函数
        /// </summary>
        public static void show()
        {
            try
            {
                new Program().createword();
            }
            catch (System.Exception ex)
            {
                LogHelper.WriteLog(typeof(Program), ex);
            }

        }

        /// <summary>
        /// 生成汇编文档
        /// </summary>
        void createword()
        {
            try
            {
                lstFM = FDXXtbl.GetInfo();
                lstp = FDXXtbl.Parcels(lstFM);
                using (DocX document = DocX.Create(WordPath))
                {
                    Addcover(document);//添加封面
                    document.InsertSectionPageBreak(true);  //分节符

                    Addtoc(document);//添加目录
                    document.InsertSectionPageBreak(true);  //分节符

                    Addintro(document);//添加概述

                    Addloc(document);//添加位置分布图

                    Addfdxx(document);//添加房地信息统计

                    parcelHelper phelper = new parcelHelper("通湖路699号地块");//添加各个地块的信息
                    phelper.insertInfo(document, lstFM, lstp);
                    
                    document.Save();
                }
                Console.WriteLine("创建文档成功！");
            }
            catch (System.Exception ex)
            {
                LogHelper.WriteLog(typeof(Program), ex);
                Console.WriteLine("创建文档失败！请查看日志！");
            }
        }


        #region 各个模块
        /// <summary>
        /// 添加封面
        /// </summary>
        /// <param name="document"></param>
        void Addcover(DocX document)
        {
            int i = 0;
            Paragraph title = document.InsertParagraph();
            try
            {
                while (i < 13) { i++; title.AppendLine(); }//空13行
                //标题
                title.Append(CompanyName + "非生产性房产资源汇编").FontSize(48).Bold().Alignment = Alignment.center;

                //日期
                Paragraph _date = document.InsertParagraph();
                i = 0;
                while (i < 13) { i++; _date.AppendLine(); }//空13行
                _date.Append(NumberToChinese(DateTime.Now.Year) + "年" + NumberToChinese(DateTime.Now.Month) + "月")
                    .FontSize(22)
                    .Bold()
                    .Alignment = Alignment.center;
                document.DifferentFirstPage = true;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(typeof(Program), ex);
            }

        }


        /// <summary>
        /// 添加目录
        /// </summary>
        /// <param name="document"></param>
        void Addtoc(DocX document)
        {
            try
            {
                document.InsertTableOfContents("目录", TableOfContentsSwitches.O | TableOfContentsSwitches.U | TableOfContentsSwitches.Z | TableOfContentsSwitches.H, "Heading3");
            }
            catch (System.Exception ex)
            {
                LogHelper.WriteLog(typeof(Program), ex);
            }

        }

        /// <summary>
        /// 添加概述模块
        /// </summary>
        /// <param name="document"></param>
        void Addintro(DocX document)
        {
            try
            {
                var h1 = document.InsertParagraph("概述");
                //h1.Font(new FontFamily("宋体")).FontSize(16).Bold();
                h1.StyleName = "Heading1";

                string s = txt.txtHelper.readtxt(Environment.CurrentDirectory + "\\公司\\国网江苏省电力公司" + CompanyName + "\\位介绍.txt");
                Paragraph p = document.InsertParagraph(s);
                p.Font(new FontFamily("宋体")).FontSize(14);

                Picture p1 = picture.picHelper.getPic(document, Environment.CurrentDirectory + "\\公司\\国网江苏省电力公司" + CompanyName + "\\单位介绍图.jpg", 330, 650);
                Paragraph pic = document.InsertParagraph();
                pic.AppendPicture(p1).Alignment = Alignment.center;
            }
            catch (System.Exception ex)
            {
                LogHelper.WriteLog(typeof(Program), ex);
            }
        }

        /// <summary>
        /// 添加位置分布图模块
        /// </summary>
        /// <param name="document"></param>
        void Addloc(DocX document)
        {
            try
            {
                var h2 = document.InsertParagraph("位置分布图");
                // h1.Font(new FontFamily("宋体")).FontSize(16).Bold();
                h2.StyleName = "Heading1";

                Picture p1 = picture.picHelper.getPic(document, Environment.CurrentDirectory + "\\公司\\国网江苏省电力公司" + CompanyName + "\\位置分布图.jpg", 230, 650);
                Paragraph pic = document.InsertParagraph();
                pic.AppendPicture(p1).Alignment = Alignment.center;
            }
            catch (System.Exception ex)
            {
                LogHelper.WriteLog(typeof(Program), ex);
            }
        }

        /// <summary>
        /// 添加房地信息统计模块
        /// </summary>
        /// <param name="document"></param>
        void Addfdxx(DocX document)
        {
            try
            {
                var h3 = document.InsertParagraph("房地信息统计");
                //h3.Font(new FontFamily("宋体")).FontSize(16).Bold();
                h3.StyleName = "Heading1";
                //文字描述
                var p = document.InsertParagraph(CompanyName);
                p.Font(new FontFamily("宋体")).FontSize(14);
                p.Append("市公司现有各类用房");
                p.AppendBookmark("各类用房栋数");
                p.Append("栋，占地总面积");
                p.AppendBookmark("占地总面积");
                p.Append("平方米,总建筑面积");
                p.AppendBookmark("总建筑面积");
                p.Append("平方米。其中");
                p.AppendBookmark("各类用房面积");
                p.Append("；建成投运10年内的房屋面积为");
                p.AppendBookmark("十年内房屋面积");
                p.Append("平方米，建成投运10-20年的房屋面积为");
                p.AppendBookmark("十到二十年内房屋面积");
                p.Append("平方米，建成投运30年以上的房屋面积为");
                p.AppendBookmark("三十年以上房屋面积");
                p.Append("平方米。");
                finishBM(document);//完成书签内容
                //表格描述
                var tbltitle = document.InsertParagraph("房地信息汇总表").Alignment = Alignment.center;
                Table t = tableHelper.Template(document);
                t = tableHelper.inserttable(t, lstFM);
                t = tableHelper.combineCells(t, lstp);
                document.InsertTable(t);
            }
            catch (System.Exception ex)
            {
                LogHelper.WriteLog(typeof(Program), ex);
            }
        }
        #endregion

        /// <summary>
        /// 阿拉伯数字变中文数字
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        string NumberToChinese(int number)
        {
            string outString = string.Empty;
            char d;
            try
            {


                if (number > 12)//年份
                {
                    string year = number.ToString();
                    foreach (char s in year)
                    {
                        d = ' ';
                        switch (s)
                        {
                            case '0': d = '零'; break;
                            case '1': d = '一'; break;
                            case '2': d = '二'; break;
                            case '3': d = '三'; break;
                            case '4': d = '四'; break;
                            case '5': d = '五'; break;
                            case '6': d = '六'; break;
                            case '7': d = '七'; break;
                            case '8': d = '八'; break;
                            case '9': d = '九'; break;
                        }
                        outString += d;
                    }
                }
                else//月份
                {
                    switch (number)
                    {
                        case 1: outString = "一"; break;
                        case 2: outString = "二"; break;
                        case 3: outString = "三"; break;
                        case 4: outString = "四"; break;
                        case 5: outString = "五"; break;
                        case 6: outString = "六"; break;
                        case 7: outString = "七"; break;
                        case 8: outString = "八"; break;
                        case 9: outString = "九"; break;
                        case 10: outString = "十"; break;
                        case 11: outString = "十一"; break;
                        case 12: outString = "十二"; break;
                    }
                }

            }
            catch (System.Exception ex)
            {
                LogHelper.WriteLog(typeof(Program), ex);
            }
            return outString;
        }

        /// <summary>
        /// 完成书签内容
        /// </summary>
        /// <param name="document"></param>
        void finishBM(DocX document)
        {
            FDXXsentence1 fs1 = new FDXXsentence1();
            List<FDXXsentence2> lstFS2 = new List<FDXXsentence2>();
            FDXXsentence3 fs3 = new FDXXsentence3();
            string temp = "";
            try
            {
                fs1 = FDXXsentence1.GetInfo();
                lstFS2 = FDXXsentence2.GetInfo();
                fs3 = FDXXsentence3.GetInfo();

                document.Bookmarks["各类用房栋数"].SetText("" + fs1.count);
                document.Bookmarks["占地总面积"].SetText("" + fs1.ZDZMJ);
                document.Bookmarks["总建筑面积"].SetText("" + fs1.ZJZMJ);

                foreach (FDXXsentence2 fs2 in lstFS2)
                {
                    temp += fs2.GNGL;
                    temp += "" + fs2.GNGL_MJ;
                    temp += "平方米，";
                }
                temp = temp.Trim('，');
                document.Bookmarks["各类用房面积"].SetText(temp);

                document.Bookmarks["十年内房屋面积"].SetText("" + fs3.FWMJ_10);
                document.Bookmarks["十到二十年内房屋面积"].SetText("" + fs3.FWMJ_20);
                document.Bookmarks["三十年以上房屋面积"].SetText("" + fs3.FWMJ_30);
            }
            catch (System.Exception ex)
            {
                LogHelper.WriteLog(typeof(Program), ex);
            }
        }

    }
}
